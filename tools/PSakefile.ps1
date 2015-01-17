# PSakefile script.

# We force the framework to be sure we use the v12.0 of the build tools.
# For instance, this is a requirement for the _MyGet-Build target where
# the DeployOnBuild instruction is not understood by previsou versions of MSBuild.
Framework '4.5.1x64'

# ------------------------------------------------------------------------------
# Properties
# ------------------------------------------------------------------------------

Properties {
    # Process mandatory parameters.
    Assert ($Retail -ne $null) "`$Retail must not be null, e.g. run with -Parameters @{ 'retail' = $true; }"

    # Process optional parameters.
    if ($Verbosity -eq $null) { $Verbosity = 'minimal' }
    if ($Developer -eq $null) { $Developer = $false }

    # Define the NuGet verbosity.
    $NuGetVerbosity = Get-NuGetVerbosity $Verbosity

    # NuGet servers.
    $NuGetSource    = "https://www.nuget.org/api/v2/"
    $MyGetSource    = "http://narvalo.org/myget/nuget/"
    $MyGetApiSource = "http://narvalo.org/myget/"
    
    # Work directories.
    $MyGetDirectory    = Get-LocalPath 'work\myget'
    $MyGetZipFile      = Get-LocalPath 'work\myget.7z'
    $PackagesDirectory = Get-LocalPath 'work\packages'

    # MSBuild options.
    $Opts = '/nologo', "/verbosity:$Verbosity", '/maxcpucount', '/nodeReuse:false'

    # MSBuild projects.
    $Everything  = Get-LocalPath 'tools\Make.proj' -Resolve
    $Foundations = Get-LocalPath 'tools\Make.Foundations.proj' -Resolve
    $Edge        = Get-LocalPath 'tools\Edge\Edge.csproj' -Resolve
    $NuGetHelper = Get-LocalPath 'tools\NuGetHelper\NuGetHelper.fsproj' -Resolve
    
    # Packaging properties:
    # - Release configuration
    # - Generate assembly versions (necessary for NuGet packaging)
    # - Sign assemblies
    # - Do not skip the generation of the Code Contracts reference assembly
    # - Unconditionally hide internals (implies no white-box testing)
    $PackagingProps = `
        '/p:Configuration=Release',
        '/p:BuildGeneratedVersion=true',
        "/p:Retail=$Retail",
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
        "/p:Retail=$Retail",
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
    
    # Properties for the project Edge.
    $EdgeProps = '/p:Configuration=Release'

    # Properties for the project NuGetHelper.
    $NuGetHelperProps = '/p:Configuration=Release'
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

# NB: No need to restore packages before building the projects $Everything
# or $Foundations, this will be done in MSBuild.

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
    -Alias CI `
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
    -Alias Clean `
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
        "/p:GitCommitHash=$script:GitCommitHash"
}

Task Package-Core `
    -Description 'Package core projects.' `
    -Depends _Package-DependsOn `
    -Alias PackCore `
{
    MSBuild $Foundations $Opts $PackagingTargets $PackagingProps `
        "/p:GitCommitHash=$script:GitCommitHash",
        '/p:Filter=_Core_' 
}

Task Package-Mvp `
    -Description 'Package MVP projects.' `
    -Depends _Package-DependsOn `
    -Alias PackMvp `
{
    MSBuild $Foundations $Opts $PackagingTargets $PackagingProps `
        "/p:GitCommitHash=$script:GitCommitHash",
        '/p:Filter=_Mvp_' 
}

Task Package-Build `
    -Description 'Package the project Narvalo.Build.' `
    -Depends _Package-DependsOn `
    -Alias PackBuild `
{
    MSBuild $Foundations $Opts $PackagingTargets $PackagingProps `
        "/p:GitCommitHash=$script:GitCommitHash",
        '/p:Filter=_Build_' 
} 

