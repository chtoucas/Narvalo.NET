// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
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

        // REVIEW: Prefers composition over extension?
        public virtual Type Resolve(Type viewType)
        {
            Require.NotNull(viewType, "viewType");

            Tracer.Info(this, @"Attempting to resolve ""{0}"".", viewType.FullName);

            var candidatePrefixes = GetCandidatePrefixesFromInterfaces(viewType)
                .Append(GetCandidatePrefixFromViewName(viewType.Name));

            // We also look into the view namespace 
            // and into the assembly where the view is defined.
            var nameSpaces = _defaultNamespaces
                .Append(viewType.Namespace)
                .Append(new AssemblyName(viewType.Assembly.FullName).Name);

            var presenterTypes
                = from candidatePrefix in candidatePrefixes.Distinct()
                  from template in
                      (from nameSpace in nameSpaces.Distinct()
                       from template in _presenterNameTemplates
                       select template.Replace("{namespace}", nameSpace))
                  let typeName = template.Replace("{presenter}", candidatePrefix + "Presenter")
                  let presenterType = _buildManager.GetType(
                      typeName,
                      throwOnError: false,
                      ignoreCase: true)
                  where presenterType != null && typeof(Narvalo.Mvp.IPresenter).IsAssignableFrom(presenterType)
                  select presenterType;

            return presenterTypes.FirstOrDefault();
        }

        [SuppressMessage("Microsoft.Design",
            "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "False positive, MemberInfo does not work.")]
        internal static IEnumerable<string> GetCandidatePrefixesFromInterfaces(Type viewType)
        {
            DebugCheck.NotNull(viewType);

            // Only keep interfaces inheriting IView, except IView and IView<T>, whose
            // name ends with "View".
            // If the interface is generic, remove everything after ` in the interface name.
            // Trim "I" from the start of the interface name.
            // Trim "View" from the end of the interface name.
            // NB: We can not use _ != typeof(IView<>) to filter IView<T>
            return
                from _ in viewType.GetInterfaces()
                where _.Name != "IView"
                    && _.Name != "IView`1"
                    && typeof(IView).IsAssignableFrom(_)
                let name = _.IsGenericType
                    ? _.Name.Substring(0, _.Name.LastIndexOf("`", StringComparison.OrdinalIgnoreCase))
                    : _.Name
                where name.EndsWith("View", StringComparison.OrdinalIgnoreCase)
                select name.Substring(0, name.Length - 4).TrimStart('I');
        }

        internal string GetCandidatePrefixFromViewName(string viewName)
        {
            var name = (from suffix in _viewSuffixes
                        where viewName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase)
                        select viewName.Substring(0, viewName.Length - suffix.Length)).FirstOrDefault();

            return String.IsNullOrEmpty(name) ? viewName : name;
        }
    }
}
