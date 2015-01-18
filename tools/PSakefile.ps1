# PSakefile script.
# Requires the module 'Narvalo.ProjectAutomation'.

# We force the framework to be sure we use the v12.0 of the build tools.
# For instance, this is a requirement for the _MyGet-Publish target where
# the DeployOnBuild instruction is not understood by previsou versions of MSBuild.
Framework '4.5.1x64'

# ------------------------------------------------------------------------------
# Properties
# ------------------------------------------------------------------------------

Properties {
    # Process mandatory parameters.
    Assert($Retail -ne $null) "`$Retail must not be null, e.g. run with -Parameters @{ 'retail' = `$true; }"

    # Process optional parameters.
    if ($Verbosity -eq $null) { $Verbosity = 'minimal' }
    if ($Developer -eq $null) { $Developer = $false }

    # Define the NuGet verbosity level.
    $NuGetVerbosity = ConvertTo-NuGetVerbosity $Verbosity

    # MSBuild options.
    $Opts = '/nologo', "/verbosity:$Verbosity", '/maxcpucount', '/nodeReuse:false'

    # Main MSBuild projects.
    $Everything  = Get-LocalPath 'tools\Make.proj'
    $Foundations = Get-LocalPath 'tools\Make.Foundations.proj'
}

FormatTaskName {
    param([Parameter(Mandatory = $true)] [string] $TaskName)
    
    Write-Host "Executing Task '$taskName'." -ForegroundColor DarkCyan
}

TaskTearDown {
    # Catch errors from both PowerShell and Win32 exe.
    if (!$?) {
        Exit-Gracefully -ExitCode 1 'Build failed.'
    }

    #if ($LastExitCode -ne 0) {
    #    Exit-Gracefully -ExitCode $LastExitCode 'Build failed.'
    #}
}

Task Default -Depends FastBuild

# ------------------------------------------------------------------------------
# Continuous Integration and development tasks
# ------------------------------------------------------------------------------

# NB: No need to restore packages before building the projects $Everything
# or $Foundations; this will be done in MSBuild.

Task FastBuild `
    -Description 'Fast build ''Foundations'' then run tests.' `
    -Depends _CI-InitializeVariables `
{
    MSBuild $Foundations $Opts $script:CIProps `
        '/t:Xunit', 
        '/p:Configuration=Debug',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true'
}

Task Build `
    -Description 'Build all projects.' `
    -Depends _CI-InitializeVariables `
{
    MSBuild $Everything $Opts $script:CIProps '/t:Build'
}

Task FullBuild `
    -Description 'Build all projects, run source analysis, verify results & run tests.' `
    -Depends _CI-InitializeVariables `
    -Alias CI `
{
    # Perform the following operations:
    # - Run Source Analysis
    # - Build all projects
    # - Verify Portable Executable (PE) format
    # - Run Xunit tests, including white-box tests
    MSBuild $Everything $Opts $script:CIProps `
        '/t:Build;PEVerify;Xunit',
        '/p:SourceAnalysisEnabled=true'
}   

Task FullClean `
    -Description 'Delete the entire build directory.' `
    -Alias Clean `
    -ContinueOnError `
{
    # Sometimes this task fails for some obscure reasons. Maybe the directory is locked?
    Remove-LocalItem 'work' -Recurse
}

Task CodeAnalysis `
    -Description 'Run Code Analysis (SLOW).' `
    -Depends _CI-InitializeVariables `
    -Alias CA `
{
    MSBuild $Everything $Opts $script:CIAnalysisProps `
        '/t:Build', 
        '/p:RunCodeAnalysis=true',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true'
}

Task CodeContractsAnalysis `
    -Description 'Run Code Contracts Analysis on ''Foundations'' (EXTREMELY SLOW).' `
    -Depends _CI-InitializeVariables `
    -Alias CC `
{
    MSBuild $Foundations $Opts $script:CIAnalysisProps `
        '/t:Build',
        '/p:Configuration=CodeContracts'
} 

