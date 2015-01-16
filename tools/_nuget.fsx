
#I @"NuGetHelper\bin\Release"

#r "NuGet.Core.dll"
#r "NuGetHelper.dll"

open NuGetHelper
open NuGet

let packageID = "Narvalo.Core.EDGE"

let repository =
    NuGet.PackageRepositoryFactory.Default.CreateRepository
        "http://narvalo.org/myget/nuget" 

let packages = repository.FindPackagesById(packageID)

for p in packages do
    printfn "%s" p.Id

