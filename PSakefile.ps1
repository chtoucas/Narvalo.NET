#
# WARNING: This file must stay at the root of the repository.
#
# See Make.proj for a description of all available options.
# NB: To speed up things a bit, we do not call the Clean target most of time.

Properties {
    $DefaultProject = "$PSScriptRoot\tools\Make.proj"
    $Foundations = "$PSScriptRoot\tools\Make.Foundations.proj"

    $BuildArgs = '/verbosity:minimal', '/maxcpucount', '/nodeReuse:false'
}

Task default -depends Tests

# ==============================================================================
# Test and Analysis Targets
# ==============================================================================

Task CI {
    # Continuous Integration Build. Mimic the Retail target, with detailed log.
    MSBuild $Foundations '/v:d', '/m', '/nr:false', '/t:FullBuild',
        '/p:Configuration=Release;SignAssembly=true;VisibleInternals=false'
}

Task FakeRetail {
    MSBuild $Foundations '/v:m', '/m', '/nr:false', '/t:FullBuild',
        '/p:SignAssembly=true;VisibleInternals=false'
}
Task Tests {
    MSBuild $DefaultProject $BuildArgs '/t:Test', '/p:BuildGeneratedVersion=false'
}
Task Verifications {
    MSBuild $DefaultProject $BuildArgs '/t:Verify', '/p:BuildGeneratedVersion=false'
}
Task SourceAnalysis {
    MSBuild $DefaultProject $BuildArgs '/p:SourceAnalysisEnabled=true;BuildGeneratedVersion=false'
}
# Code Analysis is slow. Analysis is only performed on public projects.
# WARNING: Do not change VisibleInternals to true.
Task CodeAnalysis {
    MSBuild $Foundations $BuildArgs '/p:RunCodeAnalysis=true;BuildGeneratedVersion=false;VisibleInternals=false'
}
Task FullCodeAnalysis {
    MSBuild $DefaultProject $BuildArgs '/p:RunCodeAnalysis=true;BuildGeneratedVersion=false;VisibleInternals=false'
}
# Code Contracts Analysis is really slow. Analysis is only performed on public projects.
Task CodeContractsAnalysis {
    MSBuild $Foundations $BuildArgs '/p:Configuration=CodeContracts;BuildGeneratedVersion=false'
}
Task FullCodeContractsAnalysis {
    MSBuild $DefaultProject $BuildArgs '/p:Configuration=CodeContracts;BuildGeneratedVersion=false'
}
# Security Analysis is slow. Analysis is only performed on public projects.
Task SecurityAnalysis {
    MSBuild $Foundations $BuildArgs '/t:SecAnnotate', '/p:BuildGeneratedVersion=false'
}
Task FullSecurityAnalysis {
    MSBuild $DefaultProject $BuildArgs '/t:SecAnnotate', '/p:BuildGeneratedVersion=false'
}


# ==============================================================================
# Retail Targets
# ==============================================================================

Task Retail -depends FullClean {
    MSBuild $Foundations $BuildArgs '/t:FullRebuild', '/p:Retail=true'
}
# At first, I wanted to also offer the possibility of packaging a single project
# but then we won't be able to run the test suite. A better solution is to use
# the solution files, even if this is not perfect. For instance, the solution
# might have been incorrectly configured and only a subset of the projects
# might be built.
Task RetailCore -depends FullClean {
    MSBuild $DefaultProject $BuildArgs '/t:FullRebuild', '/p:Retail=true;CustomSolution=.\Narvalo (Core).sln'
}
Task RetailMiscs -depends FullClean {
    MSBuild $DefaultProject $BuildArgs '/t:FullRebuild', '/p:Retail=true;CustomSolution=.\Narvalo (Miscs).sln'
}
Task RetailMvp -depends FullClean {
    MSBuild $DefaultProject $BuildArgs '/t:FullRebuild', '/p:Retail=true;CustomSolution=.\Narvalo (Mvp).sln'
}

# ==============================================================================
# Visual Studio Targets
# ==============================================================================

Task MainSolution {
    MSBuild $DefaultProject $BuildArgs '/p:Lean=true;CustomSolution=.\Narvalo.sln'
}
Task CoreSolution {
    MSBuild $DefaultProject $BuildArgs '/p:Lean=true;CustomSolution=.\Narvalo (Core).sln'
}
Task MiscsSolution {
    MSBuild $DefaultProject $BuildArgs '/p:Lean=true;CustomSolution=.\Narvalo (Miscs).sln'
}
Task MvpSolution {
    MSBuild $DefaultProject $BuildArgs '/p:Lean=true;CustomSolution=.\Narvalo (Mvp).sln'
}

# ==============================================================================
# Miscs
# ==============================================================================

# Delete work directory.
Task FullClean {
    $workDir = "$PSScriptRoot\work\"

    if (Test-Path $workDir) {
        Remove-Item $workDir -Force -Recurse
    }
}

TaskTearDown {
    if ($LastExitCode -ne 0) {
        Write-Host ''
        Write-Host 'Build failed.' -BackgroundColor Red -ForegroundColor Yellow
        Write-Host ''
        Exit 1
    }
}

