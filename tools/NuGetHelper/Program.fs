// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

// In Retail mode:
//   Use NuGetHelper to:
//   - Find packages to be published
//   - Find current public version of packages
//   - Remove packages already public
//   - Check the dependency tree
//   - Publish packages, in the order of the dependency tree
// In non-Retail mode:
//   Use NuGetHelper to:
//   - Find packages to be published
//   - Check the dependency tree
//   - For each package, in the order of the dependency tree
//   * Find previous versions
//   * Delete all previous versions but last
//   * Publish package

module Program

open System.Xml.Linq

open NuGet

[<EntryPoint>]
let main args = 
    let (retail, directory) = CLI.parseArguments (args)

    let packageID = "Narvalo.Core.EDGE"

    let repository =
        NuGet.PackageRepositoryFactory.Default.CreateRepository
            "http://narvalo.org/myget/nuget" 

    let packages = repository.FindPackagesById(packageID)

    for package in packages do
        printfn "%s" package.Id

    0
