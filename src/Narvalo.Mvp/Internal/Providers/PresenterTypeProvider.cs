// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Narvalo.Mvp;

    internal class PresenterTypeProvider : IComponentProvider<Type, Type>
    {
        readonly IEnumerable<string> _candidatePresenterNames;
        readonly IEnumerable<string> _viewInstanceSuffixes;
        readonly ViewInterfacesProvider _viewInterfacesProvider;

        public PresenterTypeProvider(
            ViewInterfacesProvider viewInterfacesProvider,
            IEnumerable<string> viewInstanceSuffixes,
            IEnumerable<string> candidatePresenterNames)
        {
            _viewInterfacesProvider = viewInterfacesProvider;
            _viewInstanceSuffixes = viewInstanceSuffixes;
            _candidatePresenterNames = candidatePresenterNames;
        }

        public virtual Type GetComponent(Type input)
        {
            DebugCheck.NotNull(input);

            var candidateNames = GetCandidateFullNames_(input);

            foreach (var name in candidateNames.Distinct()) {
                var presenterType = input.Assembly.GetType(
                    name,
                    false /* throwOnError */,
                    true /* ignoreCase */);

                if (presenterType != null && typeof(IPresenter).IsAssignableFrom(presenterType)) {
                    return presenterType;
                }
            }

            return null;
        }

        static string TrimEnd_(string source, string suffix)
        {
            if (String.IsNullOrEmpty(source) || String.IsNullOrEmpty(suffix)) {
                return source;
            }

            var length = source.LastIndexOf(suffix, StringComparison.OrdinalIgnoreCase);

            return length > 0 ? source.Substring(0, length) : source;
        }

        string GetCandidateNameFromViewType_(Type viewType)
        {
            // Check for existance of supported suffixes and if found, remove and use result
            // as basis for presenter type name
            // e.g. HelloWorldControl => HelloWorldPresenter
            //      WidgetsWebService => WidgetsPresenter
            var presenterTypeName
                = (from suffix in _viewInstanceSuffixes
                   where viewType.Name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase)
                   select TrimEnd_(viewType.Name, suffix)).FirstOrDefault();

            return (String.IsNullOrEmpty(presenterTypeName) ? viewType.Name : presenterTypeName) + "Presenter";
        }

        IEnumerable<string> GenerateCandidateFullNames_(
           Type viewType,
           IEnumerable<string> presenterTypeNames)
        {
            // We assume the assembly name is the same as the namespace
            //var assemblyName = viewType.Assembly.GetNameSafe().Name;
            var assemblyName = new AssemblyName(viewType.Assembly.FullName).Name;

            foreach (var presenterTypeName in presenterTypeNames) {
                // Same location as view instance, 
                // e.g. MyApp.Web.Controls.HelloWorldControl => MyApp.Web.Controls.HelloWorldPresenter
                yield return viewType.Namespace + "." + presenterTypeName;

                foreach (var typeNameFormat in _candidatePresenterNames) {
                    yield return typeNameFormat.Replace("{namespace}", assemblyName)
                                               .Replace("{presenter}", presenterTypeName);
                }
            }
        }

        IEnumerable<string> GetCandidateFullNames_(Type viewType)
        {
            var candidateNames = new List<string> { 
                GetCandidateNameFromViewType_(viewType) 
            };

            // Get presenter type names from implemented IView interfaces
            candidateNames.AddRange(GetCandidateNamesFromViewInterfaces_(viewType));

            return GenerateCandidateFullNames_(viewType, candidateNames);
        }

        IEnumerable<string> GetCandidateNamesFromViewInterfaces_(Type viewType)
        {
            var viewInterfaces = _viewInterfacesProvider.GetComponent(viewType);

            // Trim the "I" and "View" from the start & end respectively of the interface names.
            return from i in viewInterfaces
                   where i.Name != "IView" && i.Name != "IView`1"
                   select TrimEnd_(i.Name.TrimStart('I'), "View");
        }
    }
}
