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
    MSBuild $Foundations $Opts $CI_Props `
        '/t:Xunit',
        '/p:Configuration=Debug',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true'
}

Task Build `
    -Description 'Build all projects.' `
    -Depends _CI-InitializeVariables `
{
    MSBuild $Everything $Opts $CI_Props '/t:Build'
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
    MSBuild $Everything $Opts $CI_Props `
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

Task Coverage `
    -Description 'Build core libraries for OpenCover.' `
    -Depends _CI-InitializeVariables `
    -Alias Cover `
{
    # Use debug build to also cover debug-only tests.
    MSBuild $Foundations $Opts $CI_Props `
        '/p:Configuration=Debug',
        '/t:Build',
        # For static analysis, we hide internals, otherwise we might not truly
        # analyze the public API.
        '/p:VisibleInternals=false',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true',
        '/p:Filter=_Core_'
}

Task CodeAnalysis `
    -Description 'Run Code Analysis on core libraries (SLOW).' `
    -Depends _CI-InitializeVariables `
    -Alias CA `
{
    MSBuild $Foundations $Opts $CI_Props `
        '/t:Build',
        # For static analysis, we hide internals, otherwise we might not truly
        # analyze the public API.
        '/p:VisibleInternals=false',
        '/p:RunCodeAnalysis=true',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true',
        '/p:Filter=_Core_'
}

Task GendarmeAnalysis `
    -Description 'Build core libraries for Mono.Gendarme.' `
    -Depends _CI-InitializeVariables `
    -Alias Keuf `
{
    # Currently only prepare the assemblies for Gendarme.
    MSBuild $Foundations $Opts $CI_Props `
        '/t:Build',
        # For static analysis, we hide internals, otherwise we might not truly
        # analyze the public API.
        '/p:EnableGendarme=true',
        '/p:VisibleInternals=false',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true',
        '/p:Filter=_Core_'
}

