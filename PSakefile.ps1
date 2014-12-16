#
# WARNING: This file must stay at the root of the repository.
#
# See Make.proj for a description of all available options.
# NB: To speed up things a bit, we do not call the Clean target most of time.

Properties {
    $Project = "$PSScriptRoot\Make.proj"

    $BuildArgs = '/verbosity:minimal', '/maxcpucount', '/nodeReuse:false'
}

Task default -depends Tests

# ==============================================================================
# Test and Analysis Targets
# ==============================================================================

Task CI {
    # Continuous Integration Build. Mimic the Retail target, with detailed log.
    MSBuild $Project '/v:d', '/m', '/nr:false', '/t:FullBuild', 
        '/p:Configuration=Release;SignAssembly=true;SkipPrivateProjects=true;VisibleInternals=false'
}
Task Tests {
    MSBuild $Project $BuildArgs '/t:Test', '/p:BuildGeneratedVersion=false'
}
Task Verifications {
    MSBuild $Project $BuildArgs '/t:Verify', '/p:BuildGeneratedVersion=false'
}
Task SourceAnalysis {
    MSBuild $Project $BuildArgs '/p:SourceAnalysisEnabled=true;BuildGeneratedVersion=false'
}
# Code Analysis is slow. The default is to only analyze public projects. 
# WARNING: Do not change VisibleInternals to true.
Task CodeAnalysis {
    MSBuild $Project $BuildArgs '/p:RunCodeAnalysis=true;BuildGeneratedVersion=false;SkipPrivateProjects=true;VisibleInternals=false'
}
Task FullCodeAnalysis {
    MSBuild $Project $BuildArgs '/p:RunCodeAnalysis=true;BuildGeneratedVersion=false;VisibleInternals=false'
}
# Code Contracts Analysis is really slow. The default is to only analyze public projects. 
Task CodeContractsAnalysis {
    MSBuild $Project $BuildArgs '/p:Configuration=CodeContracts;BuildGeneratedVersion=false;SkipPrivateProjects=true'
}
Task FullCodeContractsAnalysis {
    MSBuild $Project $BuildArgs '/p:Configuration=CodeContracts;BuildGeneratedVersion=false'
}
# Security Analysis is slow. The default is to only analyze public projects. 
Task SecurityAnalysis {
    MSBuild $Project $BuildArgs '/t:SecAnnotate', '/p:BuildGeneratedVersion=false;SkipPrivateProjects=true'
}
Task FullSecurityAnalysis {
    MSBuild $Project $BuildArgs '/t:SecAnnotate', '/p:BuildGeneratedVersion=false'
}


# ==============================================================================
# Retail Targets
# ==============================================================================

Task Retail -depends FullClean {
    MSBuild $Project $BuildArgs '/t:FullRebuild', '/p:Retail=true;SkipPrivateProjects=true'
}
# At first, I wanted to also offer the possibility of packaging a single project
# but then we won't be able to run the test suite. A better solution is to use
# the solution files, even if this is not perfect. For instance, the solution 
# might have been incorrectly configured and only a subset of the projects
# might be built.
Task RetailCore -depends FullClean {
    MSBuild $Project $BuildArgs '/t:FullRebuild', '/p:Retail=true;SolutionFile=.\Narvalo (Core).sln'
}
Task RetailMiscs -depends FullClean {
    MSBuild $Project $BuildArgs '/t:FullRebuild', '/p:Retail=true;SolutionFile=.\Narvalo (Miscs).sln'
}
Task RetailMvp -depends FullClean {
    MSBuild $Project $BuildArgs '/t:FullRebuild', '/p:Retail=true;SolutionFile=.\Narvalo (Mvp).sln' 
}

# ==============================================================================
# Visual Studio Targets
# ==============================================================================

Task MainSolution {
    MSBuild $Project $BuildArgs '/p:Lean=true;SolutionFile=.\Narvalo.sln'
}
Task CoreSolution {
    MSBuild $Project $BuildArgs '/p:Lean=true;SolutionFile=.\Narvalo (Core).sln'
}
Task MiscsSolution {
    MSBuild $Project $BuildArgs '/p:Lean=true;SolutionFile=.\Narvalo (Miscs).sln'
}
Task MvpSolution {
    MSBuild $Project $BuildArgs '/p:Lean=true;SolutionFile=.\Narvalo (Mvp).sln'
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