Task SecurityAnalysis `
    -Description 'Run SecAnnotate on ''Foundations'' (SLOW).' `
    -Depends _CI-InitializeVariables `
    -Alias SA `
{
    MSBuild $Foundations $Opts $script:CIAnalysisProps `
        '/t:SecAnnotate',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true'
}

Task _CI-InitializeVariables `
    -Description 'Initialize variables only used by the CI tasks.' `
    -RequiredVariables Retail `
{
    # Default CI properties:
    # - Release configuration
    # - Do not generate assembly versions
    # - Do not sign assemblies
    # - Do not skip the generation of the Code Contracts reference assembly
    # - Leak internals to enable all white-box tests.
    $script:CIProps = `
        '/p:Configuration=Release',
        '/p:BuildGeneratedVersion=false',
        "/p:Retail=$Retail",
        '/p:SignAssembly=false',
        '/p:SkipCodeContractsReferenceAssembly=false',
        '/p:VisibleInternals=true'

    # For static analysis, we hide internals, otherwise we might not truly 
    # analyze the public API.
    $script:CIAnalysisProps = $script:CIProps, '/p:VisibleInternals=false'
}

# ------------------------------------------------------------------------------
# Packaging tasks
# ------------------------------------------------------------------------------

Task Package-All `
    -Description 'Package ''Foundations''.' `
    -Depends Package-Core, Package-Mvp, Package-Miscs `
    -Alias Pack

Task Package-Core `
    -Description 'Package core projects.' `
    -Depends _Package-InitializeVariables, _Package-ValidateRetail `
    -Alias PackCore `
{
    MSBuild $Foundations $Opts $script:PackageTargets $script:PackageProps `
        '/p:Filter=_Core_' 
}

Task Package-Mvp `
    -Description 'Package MVP projects.' `
    -Depends _Package-InitializeVariables, _Package-ValidateRetail `
    -Alias PackMvp `
{
    MSBuild $Foundations $Opts $script:PackageTargets $script:PackageProps `
        '/p:Filter=_Mvp_' 
}

Task Package-Build `
    -Description 'Package the project Narvalo.Build.' `
    -Depends _Package-InitializeVariables, _Package-ValidateRetail `
    -Alias PackBuild `
{
    MSBuild $Foundations $Opts $script:PackageTargets $script:PackageProps `
        '/p:Filter=_Build_'
}

Task _Package-InitializeVariables `
    -Description 'Initialize variables only used by the Package-* tasks.' `
    -Depends _Initialize-GitCommitHash `
    -RequiredVariables Retail `
{
    # Packaging properties:
    # - Release configuration
    # - Generate assembly versions (necessary for NuGet packaging)
    # - Sign assemblies
    # - Do not skip the generation of the Code Contracts reference assembly
    # - Unconditionally hide internals (implies no white-box testing)
    $script:PackageProps = `
        '/p:Configuration=Release',
        '/p:BuildGeneratedVersion=true',
        "/p:GitCommitHash=$script:GitCommitHash",
        "/p:Retail=$Retail",
        '/p:SignAssembly=true',
        '/p:SkipCodeContractsReferenceAssembly=false',
        '/p:VisibleInternals=false'
        
    # Packaging targets:
    # - Rebuild all
    # - Verify Portable Executable (PE) format
    # - Run Xunit tests
    # - Package
    $script:PackageTargets = '/t:Rebuild;PEVerify;Xunit;Package'
}

Task _Package-ValidateRetail `
    -Description 'Validate retail packaging tasks.' `
    -PreCondition { $Retail } `
{
    if ($script:GitCommitHash -eq '') {
        Exit-Gracefully -ExitCode 1 `
            'When building retail packages, the git commit hash MUST not be empty.'
    }
}

# ------------------------------------------------------------------------------
# Publication tasks
# ------------------------------------------------------------------------------

Task Publish-All `
    -Description 'Publish ''Foundations''.' `
    -Depends Publish-Core, Publish-Mvp, Publish-Build `
    -Alias Push

Task Publish-Core `
    -Description 'Publish core projects.' `
    -Depends _Publish-InitializeVariables, _Publish-DependsOn, Package-Core `
    -RequiredVariables Retail `
    -Alias PushCore `
{
    Publish-Packages $script:PublishPackagesDirectory -Retail:$Retail
}

Task Publish-Mvp `
    -Description 'Publish MVP projects.' `
    -Depends _Publish-InitializeVariables, _Publish-DependsOn, Package-Mvp `
    -RequiredVariables Retail `
    -Alias PushMvp `
{
    Publish-Packages $script:PublishPackagesDirectory -Retail:$Retail
}

Task Publish-Build `
    -Description 'Publish the project Narvalo.Build.' `
    -Depends _Publish-InitializeVariables, _Publish-DependsOn, Package-Build `
    -RequiredVariables Retail `
    -Alias PushBuild `
{
    Publish-Packages $script:PublishPackagesDirectory -Retail:$Retail
}

Task _Publish-DependsOn `
    -Description 'Dependencies shared among the Publish-* tasks.' `
    -Depends _Publish-Clean, NuGetHelper-Build

