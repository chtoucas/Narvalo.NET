// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

[<RequireQualifiedAccess>]
module Constants

/// ApiKeys section name in the configuration.
[<Literal>]
let ApiKeysSectionName = "apikeys"

/// Suffix used by the edge packages.
[<Literal>]
let EdgePackageSuffix = ".EDGE"

/// MyGet's config key.
[<Literal>] 
let MyGetConfigKey = "http://narvalo.org/myget/"

/// MyGet repository source.
[<Literal>] 
let MyGetRepositorySource = "http://narvalo.org/myget/nuget/"

/// MyGet server source.
[<Literal>] 
let MyGetServerSource = "http://narvalo.org/myget/"

/// Official NuGet's config key.
[<Literal>] 
let NuGetConfigKey = "https://www.nuget.org"

/// Official NuGet source.
[<Literal>] 
let NuGetSource = "https://www.nuget.org/api/v2/"

/// User-Agent string.
[<Literal>] 
let UserAgent = "NuGet Agent - Narvalo.Org"
