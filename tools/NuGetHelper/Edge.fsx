// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

#I @"..\packages\Nuget.Core.2.8.3\lib\net40-Client"

#r "System.Xml.Linq.dll"
#r "NuGet.Core.dll"

#load "Constants.fs"
#load "Edge.fs"

Edge.publishPackagesFrom @"..\..\work\packages"