// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

#load @"NuGetAgent\Scripts\load-project.fsx"

open NuGetAgent

Publishers.Processor.Create(retail=false).PublishPackagesFrom
<| __SOURCE_DIRECTORY__ ++ @"..\work\packages"