Task CodeContractsAnalysis `
    -Description 'Run Code Contracts Analysis on core libraries (EXTREMELY SLOW).' `
    -Depends _CI-InitializeVariables `
    -Alias CC `
{
    MSBuild $Foundations $Opts $CI_Props `
        '/t:Build',
        # For static analysis, we hide internals, otherwise we might not truly
        # analyze the public API.
        '/p:VisibleInternals=false',
        '/p:Configuration=CodeContracts',
        '/p:Filter=_Core_'
}

Task SecurityAnalysis `
    -Description 'Run SecAnnotate on core libraries (SLOW).' `
    -Depends _CI-InitializeVariables `
    -Alias SA `
{
    MSBuild $Foundations $Opts $CI_Props `
        '/t:Clean;SecAnnotate',
        # For static analysis, we hide internals, otherwise we might not truly
        # analyze the public API.
        '/p:VisibleInternals=false',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true',
        '/p:Filter=_Core_'
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
    $script:CI_Props = `
        '/p:Configuration=Release',
        '/p:BuildGeneratedVersion=false',
        "/p:Retail=$Retail",
        '/p:SignAssembly=false',
        '/p:SkipCodeContractsReferenceAssembly=false',
        '/p:VisibleInternals=true'

    # FIXME: Don't understand why doing what follows does not work.
    # Either MSBuild or PowerShell mixes up the MSBuild parameters.
    # The result is that Configuration property takes all following properties
    # as its value. For instance, Configuration is read as "Release /p:BuildGeneratedVersion=false...".
    # For static analysis, we hide internals, otherwise we might not truly
    # analyze the public API.
    #$script:CI_AnalysisProps = $CI_Props, '/p:VisibleInternals=false'
}

# ------------------------------------------------------------------------------
# Packaging tasks
# ------------------------------------------------------------------------------

Task Package-All `
    -Description 'Package ''Foundations''.' `
    -Depends Package-Core, Package-Mvp, Package-Build `
    -Alias Pack

Task Package-Core `
    -Description 'Package core projects.' `
    -Depends _Package-InitializeVariables `
    -Alias PackCore `
{
    MSBuild $Foundations $Opts $Package_Targets $Package_Props `
        '/p:Filter=_Core_'
}

Task Package-Mvp `
    -Description 'Package MVP projects.' `
    -Depends _Package-InitializeVariables `
    -Alias PackMvp `
{
    MSBuild $Foundations $Opts $Package_Targets $Package_Props `
        '/p:Filter=_Mvp_'
}

Task Package-Build `
    -Description 'Package the project Narvalo.Build.' `
    -Depends _Package-InitializeVariables `
    -Alias PackBuild `
{
    MSBuild $Foundations $Opts $Package_Targets $Package_Props `
        '/p:Filter=_Build_'
}

Task _Package-InitializeVariables `
    -Description 'Initialize variables only used by the Package-* tasks.' `
    -Depends _Initialize-GitCommitHash, _Package-CheckVariablesForRetail `
    -RequiredVariables Retail `
{
    # Packaging properties:
    # - Release configuration
    # - Generate assembly versions (necessary for NuGet packaging)
    # - Sign assemblies
    # - Do not skip the generation of the Code Contracts reference assembly
    # - Unconditionally hide internals (implies no white-box testing)
    $script:Package_Props = `
        '/p:Configuration=Release',
        '/p:BuildGeneratedVersion=true',
        "/p:GitCommitHash=$GitCommitHash",
        "/p:Retail=$Retail",
        '/p:SignAssembly=true',
        '/p:SkipCodeContractsReferenceAssembly=false',
        '/p:VisibleInternals=false'

    # Packaging targets:
    # - Rebuild all
    # - Verify Portable Executable (PE) format
    # - Run Xunit tests
    # - Package
    $script:Package_Targets = '/t:Rebuild;PEVerify;Xunit;Package'
}

Task _Package-CheckVariablesForRetail `
    -Description 'Check conditions are met for creating retail packages.' `
    -Depends _Initialize-GitCommitHash `
    -PreCondition { $Retail } `
{
    if ($GitCommitHash -eq '') {
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
    -Depends _Publish-DependsOn, Package-Core `
    -RequiredVariables Retail `
    -Alias PushCore `
{
    Invoke-NuGetAgent $Publish_StagingDirectory -Retail:$Retail
}

Task Publish-Mvp `
    -Description 'Publish MVP projects.' `
    -Depends _Publish-DependsOn, Package-Mvp `
    -RequiredVariables Retail `
    -Alias PushMvp `
{
    Invoke-NuGetAgent $Publish_StagingDirectory -Retail:$Retail
}

Task Publish-Build `
    -Description 'Publish the project Narvalo.Build.' `
    -Depends _Publish-DependsOn, Package-Build `
    -RequiredVariables Retail `
    -Alias PushBuild `
{
    Invoke-NuGetAgent $Publish_StagingDirectory -Retail:$Retail
}

Task _Publish-DependsOn `
    -Description 'Dependencies shared among the Publish-* tasks.' `
    -Depends _Publish-InitializeVariables, _Publish-Clean, NuGetAgent-Build

Task _Publish-Clean `
    -Description 'Remove the staging directory for packages.' `
    -Depends _Publish-InitializeVariables `
{
    Remove-LocalItem -Path $Publish_StagingDirectory -Recurse
}

Task _Publish-InitializeVariables `
    -Description 'Initialize variables only used by the Publish-* tasks.' `
{
    $script:Publish_StagingDirectory = Get-LocalPath 'work\packages'
}

# ------------------------------------------------------------------------------
# NuGetAgent project
# ------------------------------------------------------------------------------

Task NuGetAgent-Build `
    -Description 'Build the project NuGetAgent.' `
    -Depends _NuGetAgent-InitializeVariables, _NuGetAgent-RestorePackages `
    -Alias NuGetAgent `
{
    MSBuild $NuGetAgent_Project  $Opts '/p:Configuration=Release', '/t:Build'
}

Task _NuGetAgent-RestorePackages `
    -Description 'Restore packages for the project NuGetAgent.' `
    -Depends _NuGetAgent-InitializeVariables, _Tools-InitializeVariables `
{
    Restore-Packages -Source $NuGetAgent_PackagesConfig `
        -PackagesDirectory $Tools_PackagesDirectory `
        -ConfigFile $Tools_NuGetConfig `
        -Verbosity $NuGetVerbosity
}

Task _NuGetAgent-InitializeVariables `
    -Description 'Initialize variables only used by the NuGetAgent-* tasks.' `
{
    $script:NuGetAgent_Project        = Get-LocalPath 'tools\NuGetAgent\NuGetAgent.fsproj'
    $script:NuGetAgent_PackagesConfig = Get-LocalPath 'tools\NuGetAgent\packages.config'
}

# ------------------------------------------------------------------------------
# MyGet project
# ------------------------------------------------------------------------------

Task MyGet-Package `
    -Description 'Package the project MyGet.' `
    -Depends _MyGet-InitializeVariables, _MyGet-DeleteStagingDirectory, _MyGet-Publish, _MyGet-Zip `
    -Alias MyGet `
{
    Write-Host "A ready to publish zip file for MyGet may be found here: '$MyGet_ZipFile'." -ForegroundColor Green
}

Task _MyGet-Publish `
    -Description 'Clean up, build then publish the project MyGet.' `
    -Depends _MyGet-InitializeVariables, _MyGet-RestorePackages `
{
    # Force the value of "VisualStudioVersion", otherwise MSBuild won't publish the project on build.
    # Cf. http://sedodream.com/2012/08/19/VisualStudioProjectCompatabilityAndVisualStudioVersion.aspx.
    MSBuild $MyGet_Project $Opts `
        '/t:Rebuild',
        '/p:Configuration=Release;PublishProfile=NarvaloOrg;DeployOnBuild=true;VisualStudioVersion=12.0'
}

Task _MyGet-RestorePackages `
    -Description 'Restore packages for the project MyGet.' `
    -Depends _MyGet-InitializeVariables, _Tools-InitializeVariables `
{
    Restore-Packages -Source $MyGet_PackagesConfig `
        -PackagesDirectory $Tools_PackagesDirectory `
        -ConfigFile $Tools_NuGetConfig `
        -Verbosity $NuGetVerbosity
}

