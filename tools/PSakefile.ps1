
# We force the framework to be sure we use the build tools v12.0.
# Required by the Build-MyGet target.
Framework '4.5.1x64'

# ------------------------------------------------------------------------------
# Properties
# ------------------------------------------------------------------------------

Properties {
    Assert ($verbosity -ne $null) "`$verbosity should not be null, e.g. run with -Parameters @{ 'verbosity' = 'minimal'; }"
    Assert ($retail -ne $null) "`$retail should not be null, e.g. run with -Parameters @{ 'retail' = $true; }"

    $WorkRoot = Get-ProjectItem 'work' 
    $PackagesDir = Get-ProjectItem 'work\packages'
    
    $MyGetDir = Join-Path $WorkRoot 'myget'
    $MyGetPkg = Join-Path $WorkRoot 'myget.7z'

    $GitCommitHash = ''

    # Console options.
    $Opts = '/nologo', "/verbosity:$verbosity", '/maxcpucount', '/nodeReuse:false'

    $Everything = Get-ProjectItem 'tools\Make.proj' -Resolve
    $Foundations = Get-ProjectItem 'tools\Make.Foundations.proj' -Resolve
    
    # Packaging properties:
    # - Release configuration
    # - Generate assembly versions (necessary for NuGet packaging)
    # - Sign assemblies
    # - Do not skip the generation of the Code Contracts reference assembly
    # - Unconditionally hide internals (implies no white-box testing)
    $PackagingProps = `
        '/p:Configuration=Release',
        '/p:BuildGeneratedVersion=true',
        "/p:Retail=$retail",
        '/p:SignAssembly=true',
        '/p:SkipCodeContractsReferenceAssembly=false',
        '/p:VisibleInternals=false'

    # Default CI properties:
    # - Release configuration
    # - Do not generate assembly versions
    # - Do not sign assemblies
    # - Do not skip the generation of the Code Contracts reference assembly
    # - Leak internals to enable all white-box tests.
    $CIProps = `
        '/p:Configuration=Release',
        '/p:BuildGeneratedVersion=false',
        "/p:Retail=$retail",
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
    $PackagingTargets = '/t:Rebuild;PEVerify;Xunit;Package'
}

FormatTaskName "Executing Task {0}..."

TaskTearDown {
    if ($LastExitCode -ne 0) {
        Exit-Error 'Build failed.'
    }
}

Task Default -depends FastBuild

# ------------------------------------------------------------------------------
# Continuous Integration and development tasks
# ------------------------------------------------------------------------------

Task FastBuild -Description 'Fast build Foundations then run tests.' {
    MSBuild $Foundations $Opts $CIProps `
        '/t:Xunit', 
        '/p:Configuration=Debug',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true'
}

Task Build -Description 'Build all projects.' {
    MSBuild $Everything $Opts $CIProps '/t:Build'
}

Task FullBuild -Description 'Full build then run tests.' {
    # Perform the following operations:
    # - Run Source Analysis
    # - Build all projects
    # - Verify Portable Executable (PE) format
    # - Run Xunit tests, including white-box tests
    MSBuild $Everything $Opts $CIProps `
        '/t:Build;PEVerify;Xunit',
        '/p:SourceAnalysisEnabled=true'
}

Task FullClean -ContinueOnError -Description 'Delete work directory.' {
    # Sometimes this task fails for some obscure reasons. Maybe the directory is locked?
    if (Test-Path $WorkRoot) {
        Remove-Item $WorkRoot -Force -Recurse -ErrorAction SilentlyContinue
    }
}

Task CodeAnalysis -Description 'Run Code Analysis (slow).' {
    MSBuild $Everything $Opts $StaticAnalysisProps `
        '/t:Build', 
        '/p:RunCodeAnalysis=true',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true'
} -Alias CA

Task CodeContractsAnalysis -Description 'Run Code Contracts Analysis on Foundations (very slow).' {
    MSBuild $Foundations $Opts $StaticAnalysisProps `
        '/t:Build',
        '/p:Configuration=CodeContracts'
} -Alias CC

