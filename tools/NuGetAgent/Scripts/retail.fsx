// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

#load @"load-project.fsx"

open NuGetAgent

printfn "Publishing packages to the official NuGet server."
    
let publisher = Publisher.create true
let packages = publisher.FindPackagesToPublish <| __SOURCE_DIRECTORY__ + @"\..\..\..\work\packages"
    
for package in packages do publisher.PublishPackage package