Task _MyGet-DeleteStagingDirectory `
    -Description 'Remove the directory ''work\myget''.' `
    -Depends _MyGet-InitializeVariables `
{
    Remove-LocalItem -Path $MyGet_StagingDirectory -Recurse
}

Task _MyGet-DeleteZipFile `
    -Description 'Delete the package for MyGet.' `
    -Depends _MyGet-InitializeVariables `
{
    Remove-LocalItem -Path $MyGet_ZipFile
}

Task _MyGet-Zip `
    -Description 'Zip the publication artefacts for the project MyGet.' `
    -Depends _MyGet-InitializeVariables, _MyGet-DeleteZipFile `
    -PreAction {
        if (!(Test-Path $MyGet_StagingDirectory)) {
            # We do not add a dependency on _MyGet-Publish so that we can run this task alone.
            # MyGet-Package provides the stronger version.
            Exit-Gracefully -ExitCode 1 `
                'Can not create the Zip package: did you forgot to call the _MyGet-Publish task?'
        }
    } -Action {
        . (Get-7Zip -Install) -mx9 a $MyGet_ZipFile $MyGet_StagingDirectory | Out-Null

        if (!$?) {
            Exit-Gracefully -ExitCode 1 'Failed to create the Zip package.'
        }
    }

Task _MyGet-InitializeVariables `
    -Description 'Initialize variables only used by the MyGet-* tasks.' `
{
    $script:MyGet_Project          = Get-LocalPath 'tools\MyGet\MyGet.csproj'
    $script:MyGet_PackagesConfig   = Get-LocalPath 'tools\MyGet\packages.config'
    $script:MyGet_StagingDirectory = Get-LocalPath 'work\myget'
    $script:MyGet_ZipFile          = Get-LocalPath 'work\myget.7z'
}

# ------------------------------------------------------------------------------
# Edge solution
# ------------------------------------------------------------------------------

Task Edge-FullBuild `
    -Description 'Update then re-build the solution Edge.' `
    -Depends _Edge-Update, _Edge-Rebuild

