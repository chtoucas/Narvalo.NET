#
# WARNING: This file must stay at the root of the repository.
#
# See Make.proj for a description of all available options.

Properties {
    $Project = "$PSScriptRoot\Make.proj"
}

Task default -depends Default_Debug

TaskTearDown {
    if ($LastExitCode -ne 0) {
        Write-Host ''
        Write-Host 'Build failed.' -BackgroundColor Red -ForegroundColor Yellow
        Write-Host ''
        Exit 1
    }
}

# Delete work directory.
Task Clean {
    $workDir = "$PSScriptRoot\work\"

    if (Test-Path $workDir) {
        Remove-Item $workDir -Force -Recurse
    }
}

# Run tests for public projects in Debug configuration.
Task Default_Debug {
    MSBuild $Project '/t:Test',
        '/p:BuildGeneratedVersion=false;SkipPrivateProjects=true',
        '/verbosity:minimal', 
        '/maxcpucount', 
        '/nodeReuse:false'
}

# Run tests for public projects in Release configuration without visible internals.
Task Default_ReleaseNoInternals {
    MSBuild $Project '/t:Test',
        '/p:Configuration=Release;BuildGeneratedVersion=false;SkipPrivateProjects=true;VisibleInternals=false',
        '/verbosity:minimal', 
        '/maxcpucount', 
        '/nodeReuse:false'
}

# Create packages for publication: skip private projects, sign assemblies, 
# use Release configuration and unconditionally hide internals.
Task Retail -depends Clean {
    MSBuild $Project '/t:Clean;Build;Verify;Test;Package' 
        '/p:RetailBuild=true;SkipPrivateProjects=true', 
        '/verbosity:minimal', 
        '/maxcpucount', 
        '/nodeReuse:false'
}

#
# Continuous Integration Targets.
#
# REVIEW: To speed up things a bit, we do not call the Clean target?

# Same as Package. Detailed log.
Task CI {
    MSBuild $Project '/t:Build;Verify;Test;Package', 
        '/p:Configuration=Release;SignAssembly=true;SkipPrivateProjects=true;VisibleInternals=false', 
        '/verbosity:detailed',
        '/maxcpucount', 
        '/nodeReuse:false'
}

Task CI_Release {
    MSBuild $Project '/t:Build;Verify;Test', 
        '/p:Configuration=Release;BuildGeneratedVersion=false', 
        '/verbosity:minimal',
        '/maxcpucount', 
        '/nodeReuse:false'
}

Task CI_Debug {
    MSBuild $Project '/t:Build;Verify;Test', 
        '/p:BuildGeneratedVersion=false', 
        '/verbosity:minimal',
        '/maxcpucount', 
        '/nodeReuse:false'
}

Task CI_CodeContracts {
    MSBuild $Project '/t:Build;Verify;Test', 
        '/p:Configuration=CodeContracts;BuildGeneratedVersion=false', 
        '/verbosity:minimal',
        '/maxcpucount', 
        '/nodeReuse:false'
}

#
# Solution Targets.
#
# Lean build then run tests for solutions in Debug configuration.
# Since LeanRun is true, no need to set BuildGeneratedVersion=false.

Task CoreSolution {
    MSBuild $Project '/t:Test',
        '/p:LeanRun=true;SolutionFile=.\Narvalo (Core).sln', 
        '/verbosity:minimal', 
        '/maxcpucount', 
        '/nodeReuse:false'
}

Task MainSolution {
    MSBuild $Project '/t:Test',
        '/p:LeanRun=true;SolutionFile=.\Narvalo.sln', 
        '/verbosity:minimal', 
        '/maxcpucount', 
        '/nodeReuse:false'
}

Task MiscsSolution {
    MSBuild $Project '/t:Test',
        '/p:LeanRun=true;SolutionFile=.\Narvalo (Miscs).sln', 
        '/verbosity:minimal', 
        '/maxcpucount', 
        '/nodeReuse:false'
}

Task MvpSolution {
    MSBuild $Project '/t:Test',
        '/p:LeanRun=true;SolutionFile=.\Narvalo (Mvp).sln', 
        '/verbosity:minimal', 
        '/maxcpucount', 
        '/nodeReuse:false'
}
