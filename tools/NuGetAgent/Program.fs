// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

module Program

[<EntryPoint>]
let main args = 
    let retail, path = CLI.parseArguments args

    if retail 
    then Retail.publishPackagesFrom path
    else Edge.publishPackagesFrom path

    0
