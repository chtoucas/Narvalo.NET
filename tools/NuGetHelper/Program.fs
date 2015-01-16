// In Retail mode:
//   Use NuGetHelper to:
//   - Find packages to be published
//   - Find current public version of packages
//   - Remove packages already public
//   - Check the dependency tree
//   - Publish packages, in the order of the dependency tree
// In non-Retail mode:
//   Use NuGetHelper to:
//   - Find packages to be published
//   - Check the dependency tree
//   - For each package, in the order of the dependency tree
//   * Find previous versions
//   * Delete all previous versions but last
//   * Publish package

[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    0 // return an integer exit code