Task _Edge-Rebuild `
    -Description 'Re-build the solution Edge.' `
    -Depends _Edge-InitializeVariables, _Edge-RestorePackages `
{
    MSBuild $Edge_Solution $Opts '/p:Configuration=Release', '/t:Rebuild'
}

Task _Edge-RestorePackages `
    -Description 'Restore packages for the solution Edge.' `
    -Depends _Edge-InitializeVariables `
{
    Restore-Packages -Source $Edge_Solution `
        -PackagesDirectory $Edge_PackagesDirectory `
        -ConfigFile $Edge_NuGetConfig `
        -Verbosity $NuGetVerbosity
}

Task _Edge-Update `
    -Description 'Safely update NuGet packages for the solution Edge.' `
    -Depends _Edge-InitializeVariables `
{
    $nuget = Get-NuGet -Install

    try {
        . $nuget update $Edge_Solution `
            -Safe `
            -RepositoryPath $Edge_PackagesDirectory `
            -ConfigFile $Edge_NuGetConfig `
            -Verbosity $NuGetVerbosity
    } catch {
        Exit-Gracefully -ExitCode 1 "'nuget.exe update' failed: $_"
    }
}

Task _Edge-InitializeVariables `
    -Description 'Initialize variables only used by the Edge-* tasks.' `
{
    $script:Edge_Solution          = Get-LocalPath 'tools\MyPackages\Edge\Edge.sln'
    $script:Edge_NuGetConfig       = Get-LocalPath 'tools\MyPackages\Edge\.nuget\NuGet.Config'
    $script:Edge_PackagesDirectory = Get-LocalPath 'tools\MyPackages\Edge\packages'
}

# ------------------------------------------------------------------------------
# Retail solution
# ------------------------------------------------------------------------------

Task Retail-FullBuild `
    -Description 'Update then re-build the solution Retail.' `
    -Depends _Retail-Update, _Retail-Rebuild

Task _Retail-Rebuild `
    -Description 'Re-build the solution Retail.' `
    -Depends _Retail-InitializeVariables, _Retail-RestorePackages `
{
    MSBuild $Retail_Solution $Opts '/p:Configuration=Release', '/t:Rebuild'
}

Task _Retail-RestorePackages `
    -Description 'Restore packages for the solution Retail.' `
    -Depends _Retail-InitializeVariables `
{
    Restore-Packages -Source $Retail_Solution `
        -PackagesDirectory $Retail_PackagesDirectory `
        -ConfigFile $Retail_NuGetConfig `
        -Verbosity $NuGetVerbosity
}

Task _Retail-Update `
    -Description 'Safely update NuGet packages for the solution Retail.' `
    -Depends _Retail-InitializeVariables `
{
    $nuget = Get-NuGet -Install

    try {
        . $nuget update $Retail_Solution `
            -Safe `
            -RepositoryPath $Retail_PackagesDirectory `
            -ConfigFile $Retail_NuGetConfig `
            -Verbosity $NuGetVerbosity
    } catch {
        Exit-Gracefully -ExitCode 1 "'nuget.exe update' failed: $_"
    }
}

Task _Retail-InitializeVariables `
    -Description 'Initialize variables only used by the Retail-* tasks.' `
{
    $script:Retail_Solution          = Get-LocalPath 'tools\MyPackages\Retail\Retail.sln'
    $script:Retail_NuGetConfig       = Get-LocalPath 'tools\MyPackages\Retail\.nuget\NuGet.Config'
    $script:Retail_PackagesDirectory = Get-LocalPath 'tools\MyPackages\Retail\packages'
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
    $git = (Get-Git)

    $hash = ''

    if ($git -ne $null) {
        $status = Get-GitStatus $git -Short

        if ($status -eq $null) {
            Write-Warning 'Skipping... unabled to verify the git status.'
        } elseif ($status -ne '') {
            Write-Warning 'Skipping... uncommitted changes are pending.'
        } else {
            $hash = Get-GitCommitHash $git
        }
    }

    $script:GitCommitHash = $hash
}

Task _Tools-InitializeVariables `
    -Description 'Initialize variables for the tooling projects.' `
{
    $script:Tools_PackagesDirectory = Get-LocalPath 'tools\packages'
    $script:Tools_NuGetConfig       = Get-LocalPath 'tools\.nuget\NuGet.Config'
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

function Invoke-NuGetAgent {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0)] [string] $Path,

        [Parameter(Mandatory = $false, Position = 1)] [string] $Configuration = 'Release',

        [switch] $Retail
    )

    try {
        $cmd = Get-LocalPath "tools\NuGetAgent\bin\$Configuration\nuget-agent.exe" -Resolve

        if ($Retail) {
            . $cmd --path $path --retail 2>&1
        } else {
            . $cmd --path $path 2>&1
        }
    } catch {
        Exit-Gracefully -ExitCode 1 "'nuget-agent.exe' failed: $_"
    }
}

# ------------------------------------------------------------------------------
