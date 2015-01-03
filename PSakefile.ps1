Properties {
    Assert ($verbosity -ne $null) "`$verbosity should not be null. E.g. run with -Parameters @{ 'verbosity' = 'minimal'; }"
    
    $WorkRoot = Get-RepositoryPath 'work' 

    # Console parameters.
    $BuildArgs = "/verbosity:$verbosity", '/maxcpucount', '/nodeReuse:false'

    $Everything = Get-RepositoryPath 'tools', 'Make.proj'
    $Foundations = Get-RepositoryPath 'tools', 'Make.Foundations.proj'
    
    $GitCommitHash = Get-GitCommitHash

    # Packaging properties:
    # - Release configuration
    # - Generate assembly versions (necessary for NuGet packaging)
    # - Set the git commit hash
    # - Sign assemblies
    # - Do not skip the generation of the Code Contracts reference assembly
    # - Unconditionally hide internals (implies no white-box testing)
    $PackagingProps = `
        '/p:Configuration=Release',
        '/p:BuildGeneratedVersion=true',
        "/p:GitCommitHash=$GitCommitHash",
        '/p:SignAssembly=true',
        '/p:SkipCodeContractsReferenceAssembly=false',
        '/p:VisibleInternals=false'

    # Default CI properties:
    # - Release configuration
    # - Do not generate assembly versions (=> no need to set the git commit hash)
    # - Do not sign assemblies
    # - Do not skip the generation of the Code Contracts reference assembly
    # - Leak internals to enable all white-box tests.
    $CIProps = `
        '/p:Configuration=Release',
        '/p:BuildGeneratedVersion=false',
        '/p:SignAssembly=false',
        '/p:SkipCodeContractsReferenceAssembly=false',
        '/p:VisibleInternals=true'

    # For static analysis, we hide internals, otherwise we might not truly 
    # analyze the public API.
    $StaticAnalysisProps = $CIProps, '/p:VisibleInternals=false'

    # Retail targets:
    # - Rebuild all
    # - Verify Portable Executable (PE) format
    # - Run Xunit tests
    # - Package
    $RetailTargets = '/t:Rebuild;PEVerify;Xunit;Package'
}

TaskTearDown {
    if ($LastExitCode -ne 0) {
        Write-Host ''
        Write-Host 'Build failed.' -BackgroundColor Red -ForegroundColor Yellow
        Write-Host ''
        Exit 1
    }
}

Task Default -depends FastBuild

# ------------------------------------------------------------------------------
# Continuous Integration and development tasks
# ------------------------------------------------------------------------------

# - FastBuild, build then run Xunit tests in Debug configuration
# - CI, default CI build
# - Mock-Retail, mimic the Retail task
# - CodeAnalysis (slow)
# - CodeContractsAnalysis (very slow)
# - SecurityAnalysis (very slow)

Task FastBuild {
    MSBuild $Foundations $BuildArgs $CIProps `
        '/t:Xunit', 
        '/p:Configuration=Debug',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true'
}

Task CI {
    # Perform the following operations:
    # - Run Source Analysis
    # - Build all projects
    # - Verify Portable Executable (PE) format
    # - Run Xunit tests, including white-box tests
    MSBuild $Everything $BuildArgs $CIProps `
        '/t:Build;PEVerify;Xunit',
        '/p:SourceAnalysisEnabled=true'
}

Task Mock-Retail {
    MSBuild $Foundations $BuildArgs $RetailTargets $PackagingProps
}

Task CodeAnalysis {
    MSBuild $Everything $BuildArgs $StaticAnalysisProps `
        '/t:Build', 
        '/p:RunCodeAnalysis=true',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true'
}

Task CodeContractsAnalysis {
    MSBuild $Foundations $BuildArgs $StaticAnalysisProps `
        '/t:Build',
        '/p:Configuration=CodeContracts'
}

Task SecurityAnalysis {
    MSBuild $Foundations $BuildArgs $StaticAnalysisProps `
        '/t:SecAnnotate',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true'
}

# ------------------------------------------------------------------------------
# Retail tasks
# ------------------------------------------------------------------------------

# Retail tasks:
# - Retail, package all
# - Retail-Core, only package Core projects
# - Retail-Mvp, only package Mvp projects
# - Retail-Build, only package Narvalo.Build project

Task Retail {
    MSBuild $Foundations $BuildArgs $RetailTargets $PackagingProps `
        '/p:Retail=true'
}

Task Retail-Core {
    MSBuild $Foundations $BuildArgs $RetailTargets $PackagingProps `
        '/p:Retail=true',
        '/p:Filter=_Core_'
}

Task Retail-Mvp {
    MSBuild $Foundations $BuildArgs $RetailTargets $PackagingProps `
        '/p:Retail=true',
        '/p:Filter=_Mvp_'
}

Task Retail-Build {
    MSBuild $Foundations $BuildArgs $RetailTargets $PackagingProps `
        '/p:Retail=true',
        '/p:Filter=_Build_'
}

# ------------------------------------------------------------------------------
# MyGet tasks
# ------------------------------------------------------------------------------

Task Clean-MyGet {
    $script:MyGetDir = Join-Path $WorkRoot 'myget'
    $script:MyGetPkg = Join-Path $WorkRoot 'myget.7z'

    if (Test-Path $MyGetDir) {
        Remove-Item $MyGetDir -Force -Recurse
    }
    if (Test-Path $MyGetPkg) {
        Remove-Item $MyGetPkg -Force
    }
}

Task Build-MyGet {
    # Force the value of "VisualStudioVersion", otherwise MSBuild won't publish the project on build.
    # Cf. http://sedodream.com/2012/08/19/VisualStudioProjectCompatabilityAndVisualStudioVersion.aspx.
    MSBuild $BuildArgs `
        (Get-RepositoryPath 'tools', 'MyGet', 'MyGet.csproj') `
        '/t:Clean;Build',
        '/p:Configuration=Release;PublishProfile=NarvaloOrg;DeployOnBuild=true;VisualStudioVersion=12.0'
}

Task Publish-MyGet -depends Clean-MyGet, Build-MyGet {
    . (Install-7Zip) -mx9 a $script:MyGetPkg $script:MyGetDir | Out-Null

    Write-Host "A ready to publish package for MyGet may be found here: '$script:MyGetPkg'." -ForegroundColor Green
}

Task MyGet -depends Publish-MyGet

# ------------------------------------------------------------------------------
# Miscs
# ------------------------------------------------------------------------------

# Delete work directory.
Task FullClean {
    if (Test-Path $WorkRoot) {
        Remove-Item $WorkRoot -Force -Recurse
    }
}

