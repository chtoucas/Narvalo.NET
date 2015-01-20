// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

#load @"load-project.fsx"

open NuGetAgent

Publishers.Publisher.Create(retail=true).PublishPackages
<| __SOURCE_DIRECTORY__ + @"\..\..\..\work\packages"
