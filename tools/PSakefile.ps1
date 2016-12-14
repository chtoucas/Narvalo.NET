# PSakefile script.
# Requires the module 'Narvalo.ProjectAutomation'.
#

# We force the framework to be sure we use the v12.0 of the build tools.
# For instance, this is a requirement for the _MyGet-Publish target where
# the DeployOnBuild instruction is not understood by previous versions of MSBuild.
# TODO: Check if this is still necessary.
# NB: We could also use
#   Invoke-psake -framework '4.5.1x64' in make.ps1.
#Framework '4.5.1x64'
Framework '4.6.1x64'

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

    # NuGet packages.
    # TODO: Make it survive NuGet updates.
    $OpenCoverVersion       = '4.6.519'
    $ReportGeneratorVersion = '2.5.1'
    $XunitVersion           = '2.1.0'

    $OpenCoverXml = Get-LocalPath 'work\log\opencover.xml'
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

Task Default -Depends Build

# ------------------------------------------------------------------------------
# Continuous Integration and development tasks
# ------------------------------------------------------------------------------

# NB: No need to restore packages before building the projects $Everything
# or $Foundations; this will be done in MSBuild.

Task FullClean `
    -Description 'Delete permanently the "work" directory.' `
    -Alias Clean `
    -ContinueOnError `
{
    # Sometimes this task fails for some obscure reasons. Maybe the directory is locked?
    Remove-LocalItem 'work' -Recurse
}

Task Build `
    -Description 'Build.' `
    -Depends _CI-InitializeVariables `
{
    MSBuild $Everything $Opts $CI_Props '/t:Build'
}

Task Test `
    -Description 'Build then run tests.' `
    -Depends _CI-InitializeVariables `
{
    MSBuild $Everything $Opts $CI_Props `
        '/t:Xunit',
        '/p:Configuration=Debug',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true'
}

Task OpenCover `
    -Description 'Run OpenCover (summary only).' `
    -Depends _CI-InitializeVariables `
    -Alias Cover `
{
    # Use debug build to also cover debug-only tests.
    MSBuild $Foundations $Opts $CI_Props `
        '/t:Build',
        '/p:Configuration=Debug',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true',
        '/p:Filter="_Core_;_Mvp_"'

    Invoke-OpenCover 'Debug'
    Invoke-ReportGenerator -Summary
}

Task OpenCoverVerbose `
    -Description 'Run OpenCover (full details).' `
    -Depends _CI-InitializeVariables `
    -Alias CoverVerbose `
{
    # Use debug build to also cover debug-only tests.
    MSBuild $Foundations $Opts $CI_Props `
        '/t:Build',
        '/p:Configuration=Debug',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true',
        '/p:Filter="_Core_;_Mvp_"'

    Invoke-OpenCover 'Debug'
    Invoke-ReportGenerator
}

Task CodeAnalysis `
    -Description 'Build, analyze, then run PEVerify.' `
    -Depends _CI-InitializeVariables `
    -Alias CA `
{
    $output = Get-LocalPath 'work\log\code-analysis.log'

    # Perform the following operations:
    # - Build all projects
    # - Run Source Analysis
    # - Verify Portable Executable (PE) format
    # NB: For static analysis, we hide internals, otherwise we might not truly
    # analyze the public API.
    # NB: Adding Build to the targets is not necessary, but it makes clearer that
    # we do not just run PEVerify. In fact, we need to rebuild otherwise CA might fail.
    # NB: Removed '/p:SourceAnalysisEnabled=true' (replaced by StyleCop.Analyzers)
    MSBuild $Foundations $Opts $CI_Props `
        '/t:Rebuild;PEVerify',
        '/p:Configuration=Debug',
        '/p:VisibleInternals=false',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true',
        '/p:RunCodeAnalysis=true',
        '/p:Filter=_Analyze_' | Tee-Object -file $output
}

Task CodeContractsAnalysis `
    -Description 'Run Code Contracts Analysis.' `
    -Depends _CI-InitializeVariables `
    -Alias CC `
{
    $output = Get-LocalPath 'work\log\code-contracts.log'

    # For static analysis, we hide internals, otherwise we might not truly
    # analyze the public API.
    MSBuild $Foundations $Opts $CI_Props `
        '/t:Build',
        '/p:VisibleInternals=false',
        '/p:Configuration=CodeContracts',
        '/p:Filter=_CodeContracts_' | Tee-Object -file $output
}

Task SecurityAnalysis `
    -Description 'Run Security Analysis.' `
    -Depends _CI-InitializeVariables `
    -Alias SA `
{
    $output = Get-LocalPath 'work\log\security-analysis.log'

    # Keep the PEVerify target (see the comments in the MSBuild target _PEVerify).
    MSBuild $Foundations $Opts $CI_Props `
        '/t:SecAnnotate;PEVerify',
        # For static analysis, we hide internals, otherwise we might not truly
        # analyze the public API.
        '/p:VisibleInternals=false',
        '/p:SignAssembly=true',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true',
        '/p:EnableSecurityAnnotations=true',
        '/p:Filter=_Security_' | Tee-Object -file $output
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
    # FIXME: CodeContracts disabled.
    $script:CI_Props = `
        '/p:Configuration=Release',
        '/p:BuildGeneratedVersion=false',
        "/p:Retail=$Retail",
        '/p:SignAssembly=false',
        '/p:SkipCodeContractsReferenceAssembly=false',
        '/p:VisibleInternals=true',
        '/p:EnableSecurityAnnotations=false'

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
    -Description 'Package everything.' `
    -Depends Package-Core, Package-Mvp, Package-Build `
    -Alias Pack

Task Package-Core `
    -Description 'Create the core NuGet packages.' `
    -Depends _Package-InitializeVariables `
    -Alias PackCore `
{
    MSBuild $Foundations $Opts $Package_Targets $Package_Props `
        '/p:Filter=_Core_'
}

