#r @"packages\FAKE\tools\FakeLib.dll"

open Fake

//RestorePackages()

let projectName = "Project Name"
let projectDescription = "Project Description"
let projectSummary = "Project Summary"

let buildDir = "./work/staging/"

// Targets

Target "Default" DoNothing

Target "Clean" (fun _ ->
    trace "Clean task for FAKE"
)

//Target "Build" (fun _ ->
//    !! "src/**/*.csproj"
//        |> MSBuildRelease buildDir "Build"
//)

Target "Test" (fun _ ->
    trace "Test task for FAKE"
)

// Dependency Graph.

"Clean"
    ==> "Default"


RunTargetOrDefault "Default"
