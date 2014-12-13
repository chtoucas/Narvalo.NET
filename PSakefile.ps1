#
# WARNING: This file must stay at the root of the repository.
#

Properties {
    $Project = "$PSScriptRoot\Make.proj"
}

Task default -depends Minimal

# Delete work directory.
Task Clean {
    $workDir = "$PSScriptRoot\work\"

    if (Test-Path $workDir) {
        Remove-Item $workDir -Force -Recurse
    }
}

# Run tests for public projects in Debug configuration.
Task Minimal {
    MSBuild $Project '/t:RunTests',
        '/p:BuildGeneratedVersion=false;SkipPrivateProjects=true',
        '/verbosity:minimal', 
        '/maxcpucount', 
        '/nodeReuse:false'
}

# Continuous integration build: sign assemblies, use Release configuration, detailed log.
Task CI {
    MSBuild $Project '/t:Build;VerifyBuild;RunTests;Package', 
        '/p:Configuration=Release;SignAssembly=true', 
        '/verbosity:detailed',
        '/maxcpucount', 
        '/nodeReuse:false'
}

# Create packages for publication: sign assemblies, skip private projects, use Release configuration.
Task Package -depends Clean {
    MSBuild $Project '/t:Clean;Build;VerifyBuild;RunTests;Package' 
        '/p:Configuration=Release;SignAssembly=true;SkipPrivateProjects=true', 
        '/verbosity:minimal', 
        '/maxcpucount', 
        '/nodeReuse:false'
}

# Lean build then run tests for Narvalo (Core).sln in Release configuration.
Task BuildThenTestCoreSolution {
    MSBuild $Project '/t:RunTests',
        '/p:Configuration=Release;LeanRun=true;SolutionFile=.\Narvalo (Core).sln', 
        '/verbosity:minimal', 
        '/maxcpucount', 
        '/nodeReuse:false'
}

# Lean build then run tests for Narvalo.sln in Release configuration.
Task BuildThenTestMainSolution {
    MSBuild $Project '/t:RunTests',
        '/p:Configuration=Release;LeanRun=true;SolutionFile=.\Narvalo.sln', 
        '/verbosity:minimal', 
        '/maxcpucount', 
        '/nodeReuse:false'
}

# Lean build then run tests for Narvalo (Miscs).sln in Release configuration.
Task BuildThenTestMiscsSolution {
    MSBuild $Project '/t:RunTests',
        '/p:Configuration=Release;LeanRun=true;SolutionFile=.\Narvalo (Miscs).sln', 
        '/verbosity:minimal', 
        '/maxcpucount', 
        '/nodeReuse:false'
}

# Lean build then run tests for Narvalo (Mvp).sln in Release configuration.
Task BuildThenTestMvpSolution {
    MSBuild $Project '/t:RunTests',
        '/p:Configuration=Release;LeanRun=true;SolutionFile=.\Narvalo (Mvp).sln', 
        '/verbosity:minimal', 
        '/maxcpucount', 
        '/nodeReuse:false'
}

TaskTearDown {
    if ($LastExitCode -ne 0) {
        Write-Host ''
        Write-Host 'Build failed.' -BackgroundColor Red -ForegroundColor Yellow
        Write-Host ''
        Exit 1
    }
}