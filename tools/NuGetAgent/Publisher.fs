// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace NuGetAgent

open System
open System.Collections.Generic
open System.Linq

open NuGet

[<RequireQualifiedAccess>]
module Publisher =
    /// Read-Only package repository, copied from NuGet.Common
    type private ReadOnlyPackageRepository(packages:IEnumerable<IPackage>) = 
        inherit PackageRepositoryBase()

        let _packages = packages

        override this.Source = null
        override this.SupportsPrereleasePackages = true
        override this.GetPackages() = _packages.AsQueryable()
    
    /// Get the list of packages to be published, sorted by dependency order.
    /// If retail is true, only include retail packages.
    /// If retail is false, only include edge packages.
    let private getPackagesToPublish path retail =
        let repository = new LocalPackageRepository(path, enableCaching=false)

        // Find packages, keeping only retail or edge packages.
        let packages = 
            repository.GetPackages() 
            |> Seq.filter(fun p ->
                let isEdge = p.Id.EndsWith(Constants.EdgePackageSuffix)
                if retail then not(isEdge) else isEdge)

        // Collapse the list of packages, keeping only the latest version.
        let uniqPackages = packages.AsCollapsed()

        // Sort these packages by the dependency order.
        let sorter = new PackageSorter(targetFramework=null);
    
        sorter.GetPackagesByDependencyOrder(new ReadOnlyPackageRepository(uniqPackages))

    /// Publisher interface.
    type IPublisher =
        /// Get the list of packages to be published, sorted by dependency order.
        abstract member FindPackagesToPublish: path:string -> IEnumerable<IPackage>

        /// Publish a package.
        abstract member PublishPackage: package:IPackage -> unit

    /// Publisher for edge packages.
    type PublisherForEdgePackages(apiKeysContainer:ApiKeysContainer, ?purge, ?verbose) =
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
    
        interface IPublisher with
            member this.FindPackagesToPublish(path:string) = 
                getPackagesToPublish path false

            /// Publish a package.
            member this.PublishPackage(package:IPackage) = 
                if _verbose 
                then printfn "> Processing package %s v%s" <| package.Id <| package.Version.ToString()

                let obsoletePackages, newerPackages = 
                    _repository.FindPackagesById package.Id 
                    |> List.ofSeq
                    |> List.partition(fun p -> p.Version < package.Version)

                // Delete obsolete packages.
                match purge with
                | Some(true) | None ->
                    obsoletePackages
                    |> List.iter(fun p -> this._DeletePackage(p.Id, p.Version.ToString()))
                | Some(false) -> ()

                // Publish the package.
                if newerPackages.Any() 
                then printfn "Skipping... a more recent version already exists."
                else this._PushPackage(package)
          
        /// Delete a package.
        member private this._DeletePackage(id, version) = 
            if _verbose then printfn "Deleting version %s" version

            _server.DeletePackage(_apiKey, id, version)
          
        /// Delete a package.
        member private this._PushPackage(package:IPackage) = 
            if _verbose then 
                printfn "Publishing package..."

            _server.PushPackage(_apiKey, package, package.GetStream().Length, _timeout, _disableBuffering)
        
    /// Publisher for retail packages.
    type PublisherForRetailPackages(apiKeysContainer:ApiKeysContainer, ?verbose) =
        /// API Key for the MyGet server.
        let _apiKey = apiKeysContainer.NuGetApiKey
        /// Be verbose?
        let _verbose = defaultArg verbose true 
       
        /// MyGet server.
        let _server = new PackageServer(Constants.NuGetSource, Constants.UserAgent)
        /// MyGet repository.
        let _repository = PackageRepositoryFactory.Default.CreateRepository Constants.NuGetSource
        /// Disable buffering?
        let _disableBuffering = true
        /// Timeout in milliseconds (1 minute).
        let _timeout = 60000

        interface IPublisher with
            member this.FindPackagesToPublish(path:string) = 
                getPackagesToPublish path true

            /// Publish a package.
            member this.PublishPackage(package:IPackage) = 
                if _verbose 
                then printfn "> Processing package %s v%s" <| package.Id <| package.Version.ToString()
                
                // - Find current public version of packages
                // - Filter already public packages
                // - Check the dependency tree
                // - Publish packages in the reverse order of the dependency tree

                ()


    let create retail =
        let settings = loadNuGetSettings
        let container = new ApiKeysContainer(settings)

        if retail 
        then new PublisherForRetailPackages(container) :> IPublisher
        else new PublisherForEdgePackages(container) :> IPublisher
    