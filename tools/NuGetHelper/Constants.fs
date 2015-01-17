// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

[<RequireQualifiedAccess>]
module Constants

/// Official NuGet source.
[<Literal>] 
let NuGetSource    = "https://www.nuget.org/api/v2/"

/// MyGet source for read-only NuGet commands like 'list' or 'update'.
[<Literal>] 
let MyGetSource    = "http://narvalo.org/myget/nuget/"

/// MyGet source for write NuGet commands like 'delete' or 'push'.
[<Literal>] 
let MyGetApiSource = "http://narvalo.org/myget/"
