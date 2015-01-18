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

Task CodeAnalysis `
    -Description 'Run Code Analysis (SLOW).' `
    -Depends _CI-InitializeVariables `
    -Alias CA `
{
    MSBuild $Everything $Opts $CI_AnalysisProps `
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
    MSBuild $Foundations $Opts $CI_AnalysisProps `
        '/t:Build',
        '/p:Configuration=CodeContracts'
} 

Task SecurityAnalysis `
    -Description 'Run SecAnnotate on ''Foundations'' (SLOW).' `
    -Depends _CI-InitializeVariables `
    -Alias SA `
{
    MSBuild $Foundations $Opts $CI_AnalysisProps `
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
    $script:CI_Props = `
        '/p:Configuration=Release',
        '/p:BuildGeneratedVersion=false',
        "/p:Retail=$Retail",
        '/p:SignAssembly=false',
        '/p:SkipCodeContractsReferenceAssembly=false',
        '/p:VisibleInternals=true'

    # For static analysis, we hide internals, otherwise we might not truly 
    # analyze the public API.
    $script:CI_AnalysisProps = $CI_Props, '/p:VisibleInternals=false'
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
    -Depends _Package-InitializeVariables, _Package-ValidateRetail `
    -Alias PackCore `
{
    MSBuild $Foundations $Opts $Package_Targets $Package_Props `
        '/p:Filter=_Core_' 
}

Task Package-Mvp `
    -Description 'Package MVP projects.' `
    -Depends _Package-InitializeVariables, _Package-ValidateRetail `
    -Alias PackMvp `
{
    MSBuild $Foundations $Opts $Package_Targets $Package_Props `
        '/p:Filter=_Mvp_' 
}

Task Package-Build `
    -Description 'Package the project Narvalo.Build.' `
    -Depends _Package-InitializeVariables, _Package-ValidateRetail `
    -Alias PackBuild `
{
    MSBuild $Foundations $Opts $Package_Targets $Package_Props `
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

Task _Package-ValidateRetail `
    -Description 'Validate retail packaging tasks.' `
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
    -Depends _Publish-InitializeVariables, _Publish-DependsOn, Package-Core `
    -RequiredVariables Retail `
    -Alias PushCore `
{
    Publish-Packages $Publish_StagingDirectory -Retail:$Retail
}

Task Publish-Mvp `
    -Description 'Publish MVP projects.' `
    -Depends _Publish-InitializeVariables, _Publish-DependsOn, Package-Mvp `
    -RequiredVariables Retail `
    -Alias PushMvp `
{
    Publish-Packages $Publish_StagingDirectory -Retail:$Retail
}

Task Publish-Build `
    -Description 'Publish the project Narvalo.Build.' `
    -Depends _Publish-InitializeVariables, _Publish-DependsOn, Package-Build `
    -RequiredVariables Retail `
    -Alias PushBuild `
{
    Publish-Packages $Publish_StagingDirectory -Retail:$Retail
}

Task _Publish-DependsOn `
    -Description 'Dependencies shared among the Publish-* tasks.' `
    -Depends _Publish-Clean, NuGetHelper-Build

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
# NuGetHelper project
# ------------------------------------------------------------------------------

Task NuGetHelper-Build `
    -Description 'Build the project NuGetHelper.' `
    -Depends _NuGetHelper-InitializeVariables, _NuGetHelper-RestorePackages `
    -Alias NuGetHelper `
{
    MSBuild $NuGetHelper_Project  $Opts '/p:Configuration=Release', '/t:Build'
}

Task _NuGetHelper-RestorePackages `
    -Description 'Restore packages for the project NuGetHelper.' `
    -Depends _NuGetHelper-InitializeVariables, _Tools-InitializeVariables `
{
    Restore-Packages -Source $NuGetHelper_PackagesConfig `
        -PackagesDirectory $Tools_PackagesDirectory `
        -ConfigFile $Tools_NuGetConfig `
        -Verbosity $NuGetVerbosity
}

Task _NuGetHelper-InitializeVariables `
    -Description 'Initialize variables only used by the NuGetHelper-* tasks.' `
{
    $script:NuGetHelper_Project        = Get-LocalPath 'tools\NuGetHelper\NuGetHelper.fsproj'
    $script:NuGetHelper_PackagesConfig = Get-LocalPath 'tools\NuGetHelper\packages.config'
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
    MSBuild $Edge_Project $Opts '/p:Configuration=Release', '/t:Rebuild'
}

Task _Edge-RestorePackages `
    -Description 'Restore packages for the project Edge.' `
    -Depends _Edge-InitializeVariables, _Tools-InitializeVariables `
{
    Restore-Packages -Source $Edge_PackagesConfig `
        -PackagesDirectory $Tools_PackagesDirectory `
        -ConfigFile $Tools_NuGetConfig `
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
        . $nuget update $Edge_PackagesConfig `
            -Source "https://www.nuget.org/api/v2/" `
            -RepositoryPath $Tools_PackagesDirectory `
            -Verbosity $NuGetVerbosity

        . $nuget update $Edge_PackagesConfig `
            -Source "http://narvalo.org/myget/nuget/" `
            -Prerelease `
            -RepositoryPath $Tools_PackagesDirectory `
            -Verbosity $NuGetVerbosity
    } catch {
        Exit-Gracefully -ExitCode 1 "'nuget.exe update' failed: $_"
    }
}

Task _Edge-InitializeVariables `
    -Description 'Initialize variables only used by the Edge-* tasks.' `
{
    $script:Edge_Project        = Get-LocalPath 'tools\Edge\Edge.csproj'
    $script:Edge_PackagesConfig = Get-LocalPath 'tools\Edge\packages.config'
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
                'Unabled to create the Zip package: did you forgot to call the _MyGet-Publish task?'
        }
    } -Action {
        . (Get-7Zip -Install) -mx9 a $MyGet_ZipFile $MyGet_StagingDirectory | Out-Null

        if (!$?) {
            Exit-Gracefully -ExitCode 1 'Unabled to create the Zip package.'
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

function Publish-Packages {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)] [string] $Path,

        [switch] $Retail
    ) 

    try {
        $cmd = Get-LocalPath 'tools\NuGetHelper\bin\Release\NuGetHelper.exe' -Resolve

        if ($Retail) {
            . $cmd --path $path --retail 2>&1
        } else {
            . $cmd --path $path 2>&1
        }
    } catch {
        Exit-Gracefully -ExitCode 1 "'NuGetHelper.exe' failed: $_"
    }
}

# ------------------------------------------------------------------------------
