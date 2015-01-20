// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace NuGetAgent

open System
open System.Collections.Generic
open System.Linq

open NuGet

[<RequireQualifiedAccess>]
module Publishers =
    /// Read-Only package repository, copied from NuGet.Common
    type private ReadOnlyPackageRepository(packages:IEnumerable<IPackage>) = 
        inherit PackageRepositoryBase()

        let _packages = packages

        override this.Source = null
        override this.SupportsPrereleasePackages = true
        override this.GetPackages() = _packages.AsQueryable()
         
    /// Find the list of the latest packages located at path, sorted by dependency order.
    let findLatestPackages path filter =
        let repository = new LocalPackageRepository(path, enableCaching=false)

        // Find packages, keeping only retail or edge packages.
        let packages = repository.GetPackages() |> Seq.filter(filter)

        // Collapse the list of packages, keeping only the latest version.
        let uniqPackages = packages.AsCollapsed()

        // Sort these packages by the dependency order.
        let sorter = new PackageSorter(targetFramework=null)
    
        sorter.GetPackagesByDependencyOrder(new ReadOnlyPackageRepository(uniqPackages))

    /// Publisher interface.
    type IPublisher =
        /// Publish a package.
        abstract member PublishPackage: package:IPackage -> unit

    /// Publisher to our own private NuGet server.
    type PublisherToMyGet(apiKeysContainer:ApiKeysContainer, ?purge) =
        /// API Key for the MyGet server.
        let _apiKey = apiKeysContainer.MyGetApiKey

        /// MyGet server.
        let _server = new PackageServer(Constants.MyGetServerSource, Constants.UserAgent)
        /// MyGet repository.
        let _repository = PackageRepositoryFactory.Default.CreateRepository(Constants.MyGetRepositorySource)
        /// Disable buffering?
        let _disableBuffering = true
        /// Timeout in milliseconds (1 minute).
        let _timeout = 60000
    
        interface IPublisher with
            member this.PublishPackage(package:IPackage) = 
                let obsoletePackages, newerPackages = 
                    _repository.FindPackagesById package.Id 
                    |> List.ofSeq
                    |> List.partition(fun p -> p.Version < package.Version)

                match purge with
                | None 
                | Some(true) ->
                    // Delete obsolete packages.
                    obsoletePackages
                    |> List.iter(fun p -> this._DeletePackage(p.Id, p.Version.ToString()))
                | Some(false) -> ()

                // Publish the package.
                if newerPackages.Any() 
                then printfn "Skipping... a more recent version already exists."
                else this._PushPackage(package)
          
        /// Delete a package from the remote server.
        member private this._DeletePackage(id, version) = 
            printfn "Deleting version %s" version

            _server.DeletePackage(_apiKey, id, version)
          
        /// Push a package to the remote server.
        member private this._PushPackage(package:IPackage) = 
            printfn "Publishing package..."

            _server.PushPackage(_apiKey, package, package.GetStream().Length, _timeout, _disableBuffering)
        
    /// Publisher to the official NuGet server.
    type PublisherToNuGet(apiKeysContainer:ApiKeysContainer) =
        /// API Key for the MyGet server.
        let _apiKey = apiKeysContainer.NuGetApiKey
       
        /// MyGet server.
        let _server = new PackageServer(Constants.NuGetSource, Constants.UserAgent)
        /// MyGet repository.
        let _repository = PackageRepositoryFactory.Default.CreateRepository(Constants.NuGetSource)
        /// Disable buffering?
        let _disableBuffering = true
        /// Timeout in milliseconds (1 minute).
        let _timeout = 60000

        interface IPublisher with
            member this.PublishPackage(package:IPackage) = 
                let remotePackage = _repository.FindPackage(package.Id, package.Version)

                if remotePackage <> null
                then printfn "Skipping... the package already exists on the server."
                else this._PushPackage(package)
          
        /// Push a package to the remote server.
        member private this._PushPackage(package:IPackage) = 
            printfn "Pushing package..."

            _server.PushPackage(_apiKey, package, package.GetStream().Length, _timeout, _disableBuffering)

    let private publishPackages (publisher:IPublisher) packages =
        if Seq.isEmpty packages 
        then printfn "No packages found for publication."
        else
            packages 
            |> Seq.iter(fun (p:IPackage) -> 
                printfn "> Processing package %s v%O" p.Id p.Version
                publisher.PublishPackage p)
    
    type Publisher = 
        | Retail of bool * IPublisher 
        | Edge   of IPublisher
    with 
        static member Create(retail, ?official) = 
            let settings = loadNuGetSettings
            let container = new ApiKeysContainer(settings)

            if retail then
                // For retail packages, the default behaviour is to publish them
                // to the official NuGet server.
                match official with
                | None
                | Some(true)  -> Retail(true, new PublisherToNuGet(container))
                | Some(false) -> Retail(false, new PublisherToMyGet(container))
            else
                // For edge packages, we only allow publication to our own private NuGet server.
                match official with
                | None
                | Some(false) -> Edge(new PublisherToMyGet(container))
                | Some(true)  -> failwith "You CAN NOT publish edge packages to the official NuGet server."

        member this.PublishPackagesFrom path =
            match this with

            | Retail(official, inner) -> 
                printfn <| if official then "Publishing retail packages to the official NuGet server."
                           else "Publishing retail packages to our own private NuGet server."

                findLatestPackages path (fun p -> not(p.Id.EndsWith(Constants.EdgePackageSuffix)))
                |> (publishPackages <| inner)

            | Edge(inner) -> 
                printfn "Publishing edge packages to our own private NuGet server."

                findLatestPackages path (fun p -> p.Id.EndsWith(Constants.EdgePackageSuffix))
                |> (publishPackages <| inner)
      
