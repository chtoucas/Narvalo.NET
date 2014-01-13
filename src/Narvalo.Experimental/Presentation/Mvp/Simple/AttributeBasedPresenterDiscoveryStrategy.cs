namespace Narvalo.Presentation.Mvp.Simple
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Collections;

    /// <summary>
    /// A strategy for discovery presenters based on [PresenterBinding] attributes being placed
    /// on views and view hosts.
    /// </summary>
    public class AttributeBasedPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        static readonly
            IDictionary<RuntimeTypeHandle, IEnumerable<PresenterBindingAttribute>>
            TypeToAttributeCache_
            = new Dictionary<RuntimeTypeHandle, IEnumerable<PresenterBindingAttribute>>();

        /// <summary>
        /// Gets the presenter bindings for passed views using the passed hosts.
        /// </summary>
        /// <param name="views">A list of view instances (user controls, pages, etc).</param>
        public IEnumerable<PresenterDiscoveryResult> GetBindings(IEnumerable<IView> views)
        {
            Requires.NotNull(views, "views");

            var pendingViews = views.ToList();

            var passes = 0;
            var allowedPasses = 10 * pendingViews.Count();

            while (pendingViews.Any()) {
                var bindings = new List<PresenterBinding>();
                var view = pendingViews.First();

                var viewType = view.GetType();

                var viewAttributes = GetAttributes(viewType);

                if (!viewAttributes.Any()) {
                    // Could not find a [PresenterBinding] attribute on view instance {0}",
                }

                foreach (var attribute in viewAttributes.OrderBy(_ => _.PresenterType.Name)) {
                    if (!attribute.ViewType.IsAssignableFrom(viewType)) {
                        // found, but ignored, a [PresenterBinding] attribute on view instance
                        // because the view type on the attribute is not compatible with 
                        // the type of the view instance
                        continue;
                    }

                    // found a [PresenterBinding] attribute on view instance

                    bindings.Add(new PresenterBinding(
                        attribute.PresenterType,
                        attribute.ViewType,
                        view
                    ));
                }

                var totalViewsBound = bindings.Select(_ => _.View).Concat(new[] { view }).Distinct();

                yield return new PresenterDiscoveryResult(totalViewsBound, bindings);

                foreach (var viewToRemoveFromQueue in totalViewsBound) {
                    pendingViews.Remove(viewToRemoveFromQueue);
                }

                if (passes++ > allowedPasses)
                    throw new ApplicationException(
                        "The loop has executed too many times. An exit condition is failing and needs to be investigated.");
            }
        }

        internal static IEnumerable<PresenterBindingAttribute> GetAttributes(Type sourceType)
        {
            var hostTypeHandle = sourceType.TypeHandle;

            return TypeToAttributeCache_.GetOrCreateValue(hostTypeHandle, () => {
                return sourceType
                    .GetCustomAttributes(typeof(PresenterBindingAttribute), true /* inherit */)
                    .OfType<PresenterBindingAttribute>()
                    .Select(_ =>
                        new PresenterBindingAttribute(_.PresenterType) {
                            ViewType = _.ViewType ?? sourceType
                        })
                    .ToArray();
            });
        }
    }
}