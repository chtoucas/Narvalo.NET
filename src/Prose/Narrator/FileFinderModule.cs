// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Prose.Narrator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Autofac;
    using Autofac.Core;
    using Narvalo;
    using Narvalo.IO;
    using Prose.IO;

    public sealed class FileFinderModule : Module
    {
        static readonly List<string> DirectoriesToIgnore_ = new List<string> { "bin", "obj", "_Aliens" };

        static readonly Func<DirectoryInfo, bool> DirectoryFilter_
            = _ => !DirectoriesToIgnore_.Any(s => _.Name.Equals(s, StringComparison.OrdinalIgnoreCase));

        static readonly Func<FileInfo, bool> FileFilter_
            = _ => !_.Name.EndsWith("Designer.cs", StringComparison.OrdinalIgnoreCase);

        protected override void Load(ContainerBuilder builder)
        {
            Require.NotNull(builder, "builder");

            builder.Register(_ => new FileFinder(DirectoryFilter_, FileFilter_))
                .AsSelf()
                .OnActivating(OnActivatingThunk_);

            builder.Register(_ => new ConcurrentFileFinder(DirectoryFilter_, FileFilter_))
                .AsSelf()
                .OnActivating(OnActivatingThunk_);
        }

        static EventHandler<RelativeDirectoryEventArgs> OnDirectoryStartThunk_(IOutputWriter writer)
        {
            return (sender, e) => writer.CreateDirectory(e.RelativeDirectory);
        }

        void OnActivatingThunk_(IActivatingEventArgs<ConcurrentFileFinder> handler)
        {
            var writer = handler.Context.Resolve<IOutputWriter>();

            handler.Instance.DirectoryStart += OnDirectoryStartThunk_(writer);
        }

        void OnActivatingThunk_(IActivatingEventArgs<FileFinder> handler)
        {
            var writer = handler.Context.Resolve<IOutputWriter>();

            handler.Instance.DirectoryStart += OnDirectoryStartThunk_(writer);
        }
    }
}
