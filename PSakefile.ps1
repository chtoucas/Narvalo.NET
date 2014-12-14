#
# WARNING: This file must stay at the root of the repository.
#

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
    MSBuild $Project '/t:RunTests',
        '/p:BuildGeneratedVersion=false;SkipPrivateProjects=true',
        '/verbosity:minimal', 
        '/maxcpucount', 
        '/nodeReuse:false'
}

# Run tests for public projects in Release configuration without visible internals.
Task Default_ReleaseNoInternals {
    MSBuild $Project '/t:RunTests',
        '/p:Configuration=Release;BuildGeneratedVersion=false;NoVisibleInternals=true;SkipPrivateProjects=true',
        '/verbosity:minimal', 
        '/maxcpucount', 
        '/nodeReuse:false'
}

# Create packages for publication: skip private projects, assemblies are 
# signed, use Release configuration and unconditionally hide internals.
Task Package -depends Clean {
    MSBuild $Project '/t:Clean;Build;VerifyBuild;RunTests;Package' 
        '/p:Configuration=Release;NoVisibleInternals=true;SignAssembly=true;SkipPrivateProjects=true', 
        '/verbosity:minimal', 
        '/maxcpucount', 
        '/nodeReuse:false'
}

#
# Continuous Integration Targets.
#
# REVIEW: To speed up things a bit, we do not call the Clean target?

# Same as Package.
Task CI {
    MSBuild $Project '/t:Build;VerifyBuild;RunTests;Package', 
        '/p:Configuration=Release;ContinuousBuild=true;NoVisibleInternals=true;SignAssembly=true;SkipPrivateProjects=true', 
        '/verbosity:detailed',
        '/maxcpucount', 
        '/nodeReuse:false'
}

Task CI_Release {
    MSBuild $Project '/t:Build;VerifyBuild;RunTests', 
        '/p:Configuration=Release;BuildGeneratedVersion=false;ContinuousBuild=true', 
        '/verbosity:minimal',
        '/maxcpucount', 
        '/nodeReuse:false'
}

Task CI_Debug {
    MSBuild $Project '/t:Build;VerifyBuild;RunTests', 
        '/p:BuildGeneratedVersion=false;ContinuousBuild=true', 
        '/verbosity:minimal',
        '/maxcpucount', 
        '/nodeReuse:false'
}

Task CI_CodeContracts {
    MSBuild $Project '/t:Build;VerifyBuild;RunTests', 
        '/p:Configuration=CodeContracts;BuildGeneratedVersion=false;ContinuousBuild=true', 
        '/verbosity:minimal',
        '/maxcpucount', 
        '/nodeReuse:false'
}

#
# Solution Targets.
#
# Lean build then run tests for solutions in Debug configuration.

Task CoreSolution {
    MSBuild $Project '/t:RunTests',
        '/p:LeanRun=true;SolutionFile=.\Narvalo (Core).sln', 
        '/verbosity:minimal', 
        '/maxcpucount', 
        '/nodeReuse:false'
}

Task MainSolution {
    MSBuild $Project '/t:RunTests',
        '/p:LeanRun=true;SolutionFile=.\Narvalo.sln', 
        '/verbosity:minimal', 
        '/maxcpucount', 
        '/nodeReuse:false'
}

Task MiscsSolution {
    MSBuild $Project '/t:RunTests',
        '/p:LeanRun=true;SolutionFile=.\Narvalo (Miscs).sln', 
        '/verbosity:minimal', 
        '/maxcpucount', 
        '/nodeReuse:false'
}

Task MvpSolution {
    MSBuild $Project '/t:RunTests',
        '/p:LeanRun=true;SolutionFile=.\Narvalo (Mvp).sln', 
        '/verbosity:minimal', 
        '/maxcpucount', 
        '/nodeReuse:false'
}
