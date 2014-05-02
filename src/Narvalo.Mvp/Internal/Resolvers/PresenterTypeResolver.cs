// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Narvalo.Collections;
    using Narvalo.Mvp;

    internal class PresenterTypeResolver : IComponentResolver<Type, Type>
    {
        readonly IList<string> _presenterNameTemplates;
        readonly IList<string> _viewSuffixes;

        public PresenterTypeResolver(
            IList<string> viewSuffixes,
            IList<string> presenterNameTemplates)
        {
            DebugCheck.NotNull(viewSuffixes);
            DebugCheck.NotNull(presenterNameTemplates);

            _viewSuffixes = viewSuffixes;
            _presenterNameTemplates = presenterNameTemplates;
        }

        public virtual Type Resolve(Type input)
        {
            DebugCheck.NotNull(input);

            var shortNames = GetNamesFromInterfaces_(input)
                .Append(GetNameFromType_(input));

            // TODO: We could allow to specify a custom namespace.
            var nameSpaces = new[] 
            { 
                input.Namespace,
                new AssemblyName(input.Assembly.FullName).Name
            };

            var names = from name in shortNames.Distinct()
                        from template in
                            (from nameSpace in nameSpaces.Distinct()
                             from template in _presenterNameTemplates
                             select template.Replace("{namespace}", nameSpace))
                        select template.Replace("{presenter}", name + "Presenter");

            foreach (var name in names) {
                var presenterType = input.Assembly.GetType(
                    name,
                    throwOnError: false,
                    ignoreCase: true);

                if (presenterType != null && typeof(IPresenter).IsAssignableFrom(presenterType)) {
                    return presenterType;
                }
            }

            return null;
        }

        static IEnumerable<string> GetNamesFromInterfaces_(Type viewType)
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

        string GetNameFromType_(Type viewType)
        {
            var viewName = viewType.Name;

            var name = (from suffix in _viewSuffixes
                        where viewName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase)
                        select viewName.Substring(0, viewName.Length - suffix.Length)).FirstOrDefault();

            return (String.IsNullOrEmpty(name) ? viewName : name);
        }
    }
}
