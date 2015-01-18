// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

(*
- Find packages to be published
- Check the dependency tree
- For each package, in the reverse order of the dependency tree,
    * Find previous versions
    * Delete all previous versions but last
    * Publish package
*)

[<RequireQualifiedAccess>]
module Edge

open System
open System.IO
open System.Xml.Linq

open NuGet

/// Timeout in milliseconds.
[<Literal>] 
let private timeout = 120000

let publishPackage path apiKey =
    let file = new FileInfo(path)
    let package = new OptimizedZipPackage(file.FullName)

    Console.WriteLine (printfn "Publishing package %s at version %s" package.Id (package.Version.ToString()))

    let repository = PackageRepositoryFactory.Default.CreateRepository Constants.MyGetSource
    let oldPackages = repository.FindPackagesById package.Id

    let server = new PackageServer(Constants.MyGetApiSource, Constants.UserAgent)

    for oldPackage in oldPackages do 
        printfn "Deleting package %s" (oldPackage.Version.ToString())
        //server.DeletePackage(apiKey, oldPackage.Id, oldPackage.Version.ToString())
    
    //server.PushPackage(apiKey, package, file.Length, timeout, true)

    ()

let publishPackagesFrom packagesDir =
    Console.WriteLine (printfn "Publishing EDGE packages from %s." packagesDir)

    let apiKey = "XXX"

    let files = Directory.GetFiles(packagesDir, "*.nupkg", SearchOption.TopDirectoryOnly)

    for file in files do publishPackage file apiKey