Task _Package-DependsOn `
    -Description 'Task on which all Package-* targets depend.' `
    -Depends _Set-GitCommitHash, _Validate-PackagingForRetail

Task _Validate-PackagingForRetail `
    -Description 'Validate retail packaging tasks.' `
    -PreCondition { $Retail } `
{
    if ($script:GitCommitHash -eq '') {
        Exit-Gracefully -ExitCode 1 `
            'When building retail packages, the git commit hash MUST not be empty.'
    }
}

# ------------------------------------------------------------------------------
# Publish tasks
# ------------------------------------------------------------------------------

Task _Fake-Package {
    Write-Warning 'Fake Package task. To be replaced by the real one.'
}

Task Publish `
    -Description 'Publish ''Foundations''.' `
    -Depends _Publish-Clean, _Fake-Package, _Publish-Packages `
    -Alias Push

Task Publish-Core `
    -Description 'Publish core projects.' `
    -Depends _Publish-Clean, Package-Core, _Publish-Packages `
    -Alias PushCore

Task Publish-Mvp `
    -Description 'Publish MVP projects.' `
    -Depends _Publish-Clean, Package-Mvp, _Publish-Packages `
    -Alias PushMvp

Task Publish-Build `
    -Description 'Publish the project Narvalo.Build.' `
    -Depends _Publish-Clean, Package-Build, _Publish-Packages `
    -Alias PushBuild

Task _Publish-Clean `
    -Description 'Remove the directory ''work\packages''.' `
{
    Remove-LocalItem -Path $PackagesDirectory -Recurse
}

Task _Publish-Core `
    -Description 'Core publish task.' `
    -Depends NuGetHelper-Build `
{
    Publish-Packages -Directory $PackagesDirectory -Retail:$Retail
}

Task _Publish-Packages `
    -Description 'Proxy to the real publication task.' `
{
    if ($Retail) {
        Invoke-Task _Publish-PackagesForRetail
    } else {
        Invoke-Task _Publish-PackagesNotForDistribution
    }
}

Task _Publish-PackagesForRetail `
    -Description 'Publish retail packages.' `
    -Depends _Publish-Core `
    -PreCondition { $Retail }
    
Task _Publish-PackagesNotForDistribution `
    -Description 'Publish packages not for distribution.' `
    -Depends _Publish-Core, _Edge-Update, Edge-Build `
    -PreCondition { -not $Retail }

# ------------------------------------------------------------------------------
# NuGetHelper project
# ------------------------------------------------------------------------------

Task NuGetHelper-Build `
    -Description 'Build the project NuGetHelper.' `
    -Depends _Restore-MaintenancePackages `
    -Alias NuGetHelper `
{
    MSBuild $NuGetHelper $Opts $NuGetHelperProps '/t:Build'
}

# ------------------------------------------------------------------------------
# Edge project
# ------------------------------------------------------------------------------

Task Edge-Build `
    -Description 'Update NuGet dependencies, clean then build the project Edge.' `
    -Depends _Restore-MaintenancePackages `
    -Alias Edge `
{
    MSBuild $Edge $Opts $EdgeProps '/t:Clean;Build'
}

Task _Edge-Update `
    -Description 'Update NuGet packages for the project Edge.' `
{
    Update-PackagesForEdge $MyGetSource $NuGetVerbosity
}

# ------------------------------------------------------------------------------
# MyGet project
# ------------------------------------------------------------------------------

Task MyGet-Repackage `
    -Description 'Clean up then package the project MyGet.' `
    -Depends _MyGet-Clean, _MyGet-Package `
    -Alias MyGet
    