Task Package-Mvp `
    -Description 'Create the MVP-related packages.' `
    -Depends _Package-InitializeVariables `
    -Alias PackMvp `
{
    MSBuild $Foundations $Opts $Package_Targets $Package_Props `
        '/p:Filter=_Mvp_'
}

Task Package-Build `
    -Description 'Create the Narvalo.Build package.' `
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
    # FIXME: CodeContracts disabled.
    $script:Package_Props = `
        '/p:Configuration=Release',
        '/p:BuildGeneratedVersion=true',
        "/p:GitCommitHash=$GitCommitHash",
        "/p:Retail=$Retail",
        '/p:SignAssembly=true',
        '/p:SkipCodeContractsReferenceAssembly=false',
        '/p:VisibleInternals=false',
        '/p:EnableSecurityAnnotations=false'

    # Packaging targets:
    # - Rebuild all
    # - Verify Portable Executable (PE) format
    # - Run Xunit tests
    # - Package
    $script:Package_Targets = '/t:PEVerify;Xunit;Package'
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
        '/p:Configuration=Release;PublishProfile=NarvaloOrg;DeployOnBuild=true;VisualStudioVersion=14.0'
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
    $script:MyGet_Project          = Get-LocalPath 'tools\src\MyGet\MyGet.csproj'
    $script:MyGet_PackagesConfig   = Get-LocalPath 'tools\src\MyGet\packages.config'
    $script:MyGet_StagingDirectory = Get-LocalPath 'work\myget'
    $script:MyGet_ZipFile          = Get-LocalPath 'work\myget.7z'
}

# ------------------------------------------------------------------------------
# Miscs
# ------------------------------------------------------------------------------

Task RestoreSolutionPackages `
    -Description 'Restore solution-level packages.' `
    -Alias restore `
{
    # Usually, it is not necessary to run this task, since it is already done in make.ps1.
    Restore-SolutionPackages -Verbosity quiet
}

Task Environment `
    -Description 'Display infos on the build environment.' `
    -Alias Env `
{
    # The output of running "MSBuild /version" looks like:
    # >   Microsoft (R) Build Engine, version 12.0.31101.0
    # >   [Microsoft .NET Framework, Version 4.0.30319.34209]
    # >   Copyright (C) Microsoft Corporation. Tous droits réservés.
    # >
    # >   12.0.31101.0
    # Since VS 2015, we no longer get the framework version.
    $infos = (MSBuild '/version') -Split "`n", 3

    $msbuild = $infos[3]
    #$netFramework = ($infos[1] -Split ' ', 5)[4].TrimEnd(']')
    $psakeFramework = $psake.context.peek().config.framework
    $psakeVersion = $psake.version

    Write-Host "  MSBuild         v$msbuild"
    #Write-Host "  .NET Framework  v$netFramework"
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

# TODO: Should be a task.
function Invoke-OpenCover {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true, Position = 0)]
        [string] $Configuration
    )

    $opencover = Get-LocalPath "packages\OpenCover.$OpenCoverVersion\tools\OpenCover.Console.exe" -Resolve
    $xunit     = Get-LocalPath "packages\xunit.runner.console.$XunitVersion\tools\xunit.console.exe" -Resolve

    $filter = '+[Narvalo*]* -[*Facts]* -[Xunit.*]*'
    $excludeByAttribute = 'System.Runtime.CompilerServices.CompilerGeneratedAttribute;Narvalo.ExcludeFromCodeCoverageAttribute;System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute'

    $asm1 = Get-LocalPath "work\bin\$Configuration\Narvalo.Facts.dll" -Resolve
    $asm2  = Get-LocalPath "work\bin\$Configuration\Narvalo.Mvp.Facts.dll" -Resolve
    $asms = "$asm1 $asm2"

    # Be very careful with arguments containing spaces.
    . $opencover `
      -register:user `
      "-filter:$filter" `
      "-excludebyattribute:$excludeByAttribute" `
      "-output:$OpenCoverXml" `
      "-target:$xunit"  `
      "-targetargs:$asms -nologo -noshadow"
}

# TODO: Should be a task.
function Invoke-ReportGenerator {
    [CmdletBinding()]
    param(
        [switch] $Summary
    )

    $reportgenerator = Get-LocalPath "packages\ReportGenerator.$ReportGeneratorVersion\tools\ReportGenerator.exe" -Resolve

    if ($summary.IsPresent) {
        $targetdir   = Get-LocalPath 'work\log'
        $filters     = '+*'
        $reporttypes = 'HtmlSummary'
    }
    else {
        $targetdir   = Get-LocalPath 'work\log\opencover'
        $filters     = '-Narvalo.Common;-Narvalo.Core;-Narvalo.Fx;-Narvalo.Mvp;-Narvalo.Mvp.Web;-Narvalo.Web'
        $reporttypes = 'Html'
    }

    . $reportgenerator `
        -verbosity:Info `
        -reporttypes:$reporttypes `
        "-filters:$filters" `
        -reports:$OpenCoverXml `
        -targetdir:$targetdir
}

# ------------------------------------------------------------------------------