Task _Publish-Clean `
    -Description 'Remove the staging directory for packages.' `
    -Depends _Publish-InitializeVariables `
{
    Remove-LocalItem -Path $script:PublishPackagesDirectory -Recurse
}

Task _Publish-InitializeVariables `
    -Description 'Initialize variables only used by the Publish-* tasks.' `
{
    $script:PublishPackagesDirectory = Get-LocalPath 'work\packages'
}

# ------------------------------------------------------------------------------
# NuGetHelper project
# ------------------------------------------------------------------------------

Task NuGetHelper-Build `
    -Description 'Build the project NuGetHelper.' `
    -Depends _NuGetHelper-InitializeVariables, _NuGetHelper-RestorePackages `
    -Alias NuGetHelper `
{
    MSBuild $script:NuGetHelperProject  $Opts '/p:Configuration=Release', '/t:Build'
}

Task _NuGetHelper-RestorePackages `
    -Description 'Restore packages for the project NuGetHelper.' `
    -Depends _NuGetHelper-InitializeVariables, _Tools-InitializeVariables `
{
    Restore-Packages -Source $script:NuGetHelperPackagesConfig `
        -PackagesDirectory $script:ToolsPackagesDirectory `
        -ConfigFile $script:ToolsNuGetConfig `
        -Verbosity $NuGetVerbosity
}

Task _NuGetHelper-InitializeVariables `
    -Description 'Initialize variables only used by the NuGetHelper-* tasks.' `
{
    $script:NuGetHelperProject        = Get-LocalPath 'tools\NuGetHelper\NuGetHelper.fsproj'
    $script:NuGetHelperPackagesConfig = Get-LocalPath 'tools\NuGetHelper\packages.config'
}

# ------------------------------------------------------------------------------
# Edge project
# ------------------------------------------------------------------------------

Task Edge-FullBuild `
    -Description 'Update then re-build the project Edge.' `
    -Depends _Edge-Update, _Edge-Rebuild `
    -Alias Edge

Task _Edge-Rebuild `
    -Description 'Re-build the project Edge.' `
    -Depends _Edge-InitializeVariables, _Edge-RestorePackages `
{
    MSBuild $script:EdgeProject $Opts '/p:Configuration=Release', '/t:Rebuild'
}

Task _Edge-RestorePackages `
    -Description 'Restore packages for the project Edge.' `
    -Depends _Edge-InitializeVariables, _Tools-InitializeVariables `
{
    Restore-Packages -Source $script:EdgePackagesConfig `
        -PackagesDirectory $script:ToolsPackagesDirectory `
        -ConfigFile $script:ToolsNuGetConfig `
        -Verbosity $NuGetVerbosity
}

Task _Edge-Update `
    -Description 'Update NuGet packages for the project Edge.' `
    -Depends _Edge-InitializeVariables, _Tools-InitializeVariables `
{
    # This function also updates the project to the last versions of the packages 
    # from the official NuGet repository, which might not be what we do really want.
    # The problem is that we want to update the Narvalo packages to their latest
    # pre-release versions (only available on our own NuGet server) but they might
    # include a new or updated dependency which is not available on our NuGet server.
    # The current workaround is to update first from the official NuGet source.
    # Unfortunately it won't help if there is a newly created dependency or if we 
    # update a dependency to a new untested version.

    $nuget = Get-NuGet -Install

    try {
        . $nuget update $script:EdgePackagesConfig `
            -Source "https://www.nuget.org/api/v2/" `
            -RepositoryPath $script:ToolsPackagesDirectory `
            -Verbosity $NuGetVerbosity

        . $nuget update $script:EdgePackagesConfig `
            -Source "http://narvalo.org/myget/nuget/" `
            -Prerelease `
            -RepositoryPath $script:ToolsPackagesDirectory `
            -Verbosity $NuGetVerbosity
    } catch {
        Exit-Gracefully -ExitCode 1 "'nuget.exe update' failed: $_"
    }
}

Task _Edge-InitializeVariables `
    -Description 'Initialize variables only used by the Edge-* tasks.' `
{
    $script:EdgeProject        = Get-LocalPath 'tools\Edge\Edge.csproj'
    $script:EdgePackagesConfig = Get-LocalPath 'tools\Edge\packages.config'
}

# ------------------------------------------------------------------------------
# MyGet project
# ------------------------------------------------------------------------------

Task MyGet-Package `
    -Description 'Package the project MyGet.' `
    -Depends _MyGet-InitializeVariables, _MyGet-DeleteStagingDirectory, _MyGet-Publish, _MyGet-Zip `
    -Alias MyGet `
{
    Write-Host "A ready to publish zip file for MyGet may be found here: '$script:MyGetZipFile'." -ForegroundColor Green
}
       
Task _MyGet-Publish `
    -Description 'Clean up, build then publish the project MyGet.' `
    -Depends _MyGet-InitializeVariables, _MyGet-RestorePackages `
{
    # Force the value of "VisualStudioVersion", otherwise MSBuild won't publish the project on build.
    # Cf. http://sedodream.com/2012/08/19/VisualStudioProjectCompatabilityAndVisualStudioVersion.aspx.
    MSBuild $script:MyGetProject $Opts `
        '/t:Rebuild',
        '/p:Configuration=Release;PublishProfile=NarvaloOrg;DeployOnBuild=true;VisualStudioVersion=12.0'
}
    
Task _MyGet-RestorePackages `
    -Description 'Restore packages for the project MyGet.' `
    -Depends _MyGet-InitializeVariables, _Tools-InitializeVariables `
{
    Restore-Packages -Source $script:MyGetPackagesConfig `
        -PackagesDirectory $script:ToolsPackagesDirectory `
        -ConfigFile $script:ToolsNuGetConfig `
        -Verbosity $NuGetVerbosity
}

Task _MyGet-DeleteStagingDirectory `
    -Description 'Remove the directory ''work\myget''.' `
    -Depends _MyGet-InitializeVariables `
{
    Remove-LocalItem -Path $script:MyGetStagingDirectory -Recurse
}

Task _MyGet-DeleteZipFile `
    -Description 'Delete the package for MyGet.' `
    -Depends _MyGet-InitializeVariables `
{
    Remove-LocalItem -Path $script:MyGetZipFile
}

Task _MyGet-Zip `
    -Description 'Zip the publication artefacts for the project MyGet.' `
    -Depends _MyGet-InitializeVariables, _MyGet-DeleteZipFile `
    -PreAction {
        if (!(Test-Path $script:MyGetStagingDirectory)) {
            # We do not add a dependency on _MyGet-Publish so that we can run this task alone.
            # MyGet-Package provides the stronger version. 
            Exit-Gracefully -ExitCode 1 `
                'Unabled to create the Zip package: did you forgot to call the _MyGet-Publish task?'
        }
    } -Action {
        . (Get-7Zip -Install) -mx9 a $script:MyGetZipFile $script:MyGetStagingDirectory | Out-Null

        if (!$?) {
            Exit-Gracefully -ExitCode 1 'Unabled to create the Zip package.'
        }
    }

Task _MyGet-InitializeVariables `
    -Description 'Initialize variables only used by the MyGet-* tasks.' `
{
    $script:MyGetProject          = Get-LocalPath 'tools\MyGet\MyGet.csproj'
    $script:MyGetPackagesConfig   = Get-LocalPath 'tools\MyGet\packages.config'
    $script:MyGetStagingDirectory = Get-LocalPath 'work\myget'
    $script:MyGetZipFile          = Get-LocalPath 'work\myget.7z'
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

Task _Initialize-GitCommitHash `
    -Description 'Initialize GitCommitHash.' `
{
    $script:GitCommitHash = Get-Git | Get-GitCommitHash -NoWarn
}

Task _Tools-InitializeVariables `
    -Description 'Initialize variables for the tooling projects.' `
{
    $script:ToolsPackagesDirectory = Get-LocalPath 'tools\packages'
    $script:ToolsNuGetConfig       = Get-LocalPath 'tools\.nuget\NuGet.Config'
}

# ------------------------------------------------------------------------------
# Functions
# ------------------------------------------------------------------------------

<#
.SYNOPSIS
    Convert a MSBuild verbosity level to a NuGet verbosity level.
.PARAMETER Verbosity
    Specifies the MSBuild verbosity.
.INPUTS
    None.
.OUTPUTS
    None.
#>
function ConvertTo-NuGetVerbosity {
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

function Publish-Packages {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)] [string] $StagingDirectory,

        [switch] $Retail
    ) 

    $cmd = Get-LocalPath 'tools\NuGetHelper\bin\Release\NuGetHelper.exe'

    if (!(Test-Path $cmd)) {
        Exit-Gracefully -ExitCode 1 `
            'Before calling this function, make sure to build the project NuGetHelper in Release configuration.'
    }

    try {
        if ($Retail) {
            . $cmd --directory $stagingDirectory --retail 2>&1
        } else {
            . $cmd --directory $stagingDirectory 2>&1
        }
    } catch {
        Exit-Gracefully -ExitCode 1 "'NuGetHelper.exe' failed: $_"
    }
}

# ------------------------------------------------------------------------------
