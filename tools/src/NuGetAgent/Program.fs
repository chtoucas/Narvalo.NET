// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace NuGetAgent

open Nessos.UnionArgParser

[<RequireQualifiedAccess>]
module Program =

    [<RequireQualifiedAccess>]
    module Args =

        /// Parser template for the command-line arguments.
        type private ParserTemplate = 
            | [<Mandatory; AltCommandLine("-p")>] Path of string
            | [<AltCommandLine("-r")>] Retail
            interface IArgParserTemplate with
                member s.Usage = 
                    match s with
                    | Path _   -> "Specify the path to the directory where packages to be published are located."
                    | Retail _ -> "If present, packages are for retail."

        /// Parse command-line arguments.
        let parse args = 
            let parser = UnionArgParser.Create<ParserTemplate>()
            let result = parser.ParseCommandLine(args, new ProcessExiter())

            let path   = result.GetResult <@ Path @>
            let retail = result.Contains <@ Retail @>

            (path, retail)

    /// Entry point for the application.
    [<EntryPoint>]
    let main args = 
        let path, retail = Args.parse args

        Publishers.Processor.Create(retail).PublishPackagesFrom path

        0
