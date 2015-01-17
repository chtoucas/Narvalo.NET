// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

[<RequireQualifiedAccess>]
module CLI

open Nessos.UnionArgParser

type Arguments = 
    | [<Mandatory; AltCommandLine("-d")>] Directory of string
    | [<AltCommandLine("-r")>] Retail
    interface IArgParserTemplate with
        member s.Usage = 
            match s with
            | Directory _ -> "Specify the directory where packages are stored."
            | Retail _    -> "If present, packages are for retail."

let parseArguments args = 
    let parser = UnionArgParser.Create<Arguments>()
    let result = parser.ParseCommandLine(args, new ProcessExiter())

    let retail = result.Contains <@ Retail @>
    let directory = result.GetResult <@ Directory @>

    (retail, directory)