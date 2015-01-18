// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

(*
- Find packages to be published
- Find current public version of packages
- Remove packages already public
- Check the dependency tree
- Publish packages in the reverse order of the dependency tree
*)

[<RequireQualifiedAccess>]
module Retail

open System
open System.Xml.Linq

open NuGet

let publishPackagesFrom path =
    Console.WriteLine (printfn "Publishing RETAIL packages from %s." path)

