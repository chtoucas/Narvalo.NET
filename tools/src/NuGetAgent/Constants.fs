// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

[<RequireQualifiedAccess>]
module Constants

/// ApiKeys section name in the configuration.
let [<Literal>] ApiKeysSectionName = "apikeys"

/// Suffix used by the edge packages.
let [<Literal>] EdgePackageSuffix = ".EDGE"

/// MyGet's config key.
let [<Literal>] MyGetConfigKey = "http://narvalo.org/myget/"

/// MyGet repository source.
let [<Literal>] MyGetRepositorySource = "http://narvalo.org/myget/nuget/"

/// MyGet server source.
let [<Literal>] MyGetServerSource = "http://narvalo.org/myget/"

/// Official NuGet's config key.
let [<Literal>] NuGetConfigKey = "https://www.nuget.org"

/// Official NuGet repository source.
let [<Literal>] NuGetRepositorySource = "https://www.nuget.org/api/v2/"

/// Official NuGet server source.
let [<Literal>] NuGetServerSource = "https://www.nuget.org/"

/// User-Agent string.
let [<Literal>] UserAgent = "NuGet Agent - Narvalo.Org"
