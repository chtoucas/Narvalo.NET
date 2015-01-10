
# We force the framework to be sure we use the build tools v12.0.
# Required by the Build-MyGet target.
Framework '4.5.1x64'

# ------------------------------------------------------------------------------
# Properties
# ------------------------------------------------------------------------------

Properties {
    Assert ($verbosity -ne $null) "`$verbosity should not be null, e.g. run with -Parameters @{ 'verbosity' = 'minimal'; }"
    Assert ($retail -ne $null) "`$retail should not be null, e.g. run with -Parameters @{ 'retail' = $true; }"

    $WorkRoot = Get-LocalPath 'work' 
    $PackagesDir = Get-LocalPath 'work\packages'
    
    $MyGetDir = Join-Path $WorkRoot 'myget'
    $MyGetPkg = Join-Path $WorkRoot 'myget.7z'

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
    
    Write-Host "Executing Task '$taskName'." -ForegroundColor Green
}

TaskTearDown {
    if ($LastExitCode -ne 0) {
        Exit-ErrorGracefully 'Build failed.'
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
    if (Test-Path $WorkRoot) {
        Remove-Item $WorkRoot -Force -Recurse
    }
}

Task CodeAnalysis `
    -Description 'Run Code Analysis (slow).' `
    -Alias CA `
{
    MSBuild $Everything $Opts $StaticAnalysisProps `
        '/t:Build', 
        '/p:RunCodeAnalysis=true',
        '/p:SkipCodeContractsReferenceAssembly=true',
        '/p:SkipDocumentation=true'
}

Task CodeContractsAnalysis `
    -Description 'Run Code Contracts Analysis on ''Foundations'' (extremely slow).' `
    -Alias CC `
{
    MSBuild $Foundations $Opts $StaticAnalysisProps `
        '/t:Build',
        '/p:Configuration=CodeContracts'
} 

Task SecurityAnalysis `
    -Description 'Run SecAnnotate on ''Foundations'' (slow).' `
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
    -Depends _Set-GitCommitHash, _Validate-Packaging `
    -Alias Pack `
{
    MSBuild $Foundations $Opts $PackagingTargets $PackagingProps `
        "/p:GitCommitHash=$GitCommitHash"
}

Task Package-Core `
    -Description 'Package core projects.' `
    -Depends _Set-GitCommitHash, _Validate-Packaging `
    -Alias PackCore `
{
    MSBuild $Foundations $Opts $PackagingTargets $PackagingProps `
        "/p:GitCommitHash=$GitCommitHash",
        '/p:Filter=_Core_' 
}

Task Package-Mvp `
    -Description 'Package MVP projects.' `
    -Depends _Set-GitCommitHash, _Validate-Packaging `
    -Alias PackMvp `
{
    MSBuild $Foundations $Opts $PackagingTargets $PackagingProps `
        "/p:GitCommitHash=$GitCommitHash",
        '/p:Filter=_Mvp_' 
}

Task Package-Build `
    -Description 'Package the Narvalo.Build project.' `
    -Depends _Set-GitCommitHash, _Validate-Packaging `
    -Alias PackBuild `
{
    MSBuild $Foundations $Opts $PackagingTargets $PackagingProps `
        "/p:GitCommitHash=$GitCommitHash",
        '/p:Filter=_Build_' 
} 

Task _Validate-Packaging `
    -Description 'Validate retail packaging tasks.' `
    -PreCondition { $retail } `
{
    if ($GitCommitHash -eq '') {
        Exit-ErrorGracefully 'When building retail packages, the git commit hash MUST not be empty.'
    }
}

# ------------------------------------------------------------------------------
# Publish tasks
# ------------------------------------------------------------------------------

Task Publish `
    -Description 'Publish all projects.' `
    -Alias Push `
{
    Exit-ErrorGracefully 'Not yet implemented!'

    #$nuget = Get-NuGet -Install
    #& $nuget push "$PackagesDir\*.nupkg"
}

# ------------------------------------------------------------------------------
# MyGet tasks
# ------------------------------------------------------------------------------

Task MyGet-Clean `
    -Description 'Clean MyGet server.' `
{
    if (Test-Path $MyGetDir) {
        Remove-Item $MyGetDir -Force -Recurse
    }
    if (Test-Path $MyGetPkg) {
        Remove-Item $MyGetPkg -Force
    }
}

Task MyGet-Build `
    -Description 'Build MyGet server.' `
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
{
    if (!(Test-Path $MyGetDir)) {
        Exit-ErrorGracefully 'Unabled to create the Zip package: did you forgot to call the MyGet-Build task?'
    }

    . (Get-7Zip -Install) -mx9 a $MyGetPkg $MyGetDir | Out-Null

    if (!$?) {
        Exit-ErrorGracefully 'Unabled to create the Zip package.'
    }

    Write-Host "A ready to publish zip file for MyGet may be found here: '$MyGetPkg'." -ForegroundColor Green
}

Task MyGet-Package `
    -Description 'Package MyGet server.' `
    -Depends MyGet-Build, MyGet-Zip

Task MyGet `
    -Description 'Clean then package the MyGet server.' `
    -Depends MyGet-Clean, MyGet-Build, MyGet-Zip

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

# ------------------------------------------------------------------------------
# Utilities
# ------------------------------------------------------------------------------

Task _Set-GitCommitHash `
    -Description 'Initialize GitCommitHash.' `
{
    $git = Get-Git

    if ($git -ne $null) {
        $GitCommitHash = $git | Get-GitCommitHash
    }
}

# ------------------------------------------------------------------------------
