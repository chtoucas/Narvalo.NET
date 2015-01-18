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

/// API Key
let private apiKey = "XXX"

/// Timeout in milliseconds.
let private timeout = 120000

let publishPackage (path: string) =
    let file = new FileInfo(path)
    let package = new OptimizedZipPackage(file.FullName)

    Console.WriteLine (printfn "Publishing package %s" package.Id)
    
    let packageServer = new PackageServer(Constants.MyGetApiSource, Constants.UserAgent)
    
    //packageServer.PushPackage(apiKey, package, file.Length, timeout, true)

    ()

let publishPackagesFrom (path: string) =
    Console.WriteLine (printfn "Publishing EDGE packages from %s." path)

    let files = Directory.GetFiles(path, "*.nupkg", SearchOption.TopDirectoryOnly)

    for file in files do publishPackage file

//    let packageId = "Narvalo.Core.EDGE"
//    let repository = PackageRepositoryFactory.Default.CreateRepository Constants.MyGetSource
//    let packages = repository.FindPackagesById packageId

