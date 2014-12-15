#
# WARNING: This file must stay at the root of the repository.
#
# See Make.proj for a description of all available options.
# NB: To speed up things a bit, we do not call the Clean target most of time.

Properties {
    $Project = "$PSScriptRoot\Make.proj"

    $BuildArgs = '/verbosity:minimal', '/maxcpucount', '/nodeReuse:false'

    $RetailTargets = 'Clean;Build;Verify;Test;Package'
}

Task default -depends Tests

# ==============================================================================
# Test and Analysis Targets
# ==============================================================================

# Mimic the Retail target, with detailed log.
Task CI {
    MSBuild $Project "/t:$RetailTargets", 
        '/p:Configuration=Release;SignAssembly=true;SkipPrivateProjects=true;VisibleInternals=false', 
        '/verbosity:detailed',
        '/maxcpucount', 
        '/nodeReuse:false'
}

# Run tests.
Task Tests {
    MSBuild $Project $BuildArgs '/t:Test' '/p:BuildGeneratedVersion=false'
}

# Run verifications.
Task Verifications {
    MSBuild $Project $BuildArgs '/t:Verify' '/p:BuildGeneratedVersion=false;SkipPrivateProjects=true'
}

# Run StyleCop and FxCop without visible internals (slow).
Task StandardAnalysis {
    MSBuild $Project $BuildArgs '/t:Build',
        '/p:Configuration:Release;BuildGeneratedVersion=false;SkipPrivateProjects=true;VisibleInternals=false'
}

# Run Code Contracts analysis (slow).
Task CodeContractsAnalysis {
    MSBuild $Project $BuildArgs '/t:Build',
        '/p:Configuration:CodeContracts;BuildGeneratedVersion=false;SkipPrivateProjects=true'
}

# Run extended analysis (slow).
Task ExtendedAnalysis {
    MSBuild $Project $BuildArgs '/t:Analyze' '/p:BuildGeneratedVersion=false;SkipPrivateProjects=true'
}

# ==============================================================================
# Retail Targets
# ==============================================================================

Task Retail -depends FullClean {
    MSBuild $Project $BuildArgs "/t:$RetailTargets" '/p:Retail=true;SkipPrivateProjects=true'
}
# At first, I wanted to also offer the possibility of packaging a single project
# but then we won't be able to run the test suite. A better solution is to use
# the solution files even if this is not perfect. For instance, the solution 
# might have been incorrectly configured and only a subset of the projects
# might be built.
Task RetailCore -depends FullClean {
    MSBuild $Project $BuildArgs "/t:$RetailTargets" '/p:Retail=true;SolutionFile=.\Narvalo (Core).sln'
}
Task RetailMiscs -depends FullClean {
    MSBuild $Project $BuildArgs "/t:$RetailTargets" '/p:Retail=true;SolutionFile=.\Narvalo (Miscs).sln'
}
Task RetailMvp -depends FullClean {
    MSBuild $Project $BuildArgs "/t:$RetailTargets" '/p:Retail=true;SolutionFile=.\Narvalo (Mvp).sln' 
}

# ==============================================================================
# Visual Studio Targets
# ==============================================================================

Task MainSolution {
    MSBuild $Project $BuildArgs '/t:Build' '/p:Lean=true;SolutionFile=.\Narvalo.sln'
}
Task CoreSolution {
    MSBuild $Project $BuildArgs '/t:Build' '/p:Lean=true;SolutionFile=.\Narvalo (Core).sln'
}
Task MiscsSolution {
    MSBuild $Project $BuildArgs '/t:Build' '/p:Lean=true;SolutionFile=.\Narvalo (Miscs).sln'
}
Task MvpSolution {
    MSBuild $Project $BuildArgs '/t:Build' '/p:Lean=true;SolutionFile=.\Narvalo (Mvp).sln'
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

