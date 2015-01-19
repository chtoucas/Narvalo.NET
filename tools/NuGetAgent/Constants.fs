// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

[<RequireQualifiedAccess>]
module Constants

/// ApiKeys section name in the configuration.
[<Literal>]
let ApiKeysSectionName = "apikeys"

/// Suffix used by the edge packages.
[<Literal>]
let EdgePackageSuffix = ".EDGE"

/// MyGet source for write NuGet commands like 'delete' or 'push'.
[<Literal>] 
let MyGetApiSource = "http://narvalo.org/myget/"

/// MyGet source for read-only NuGet commands like 'list' or 'update'.
[<Literal>] 
let MyGetSource = "http://narvalo.org/myget/nuget/"

/// Official NuGet source.
[<Literal>] 
let NuGetSource = "https://www.nuget.org/api/v2/"

/// User-Agent string.
[<Literal>] 
let UserAgent = "NuGet Agent - Narvalo.Org"
