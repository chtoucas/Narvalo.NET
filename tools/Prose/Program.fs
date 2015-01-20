// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Prose

open Nessos.UnionArgParser

[<RequireQualifiedAccess>]
module Program =
    [<RequireQualifiedAccess>]
    module Args =
        /// Parser template for the command-line arguments.
        type private ParserTemplate = 
            | [<Mandatory; AltCommandLine("-p")>] Path of string
            interface IArgParserTemplate with
                member s.Usage = 
                    match s with
                    | Path _   -> "Specify the path to the directory where sources are located."

        /// Parse command-line arguments.
        let parse args = 
            let parser = UnionArgParser.Create<ParserTemplate>()
            let result = parser.ParseCommandLine(args, new ProcessExiter())

            let path = result.GetResult <@ Path @>

            (path)

    [<EntryPoint>]
    let main args = 
        let path = Args.parse args

        0
