// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

[<AutoOpen>]
module Helpers

open System
open System.Collections.Generic
open System.IO

open NuGet

/// Machine-wide settings for NuGet.
type private MachineWideSettings(settings:IEnumerable<Settings>) =
    let _settings = settings

    interface IMachineWideSettings with
        member this.Settings
            with get() = _settings

/// Read the API Key from the settings.
let private readApiKey (settings:ISettings) (source:string) =
    let apiKey = settings.GetDecryptedValue(Constants.ApiKeysSectionName, source)

    if String.IsNullOrWhiteSpace(apiKey) 
    then failwithf "No API key could be found for the source: '%s'." source
    else apiKey

/// Load the NuGet settings.
let loadNuGetSettings =
    let baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
    let machineWideSettings = 
        new MachineWideSettings(
            NuGet.Settings.LoadMachineWideSettings(new PhysicalFileSystem(baseDirectory)))

    NuGet.Settings.LoadDefaultSettings(
        new PhysicalFileSystem(Directory.GetCurrentDirectory()), 
        configFileName=null, 
        machineWideSettings=machineWideSettings)

/// Container for the various NuGet API keys.
type ApiKeysContainer(settings:ISettings) =
    let _myGetApiKey = lazy( readApiKey settings Constants.MyGetApiSource )
    let _nuGetApiKey = lazy( readApiKey settings Constants.NuGetSource )

    member this.MyGetApiKey with get() = _myGetApiKey.Value
    member this.NuGetApiKey with get() = _nuGetApiKey.Value
        