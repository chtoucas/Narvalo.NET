Properties {
    # WARNING: This file is supposed to be at the root of the repository.
    $RepositoryRoot = $PSScriptRoot

    # Console parameters.
    # NB: $verbosity should have been already set in make.ps1.
    if ($verbosity -eq $null) { $verbosity = 'minimal' }
    $BuildArgs = "/verbosity:$verbosity", '/maxcpucount', '/nodeReuse:false'

    $Everything = "$RepositoryRoot\tools\Make.proj"
    $Foundations = "$RepositoryRoot\tools\Make.Foundations.proj"
    
    # Packaging properties:
    # - Release configuration
    # - Generate assembly versions (necessary for NuGet packaging)
    # - Sign assemblies
    # - Unconditionally hide internals (implies no white-box testing)
    $PackagingProps = `
        '/p:Configuration=Release',
        '/p:BuildGeneratedVersion=true',
        '/p:SignAssembly=true',
        '/p:VisibleInternals=false'

    # Default CI properties:
    # - Release configuration
    # - Do not generate assembly versions
    # - Do not sign assemblies
    # - Leak internals to enable all white-box tests.
    $CIProps = `
        '/p:Configuration=Release',
        '/p:BuildGeneratedVersion=false',
        '/p:SignAssembly=false',
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

# ==============================================================================

# Continuous Integration and development tasks:
# - FastBuild, build then run Xunit tests in Debug configuration
# - CI, default CI build
# - Mock-Retail, mimic the Retail task
# - CodeAnalysis (slow)
# - CodeContractsAnalysis (very slow)
# - SecurityAnalysis (very slow)

Task FastBuild {
    MSBuild $Foundations $BuildArgs `
        '/p:Configuration=Debug',
        '/p:BuildGeneratedVersion=false',
        '/p:SignAssembly=false',
        '/p:VisibleInternals=true',
        '/t:Build;Xunit'
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
    MSBuild $Everything $BuildArgs $StaticAnalysisProps '/t:Build', '/p:RunCodeAnalysis=true'
}

Task CodeContractsAnalysis {
    MSBuild $Foundations $BuildArgs $StaticAnalysisProps '/t:Build', '/p:Configuration=CodeContracts'
}

Task SecurityAnalysis {
    MSBuild $Foundations $BuildArgs $StaticAnalysisProps '/t:SecAnnotate'
}

# ==============================================================================

# Retail tasks:
# - Retail, package all
# - Retail-Core, only package Core projects
# - Retail-Mvp, only package Mvp projects
# - Retail-Build, only package Narvalo.Build project

Task Retail {
    MSBuild $Foundations $BuildArgs $RetailTargets $PackagingProps '/p:Retail=true'
}

Task Retail-Core {
    MSBuild $Foundations $BuildArgs $RetailTargets $PackagingProps '/p:Retail=true;Filter=_Core_'
}

Task Retail-Mvp {
    MSBuild $Foundations $BuildArgs $RetailTargets $PackagingProps '/p:Retail=true;Filter=_Mvp_'
}

Task Retail-Build {
    MSBuild $Foundations $BuildArgs $RetailTargets $PackagingProps '/p:Retail=true;Filter=_Build_'
}

# ==============================================================================

# Delete work directory.
Task FullClean {
    $workDir = "$RepositoryRoot\work\"

    if (Test-Path $workDir) {
        Remove-Item $workDir -Force -Recurse
    }
}

