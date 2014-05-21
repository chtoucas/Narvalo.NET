// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Internal;

    public class /*Default*/PresenterTypeResolver : IPresenterTypeResolver
    {
        readonly IBuildManager _buildManager;
        readonly IEnumerable<string> _defaultNamespaces;
        readonly IEnumerable<string> _presenterNameTemplates;
        readonly IEnumerable<string> _viewSuffixes;

        public PresenterTypeResolver(
            IBuildManager buildManager,
            IEnumerable<string> defaultNamespaces,
            IEnumerable<string> viewSuffixes,
            IEnumerable<string> presenterNameTemplates)
        {
            Require.NotNull(buildManager, "buildManager");
            Require.NotNull(defaultNamespaces, "defaultNamespaces");
            Require.NotNull(viewSuffixes, "viewSuffixes");
            Require.NotNull(presenterNameTemplates, "presenterNameTemplates");

            _buildManager = buildManager;
            _defaultNamespaces = defaultNamespaces;
            _viewSuffixes = viewSuffixes;
            _presenterNameTemplates = presenterNameTemplates;
        }

        public virtual Type Resolve(Type input)
        {
            Require.NotNull(input, "input");

            __Tracer.Info(this, @"Attempting to resolve ""{0}"".", input.FullName);

            var shortNames = GetShortNamesFromInterfaces_(input)
                .Append(GetShortNameFromType_(input));

            // We also look into the view namespace 
            // and into the assembly where the view is defined.
            var nameSpaces = _defaultNamespaces
                .Append(input.Namespace)
                .Append(new AssemblyName(input.Assembly.FullName).Name);

            var presenterTypes
                = from shortName in shortNames.Distinct()
                  from template in
                      (from nameSpace in nameSpaces.Distinct()
                       from template in _presenterNameTemplates
                       select template.Replace("{namespace}", nameSpace))
                  let typeName = template.Replace("{presenter}", shortName + "Presenter")
                  let presenterType = _buildManager.GetType(
                      typeName,
                      throwOnError: false,
                      ignoreCase: true)
                  where presenterType != null && typeof(Narvalo.Mvp.IPresenter).IsAssignableFrom(presenterType)
                  select presenterType;

            return presenterTypes.FirstOrDefault();
        }

        static IEnumerable<string> GetShortNamesFromInterfaces_(Type viewType)
        {
            // Trim the "I" and "View" from the start & end respectively of the interface names.
            return
                from _ in viewType.GetInterfaces()
                where typeof(IView).IsAssignableFrom(_)
                    && _ != typeof(IView)
                    && _ != typeof(IView<>)
                let name = _.Name.TrimStart('I')
                let length = name.LastIndexOf("View", StringComparison.OrdinalIgnoreCase)
                select length > 0 ? name.Substring(0, length) : name;
        }

        string GetShortNameFromType_(Type viewType)
        {
            var viewName = viewType.Name;

            var name = (from suffix in _viewSuffixes
                        where viewName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase)
                        select viewName.Substring(0, viewName.Length - suffix.Length)).FirstOrDefault();

            return String.IsNullOrEmpty(name) ? viewName : name;
        }
    }
}
