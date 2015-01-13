# We force the framework to be sure we use the build tools v12.0.
# Required by the Build-MyGet target.
Framework '4.5.1x64'

# ------------------------------------------------------------------------------
# Properties
# ------------------------------------------------------------------------------

Properties {
    Assert ($verbosity -ne $null) "`$verbosity should not be null, e.g. run with -Parameters @{ 'verbosity' = 'minimal'; }"
    Assert ($retail -ne $null) "`$retail should not be null, e.g. run with -Parameters @{ 'retail' = $true; }"

    $GitCommitHash = ''

    # Console options.
    $Opts = '/nologo', "/verbosity:$verbosity", '/maxcpucount', '/nodeReuse:false'

    $Everything = Get-LocalPath 'tools\Make.proj' -Resolve
    $Foundations = Get-LocalPath 'tools\Make.Foundations.proj' -Resolve
    
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

FormatTaskName {
    param([Parameter(Mandatory = $true)] [string] $TaskName)
    
    Write-Host "Executing Task '$taskName'." -ForegroundColor DarkCyan
}

TaskTearDown {
    if ($LastExitCode -ne 0) {
        Exit-Gracefully -ExitCode $LastExitCode 'Build failed.'
    }
}

Task Default -Depends FastBuild

# ------------------------------------------------------------------------------
# Continuous Integration and development tasks
# ------------------------------------------------------------------------------

Task FastBuild `
    -Description 'Fast build ''Foundations'' then run tests.' `
{
    MSBuild $Foundations $Opts $CIProps `
        '/t:Xunit', 
        '/p:Configuration=Debug',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true'
}

Task Build `
    -Description 'Build all projects.' `
{
    MSBuild $Everything $Opts $CIProps '/t:Build'
}

Task FullBuild `
    -Description 'Build all projects, run source analysis, verify results & run tests.' `
{
    # Perform the following operations:
    # - Run Source Analysis
    # - Build all projects
    # - Verify Portable Executable (PE) format
    # - Run Xunit tests, including white-box tests
    MSBuild $Everything $Opts $CIProps `
        '/t:Build;PEVerify;Xunit',
        '/p:SourceAnalysisEnabled=true'
}   

Task FullClean `
    -Description 'Delete work directory.' `
    -ContinueOnError `
{
    # Sometimes this task fails for some obscure reasons. Maybe the directory is locked?
    Remove-LocalItem 'work' -Recurse
}

Task CodeAnalysis `
    -Description 'Run Code Analysis (SLOW).' `
    -Alias CA `
{
    MSBuild $Everything $Opts $StaticAnalysisProps `
        '/t:Build', 
        '/p:RunCodeAnalysis=true',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true'
}

Task CodeContractsAnalysis `
    -Description 'Run Code Contracts Analysis on ''Foundations'' (EXTREMELY SLOW).' `
    -Alias CC `
{
    MSBuild $Foundations $Opts $StaticAnalysisProps `
        '/t:Build',
        '/p:Configuration=CodeContracts'
} 

Task SecurityAnalysis `
    -Description 'Run SecAnnotate on ''Foundations'' (SLOW).' `
    -Alias SA `
{
    MSBuild $Foundations $Opts $StaticAnalysisProps `
        '/t:SecAnnotate',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true'
}

# ------------------------------------------------------------------------------
# Packaging tasks
# ------------------------------------------------------------------------------

Task Package `
    -Description 'Package ''Foundations''.' `
    -Depends _Package-DependsOn `
    -Alias Pack `
{
    MSBuild $Foundations $Opts $PackagingTargets $PackagingProps `
        "/p:GitCommitHash=$GitCommitHash"
}

Task Package-Core `
    -Description 'Package core projects.' `
    -Depends _Package-DependsOn `
    -Alias PackCore `
{
    MSBuild $Foundations $Opts $PackagingTargets $PackagingProps `
        "/p:GitCommitHash=$GitCommitHash",
        '/p:Filter=_Core_' 
}

Task Package-Mvp `
    -Description 'Package MVP projects.' `
    -Depends _Package-DependsOn `
    -Alias PackMvp `
{
    MSBuild $Foundations $Opts $PackagingTargets $PackagingProps `
        "/p:GitCommitHash=$GitCommitHash",
        '/p:Filter=_Mvp_' 
}

Task Package-Build `
    -Description 'Package the Narvalo.Build project.' `
    -Depends _Package-DependsOn `
    -Alias PackBuild `
{
    MSBuild $Foundations $Opts $PackagingTargets $PackagingProps `
        "/p:GitCommitHash=$GitCommitHash",
        '/p:Filter=_Build_' 
} 

Task _Package-DependsOn `
    -Description 'Tasks on which Packaging targets depend.' `
    -Depends _Set-GitCommitHash, _Validate-PackagingForRetail `

Task _Validate-PackagingForRetail `
    -Description 'Validate retail packaging tasks.' `
    -PreCondition { $retail } `
{
    if ($GitCommitHash -eq '') {
        Exit-Gracefully -ExitCode 1 `
            'When building retail packages, the git commit hash MUST not be empty.'
    }
}

# ------------------------------------------------------------------------------
# Publish tasks
# ------------------------------------------------------------------------------

Task Publish `
    -Description 'Publish all projects.' `
    -Alias Push `
{
    Exit-Gracefully -ExitCode 1 'Not yet implemented!'

    #$packagesDir = Get-LocalPath 'work\packages'

    #$nuget = Get-NuGet -Install
    #& $nuget push "$packagesDir\*.nupkg"
}

# ------------------------------------------------------------------------------
# MyGet tasks
# ------------------------------------------------------------------------------

Task MyGet-Clean `
    -Description 'Clean MyGet server.' `
    -Depends _MyGet-Init `
{
    Remove-LocalItem -Path $script:MyGetPkg
    Remove-LocalItem -Path $script:MyGetDir -Recurse
}

Task MyGet-Build `
    -Description 'Build MyGet server.' `
    -Depends _MyGet-Restore `
{
    # Force the value of "VisualStudioVersion", otherwise MSBuild won't publish the project on build.
    # Cf. http://sedodream.com/2012/08/19/VisualStudioProjectCompatabilityAndVisualStudioVersion.aspx.
    MSBuild $Opts `
        (Get-LocalPath 'tools\MyGet\MyGet.csproj' -Resolve) `
        '/t:Clean;Build',
        '/p:Configuration=Release;PublishProfile=NarvaloOrg;DeployOnBuild=true;VisualStudioVersion=12.0'
}

Task MyGet-Zip `
    -Description 'Zip MyGet server.' `
    -Depends _MyGet-Init `
{
    if (!(Test-Path $script:MyGetDir)) {
        # We do not add a dependency on MyGet-Build so that we can run this task alone.
        # MyGet-Package provides the stronger version. 
        Exit-Gracefully -ExitCode 1 `
            'Unabled to create the Zip package: did you forgot to call the MyGet-Build task?'
    }

    . (Get-7Zip -Install) -mx9 a $script:MyGetPkg $script:MyGetDir | Out-Null

    if (!$?) {
        Exit-Gracefully -ExitCode 1 'Unabled to create the Zip package.'
    }

    Write-Host "A ready to publish zip file for MyGet may be found here: '$script:MyGetPkg'." -ForegroundColor Green
}

Task MyGet-Package `
    -Description 'Package MyGet server.' `
    -Depends MyGet-Build, MyGet-Zip

Task MyGet `
    -Description 'Clean then package the MyGet server.' `
    -Depends MyGet-Clean, MyGet-Package
    
Task _MyGet-Init `
    -Description 'Initialize the variables only used by the MyGet tasks.' `
{
    $script:MyGetDir = Get-LocalPath 'work\myget'
    $script:MyGetPkg = Get-LocalPath 'work\myget.7z'
}

Task _MyGet-Restore `
    -Description 'Restore NuGet packages for the MyGet project.' `
{
    $nuget = Get-NuGet -Install
    
    try {
        . $nuget restore (Get-LocalPath 'tools\MyGet\packages.config') `
            -PackagesDirectory (Get-LocalPath 'tools\packages') `
            -ConfigFile (Get-LocalPath 'tools\.nuget\NuGet.Config') `
            -Verbosity quiet 2>&1
    } catch {
        throw "'nuget.exe restore' failed: $_"
    }
}

# ------------------------------------------------------------------------------
# Miscs
# ------------------------------------------------------------------------------

Task Environment `
    -Description 'Display the build environment.' `
    -Alias Env `
{
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

Task _Documentation `
    -Description 'Display a description of the public tasks.' `
{
    # PSake allows to display a description of the tasks by using:
    # > Invoke-PSake $buildFile -NoLogo -Docs
    # but I find the result more geared towards developers. 
    # Here is my own version of the underlying WriteDocumentation function.
    $currentContext = $psake.context.Peek()

    if ($currentContext.tasks.default) {
        $defaultTaskDependencies = $currentContext.tasks.default.DependsOn
    } else {
        $defaultTaskDependencies = @()
    }

    $currentContext.tasks.Keys | %{
        # Ignore default and private tasks.
        if ($_ -eq 'default' -or $_.StartsWith('_')) {
            return
        }

        $task = $currentContext.tasks.$_

        if ($defaultTaskDependencies -Contains $task.Name) { 
            $name = "$($task.Name) (DEFAULT)"
        } else {
            $name = $task.Name
        }

        New-Object PSObject -Property @{
            Name = $name;
            Alias = $task.Alias;
            Description = $task.Description;
        }
    } | 
        sort 'Name' | 
        Format-Table -AutoSize -Wrap -Property Name, Alias, Description
}

Task _Set-GitCommitHash `
    -Description 'Initialize GitCommitHash.' `
{
    $GitCommitHash = Get-Git | Get-GitCommitHash -NoWarn
}

# ------------------------------------------------------------------------------
