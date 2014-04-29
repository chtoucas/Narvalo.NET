// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Narvalo;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Internal;

    public sealed class ConventionBasedPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        static readonly IEnumerable<string> ViewInstanceSuffixes_ = new[] 
        {
            "UserControl",
            "Control",
            "View",
            "Form",
        };

        static readonly IEnumerable<string> CandidatePresenterNames_ = new[]
        {
            "{namespace}.Presenters.{presenter}",
            "{namespace}.{presenter}",
        };

        readonly ReflectionCache<Type> _presenterTypeCache
           = new ReflectionCache<Type>();

        readonly ReflectionCache<IEnumerable<Type>> _viewInterfacesCache
           = new ReflectionCache<IEnumerable<Type>>();

        public IEnumerable<PresenterDiscoveryResult> FindBindings(
            IEnumerable<object> hosts,
            IEnumerable<IView> views)
        {
            Require.NotNull(views, "views");

            // REVIEW: hosts is ignored.
            return views.Select(FindBinding_).ToArray();
        }

        static string GetCandidateNameFromViewType_(Type viewType)
        {
            // Check for existance of supported suffixes and if found, remove and use result
            // as basis for presenter type name
            // e.g. HelloWorldControl => HelloWorldPresenter
            //      WidgetsWebService => WidgetsPresenter
            var presenterTypeName
                = (from suffix in ViewInstanceSuffixes_
                   where viewType.Name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase)
                   select TrimEnd_(viewType.Name, suffix)).FirstOrDefault();

            return (String.IsNullOrEmpty(presenterTypeName) ? viewType.Name : presenterTypeName) + "Presenter";
        }

        static IEnumerable<string> GenerateCandidateFullNames_(
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

                foreach (var typeNameFormat in CandidatePresenterNames_) {
                    yield return typeNameFormat.Replace("{namespace}", assemblyName)
                                               .Replace("{presenter}", presenterTypeName);
                }
            }
        }

        static string TrimEnd_(string source, string suffix)
        {
            if (String.IsNullOrEmpty(source) || String.IsNullOrEmpty(suffix)) {
                return source;
            }

            var length = source.LastIndexOf(suffix, StringComparison.OrdinalIgnoreCase);

            return length > 0 ? source.Substring(0, length) : source;
        }

        PresenterDiscoveryResult FindBinding_(IView view)
        {
            var viewType = view.GetType();

            var presenterType = _presenterTypeCache.GetOrAdd(viewType, CreatePresenterType_);

            return new PresenterDiscoveryResult(
                new[] { view },
                presenterType == null
                    ? new PresenterBinding[0]
                    : new[] { 
                        new PresenterBinding(
                            presenterType,
                            viewType, 
                            PresenterBindingMode.Default, 
                            new[] { view }) }
            );
        }

        Type CreatePresenterType_(Type viewType)
        {
            var candidateNames = GetCandidateFullNames_(viewType);

            foreach (var name in candidateNames.Distinct()) {
                var presenterType = viewType.Assembly.GetType(
                    name,
                    false /* throwOnError */,
                    true /* ignoreCase */);

                if (presenterType != null && typeof(IPresenter).IsAssignableFrom(presenterType)) {
                    return presenterType;
                }
            }

            return null;
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
            var viewInterfaces = _viewInterfacesCache.GetOrAdd(
                viewType,
                _ => _.GetInterfaces().Where(typeof(IView).IsAssignableFrom).ToArray());

            // Trim the "I" and "View" from the start & end respectively of the interface names.
            return from i in viewInterfaces
                   where i.Name != "IView" && i.Name != "IView`1"
                   select TrimEnd_(i.Name.TrimStart('I'), "View");
        }
    }
}
