// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

[<AutoOpen>]
module Core

open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Xml.Linq

open NuGet

/// Machine-wide settings for NuGet.
type MachineWideSettings(settings:IEnumerable<Settings>) =
    let _settings = settings

    interface IMachineWideSettings with
        member this.Settings
            with get() = _settings

/// Helper to get the NuGet settings.
let loadNuGetSettings =
    let baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
    let machineWideSettings = new MachineWideSettings(NuGet.Settings.LoadMachineWideSettings(new PhysicalFileSystem(baseDirectory)))

    NuGet.Settings.LoadDefaultSettings(
        new PhysicalFileSystem(Directory.GetCurrentDirectory()), 
        configFileName=null, 
        machineWideSettings=machineWideSettings)

/// Container for the various NuGet API keys.
type ApiKeysContainer(settings:ISettings) =
    let _myGetApiKey = lazy( ApiKeysContainer._ReadApiKey(settings, Constants.MyGetApiSource) )
    let _nuGetApiKey = lazy( ApiKeysContainer._ReadApiKey(settings, Constants.NuGetSource) )

    member this.MyGetApiKey with get() = _myGetApiKey.Value
    member this.NuGetApiKey with get() = _nuGetApiKey.Value
    
    static member Load =
        let settings = loadNuGetSettings

        new ApiKeysContainer(settings)
        
    /// Read the API Key from the settings.
    static member private _ReadApiKey(settings:ISettings, source:string) =
        settings.GetDecryptedValue(Constants.ApiKeysSectionName, source)
        
/// Read-Only package repository, copied from NuGet.Common
type ReadOnlyPackageRepository(packages:IEnumerable<IPackage>) = 
    inherit PackageRepositoryBase()

    let _packages = packages

    override this.Source = null
    override this.SupportsPrereleasePackages = true
    override this.GetPackages() = _packages.AsQueryable()
    
/// Get the list of packages to be published, sorted by dependency order.
/// If retail is true, only include retail packages.
/// If retail is false, only include edge packages.
let getPackagesToBePublished path retail =
    let repository = new LocalPackageRepository(path, enableCaching=false)

    // Find packages, keeping only retail or edge packages.
    let packages = 
        repository.GetPackages() 
        |> Seq.filter(fun p ->
            if retail then not(p.Id.EndsWith(Constants.EdgePackageSuffix)) 
            else p.Id.EndsWith(Constants.EdgePackageSuffix))

    // Collapse the list of packages, keeping only the latest version.
    let uniqPackages = packages.AsCollapsed()

    // Sort these packages by the dependency order.
    let sorter = new PackageSorter(targetFramework=null);
    
    sorter.GetPackagesByDependencyOrder(new ReadOnlyPackageRepository(uniqPackages))

/// Interface for NuGet.Core.
type INuGetFacade =
    /// Get the list of packages to be published, sorted by dependency order.
    abstract member GetPackagesToBePublished: path:string -> IEnumerable<IPackage>

    /// Publish a package.
    abstract member PublishPackage: package:IPackage * ?clean:bool -> unit

/// Facade for the MyGet server.
type MyGetFacade(apiKeysContainer:ApiKeysContainer, ?verbose) =
    /// API Key for the MyGet server.
    let _apiKey = apiKeysContainer.MyGetApiKey
    /// Be verbose?
    let _verbose = defaultArg verbose true 

    /// MyGet server.
    let _server = new PackageServer(Constants.MyGetApiSource, Constants.UserAgent)
    /// MyGet repository.
    let _repository = PackageRepositoryFactory.Default.CreateRepository Constants.MyGetSource
    /// Disable buffering?
    let _disableBuffering = true
    /// Timeout in milliseconds (1 minute).
    let _timeout = 60000
    
    interface INuGetFacade with
        member this.GetPackagesToBePublished(path:string) = 
            getPackagesToBePublished path false

        /// Publish a package.
        member this.PublishPackage(package:IPackage, ?clean) = 
            if _verbose then 
                printfn "Publishing package %s, version %s" <| package.Id <| package.Version.ToString()

            match clean with
            | Some(true) | None 
                -> this.DeletePreviousVersionsOfPackage(package.Id)
            | Some(false) -> ()

            //_server.PushPackage(_apiKey, package, package.GetStream().Length, _timeout, _disableBuffering)
          
    /// Delete a package.
    member this.DeletePackage(id, version) = 
        if _verbose then printfn "Deleting package %s, version %s" id version

        //_server.DeletePackage(_apiKey, id, version)
      
    /// Delete all versions of a package, except the last one.
    member this.DeletePreviousVersionsOfPackage(id) = 
        let packages = _repository.FindPackagesById id

        for package in packages do this.DeletePackage(package.Id, package.Version.ToString())
        
/// Facade for the NuGet server.
type NuGetFacade(apiKeysContainer:ApiKeysContainer, ?verbose) =
    /// API Key for the MyGet server.
    let _apiKey = apiKeysContainer.NuGetApiKey
    /// Be verbose?
    let _verbose = defaultArg verbose true 
       
    interface INuGetFacade with
        member this.GetPackagesToBePublished(path:string) = 
            getPackagesToBePublished path true

        /// Publish a package.
        member this.PublishPackage(package:IPackage, ?clean) = 
            if _verbose then 
                printfn "Publishing package %s, version %s" <| package.Id <| package.Version.ToString()
                
            // - Find current public version of packages
            // - Filter already public packages
            // - Check the dependency tree
            // - Publish packages in the reverse order of the dependency tree