Task SecurityAnalysis -Description 'Run SecAnnotate on Foundations (slow).' {
    MSBuild $Foundations $Opts $StaticAnalysisProps `
        '/t:SecAnnotate',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true'
} -Alias SA

# ------------------------------------------------------------------------------
# Packaging tasks
# ------------------------------------------------------------------------------

Task Package -depends _Set-GitCommitHash, _Validate-Packaging `
    -Description 'Package all projects.' {
    MSBuild $Foundations $Opts $PackagingTargets $PackagingProps `
        "/p:GitCommitHash=$GitCommitHash"
} -Alias Pack

Task Package-Core -depends _Set-GitCommitHash, _Validate-Packaging `
    -Description 'Package core projects.' {
    MSBuild $Foundations $Opts $PackagingTargets $PackagingProps `
        "/p:GitCommitHash=$GitCommitHash",
        '/p:Filter=_Core_' 
} -Alias PackCore

Task Package-Mvp -depends _Set-GitCommitHash, _Validate-Packaging `
    -Description 'Package MVP projects.' {
    MSBuild $Foundations $Opts $PackagingTargets $PackagingProps `
        "/p:GitCommitHash=$GitCommitHash",
        '/p:Filter=_Mvp_' 
} -Alias PackMvp

Task Package-Build -depends _Set-GitCommitHash, _Validate-Packaging `
    -Description 'Package the Narvalo.Build project.' {
    MSBuild $Foundations $Opts $PackagingTargets $PackagingProps `
        "/p:GitCommitHash=$GitCommitHash",
        '/p:Filter=_Build_' 
} -Alias PackBuild

Task _Validate-Packaging -PreCondition { $retail } `
    -Description 'Validate packaging tasks.' {
    if ($GitCommitHash -eq '') {
        Exit-Error 'When building retail packages, the git commit hash MUST not be empty.'
    }
}

# ------------------------------------------------------------------------------
# Publish tasks
# ------------------------------------------------------------------------------

Task Publish -Description 'Publish all projects.' {
    $nuget = Get-NuGet -Install

    Exit-Error 'Not yet implemented!'

    #& $nuget push "$PackagesDir\*.nupkg"
} -Alias Push

# ------------------------------------------------------------------------------
# MyGet tasks
# ------------------------------------------------------------------------------

Task MyGet-Clean -Description 'Clean MyGet server.' {
    if (Test-Path $MyGetDir) {
        Remove-Item $MyGetDir -Force -Recurse
    }
    if (Test-Path $MyGetPkg) {
        Remove-Item $MyGetPkg -Force
    }
}

Task MyGet-Build -Description 'Build MyGet server.' {
    # Force the value of "VisualStudioVersion", otherwise MSBuild won't publish the project on build.
    # Cf. http://sedodream.com/2012/08/19/VisualStudioProjectCompatabilityAndVisualStudioVersion.aspx.
    MSBuild $Opts `
        (Get-ProjectItem 'tools\MyGet\MyGet.csproj' -Resolve) `
        '/t:Clean;Build',
        '/p:Configuration=Release;PublishProfile=NarvaloOrg;DeployOnBuild=true;VisualStudioVersion=12.0'
}

Task MyGet-Zip -Description 'Zip MyGet server.' {
    . (Get-7Zip -Install) -mx9 a $MyGetPkg $MyGetDir | Out-Null

    if (!$?) {
        Exit-Error 'Unabled to create the Zip package.'
    }

    Write-Host "A ready to publish zip file for MyGet may be found here: '$MyGetPkg'." -ForegroundColor Green
}

Task MyGet -depends MyGet-Clean, MyGet-Build, MyGet-Zip `
    -Description 'Publish MyGet server.'

# ------------------------------------------------------------------------------
# Miscs
# ------------------------------------------------------------------------------

Task Environment -Description 'Display the build environment.' {
    # Running "MSBuild /version" outputs: 
    # >   Microsoft (R) Build Engine, version 12.0.31101.0
    # >   [Microsoft .NET Framework, Version 4.0.30319.34209]
    # >   Copyright (C) Microsoft Corporation. Tous droits réservés.
    # >
    # >   12.0.31101.0
    $infos = (MSBuild '/version') -Split "`n", 4

    $msbuild = $infos[4]
    $netFramework = ($infos[1] -Split ' ', 5)[4].TrimEnd(']')
    $psakeFramework = $psake.context.peek().config.framework
    $psakeVersion = $psake.version

    Write-Host "  MSBuild         v$msbuild"
    Write-Host "  .NET Framework  v$netFramework"
    Write-Host "  PSake           v$psakeVersion"
    Write-Host "  PSake Framework v$psakeFramework"
}

# ------------------------------------------------------------------------------
# Utilities
# ------------------------------------------------------------------------------

Task _Set-GitCommitHash -Description 'Initialize GitCommitHash.' {
    $git = Get-Git

    if ($git -ne $null) {
        $GitCommitHash = $git | Get-GitCommitHash
    }
}