// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

#load @"src\NuGetAgent\Scripts\load-project-release.fsx"

open NuGetAgent

Publishers.Processor.Create(retail=true).PublishPackagesFrom
<| __SOURCE_DIRECTORY__ ++ @"..\work\packages"
