// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

#load @"Scripts\load-project.fsx"

let path = __SOURCE_DIRECTORY__ + @"\..\..\work\packages"

Edge.publishPackagesFrom path