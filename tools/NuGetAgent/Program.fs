// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

module Program

open Nessos.UnionArgParser

/// Template for the expected CLI arguments.
type Arguments = 
    | [<Mandatory; AltCommandLine("-p")>] Path of string
    | [<AltCommandLine("-r")>] Retail
    interface IArgParserTemplate with
        member s.Usage = 
            match s with
            | Path _ -> "Specify the path to the directory where packages to be published are located."
            | Retail _    -> "If present, packages are for retail."

/// Parse command-line arguments.
let parseArguments args = 
    let parser = UnionArgParser.Create<Arguments>()
    let result = parser.ParseCommandLine(args, new ProcessExiter())

    let retail = result.Contains <@ Retail @>
    let path = result.GetResult <@ Path @>

    (retail, path)

[<EntryPoint>]
let main args = 
    let retail, path = parseArguments args

    match retail with
    | true -> printfn "Publishing packages to the official NuGet server."
    | false -> printfn "Publishing packages to our own private NuGet server."

    let facade = 
        match retail with
        | true -> new NuGetFacade(ApiKeysContainer.Load) :> INuGetFacade
        | false -> new MyGetFacade(ApiKeysContainer.Load) :> INuGetFacade
    
    let packages = facade.GetPackagesToBePublished path
    
    for package in packages do facade.PublishPackage package

    0