Task _MyGet-Build `
    -Description 'Build the project MyGet.' `
    -Depends _Restore-MaintenancePackages `
{
    # Force the value of "VisualStudioVersion", otherwise MSBuild won't publish the project on build.
    # Cf. http://sedodream.com/2012/08/19/VisualStudioProjectCompatabilityAndVisualStudioVersion.aspx.
    MSBuild $Opts `
        (Get-LocalPath 'tools\MyGet\MyGet.csproj' -Resolve) `
        '/t:Clean;Build',
        '/p:Configuration=Release;PublishProfile=NarvaloOrg;DeployOnBuild=true;VisualStudioVersion=12.0'
}

Task _MyGet-Rebuild `
    -Description 'Clean then build the project MyGet.' `
    -Depends _MyGet-Clean, _MyGet-Build

Task _MyGet-Package `
    -Description 'Package the project MyGet.' `
    -Depends _MyGet-Build, _MyGet-Zip
    
Task _MyGet-Clean `
    -Description 'Remove the directory ''work\myget''.' `
{
    Remove-LocalItem -Path $MyGetDirectory -Recurse
}

Task _MyGet-DeleteZip `
    -Description 'Delete the package for MyGet.' `
{
    Remove-LocalItem -Path $MyGetZipFile
}

Task _MyGet-Zip `
    -Description 'Zip build outputs from the project MyGet.' `
    -Depends MyGet-DeleteZip `
    -PreAction {
        if (!(Test-Path $MyGetDirectory)) {
            # We do not add a dependency on _MyGet-Build so that we can run this task alone.
            # _MyGet-Package provides the stronger version. 
            Exit-Gracefully -ExitCode 1 `
                'Unabled to create the Zip package: did you forgot to call the _MyGet-Build task?'
        }
    } -Action {
        . (Get-7Zip -Install) -mx9 a $MyGetZipFile $MyGetDirectory | Out-Null

        if (!$?) {
            Exit-Gracefully -ExitCode 1 'Unabled to create the Zip package.'
        }
    } -PostAction {
        Write-Host "A ready to publish zip file for MyGet may be found here: '$MyGetZipFile'." -ForegroundColor Green
    }

# ------------------------------------------------------------------------------
# Miscs
# ------------------------------------------------------------------------------

Task Environment `
    -Description 'Display the build environment.' `
    -Alias Env `
{
    # The output of running "MSBuild /version" looks like: 
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
    # > Invoke-PSake $buildFile -Docs
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
        if ($_ -eq 'default') {
            return
        }

        if (!$Developer -and $_.StartsWith('_')) {
            return
        }

        $task = $currentContext.tasks.$_

        if ($defaultTaskDependencies -Contains $task.Name) { 
            $name = "$($task.Name) (DEFAULT)"
        } else {
            $name = $task.Name
        }

        New-Object PSObject -Property @{
            Task = $name;
            Alias = $task.Alias;
            Synopsis = $task.Description;
        }
    } | 
        sort 'Task' | 
        Format-Table -AutoSize -Wrap -Property Task, Alias, Synopsis
}

Task _Restore-MaintenancePackages `
    -Description 'Restore NuGet packages fors the "maintenance" solution.' `
{
    Restore-MaintenancePackages $NuGetVerbosity
}

Task _Set-GitCommitHash `
    -Description 'Initialize GitCommitHash.' `
{
    $script:GitCommitHash = Get-Git | Get-GitCommitHash -NoWarn
}

# ------------------------------------------------------------------------------
# Functions
# ------------------------------------------------------------------------------

<#
.SYNOPSIS
    Get the module (or NuGet) verbosity from the MSBuild verbosity.
#>
function Get-NuGetVerbosity {
    param([Parameter(Mandatory = $true, Position = 0)] [string] $Verbosity)

    switch ($verbosity) {
        'q'          { return 'quiet' }
        'quiet'      { return 'quiet' }
        'm'          { return 'normal' }
        'minimal'    { return 'normal' }
        'n'          { return 'normal' }
        'normal'     { return 'normal' }
        'd'          { return 'detailed' }
        'detailed'   { return 'detailed' }
        'diag'       { return 'detailed' }
        'diagnostic' { return 'detailed' }

        default      { return 'normal' }
    }
}

# ------------------------------------------------------------------------------
