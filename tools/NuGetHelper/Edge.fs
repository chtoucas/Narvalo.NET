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
open System.Xml.Linq

open NuGet

let publishPackagesFrom path =
    Console.WriteLine (printfn "EDGE: %s" path)

//    let packageID = "Narvalo.Core.EDGE"
//
//    let repository = PackageRepositoryFactory.Default.CreateRepository Constants.MyGetSource
//
//    let packages = repository.FindPackagesById packageID
//
//    for package in packages do
//        printfn "%s" package.Id

