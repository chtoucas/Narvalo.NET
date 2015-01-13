// Learn more about F# at http://fsharp.net. See the 'F# Tutorial' project
// for more guidance on F# programming.

// C:\Program Files (x86)\Microsoft SDKs\F#\3.1\Framework\v4.0\fsi.exe

#load "Library1.fs"

#r "NuGet.Core.dll"
#r "System.Xml.Linq.dll"

open NuGet.Automation
open NuGet

let packageID = "Narvalo.Core.EDGE"

let repository =
    NuGet.PackageRepositoryFactory.Default.CreateRepository
        "http://narvalo.org/myget/nuget" 

let packages = repository.FindPackagesById(packageID)

for p in packages do
    printfn "%s" p.Id

