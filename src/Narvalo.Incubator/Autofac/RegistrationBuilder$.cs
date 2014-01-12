using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Features.Scanning;
using Castle.DynamicProxy;

namespace Narvalo.Autofac
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public static class RegistrationBuilderExtensions
    {
        const string InterceptorsPropertyName 
            = "AutofacContrib.DynamicProxy2.RegistrationExtensions.InterceptorsPropertyName";

        static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();
        static readonly IEnumerable<Service> EmptyServices = new Service[0];

        /// <summary>
        /// Enable class interception on the target type. Interceptors will be determined
        /// via Intercept attributes on the class or added with InterceptedBy().
        /// Only virtual methods can be intercepted this way.
        /// </summary>
        /// <typeparam name="TLimit"></typeparam>
        /// <typeparam name="TRegistrationStyle"></typeparam>
        /// <param name="registration"></param>
        /// <returns></returns>
        public static IRegistrationBuilder<TLimit, ScanningActivatorData, TRegistrationStyle>
            EnableClassInterceptors<TLimit, TRegistrationStyle>(
                this IRegistrationBuilder<TLimit, ScanningActivatorData, TRegistrationStyle> registration)
        {
            Requires.NotNull(registration, "registration");

            registration.ActivatorData.ConfigurationActions.Add(
                (t, rb) => rb.EnableClassInterceptors());
            return registration;
        }

        /// <summary>
        /// Enable class interception on the target type. Interceptors will be determined
        /// via Intercept attributes on the class or added with InterceptedBy().
        /// Only virtual methods can be intercepted this way.
        /// </summary>
        /// <typeparam name="TLimit"></typeparam>
        /// <typeparam name="TRegistrationStyle"></typeparam>
        /// <typeparam name="TConcreteReflectionActivatorData"></typeparam>
        /// <param name="registration"></param>
        /// <returns></returns>
        public static IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle>
            EnableClassInterceptors<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle>(
                this IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> registration)
            where TConcreteReflectionActivatorData : ConcreteReflectionActivatorData
        {
            Requires.NotNull(registration, "registration");

            registration.ActivatorData.ImplementationType =
                ProxyGenerator.ProxyBuilder.CreateClassProxyType(
                    registration.ActivatorData.ImplementationType, new Type[0], ProxyGenerationOptions.Default);

            registration.OnPreparing(e => {
                e.Parameters = new Parameter[] {
                    new PositionalParameter(0, GetInterceptorServices_(e.Component, registration.ActivatorData.ImplementationType)
                        .Select(s => e.Context.ResolveService(s))
                        .Cast<IInterceptor>()
                        .ToArray())
                }.Concat(e.Parameters).ToArray();
            });

            return registration;
        }

        /// <summary>
        /// Enable interface interception on the target type. Interceptors will be determined
        /// via Intercept attributes on the class or interface, or added with InterceptedBy() calls.
        /// </summary>
        /// <typeparam name="TLimit"></typeparam>
        /// <typeparam name="TActivatorData"></typeparam>
        /// <typeparam name="TSingleRegistrationStyle"></typeparam>
        /// <param name="registration"></param>
        /// <returns></returns>
        public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle>
            EnableInterfaceInterceptors<TLimit, TActivatorData, TSingleRegistrationStyle>(
                this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> registration)
        {
            Requires.NotNull(registration, "registration");

            registration.RegistrationData.ActivatingHandlers.Add((sender, e) => {
                EnsureInterfaceInterceptionApplies_(e.Component);

                var proxiedInterfaces = e.Instance.GetType().GetInterfaces().Where(i => i.IsVisible).ToArray();

                if (!proxiedInterfaces.Any())
                    return;

                var theInterface = proxiedInterfaces.First();
                var interfaces = proxiedInterfaces.Skip(1).ToArray();

                var interceptors = GetInterceptorServices_(e.Component, e.Instance.GetType())
                    .Select(s => e.Context.ResolveService(s))
                    .Cast<IInterceptor>()
                    .ToArray();

                e.Instance = ProxyGenerator.CreateInterfaceProxyWithTarget(theInterface, interfaces, e.Instance, interceptors);
            });

            return registration;
        }

        static void EnsureInterfaceInterceptionApplies_(IComponentRegistration componentRegistration)
        {
            if (componentRegistration.Services
                .OfType<IServiceWithType>()
                .Where(swt => !swt.ServiceType.IsInterface)
                .Any())
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                    SR.InterfaceProxyingOnlySupportsInterfaceServices,
                    componentRegistration));
        }

        static IEnumerable<Service> GetInterceptorServices_(
            IComponentRegistration registration, 
            Type implType)
        {
            Requires.NotNull(registration, "registration");
            Requires.NotNull(implType, "implType");

            var result = EmptyServices;

            object services;
            if (registration.Metadata.TryGetValue(InterceptorsPropertyName, out services))
                result = result.Concat((IEnumerable<Service>)services);

            if (implType.IsClass) {
                result = result.Concat(implType
                    .GetCustomAttributes(typeof(InterceptAttribute), true)
                    .Cast<InterceptAttribute>()
                    .Select(att => att.InterceptorService));

                result = result.Concat(implType.GetInterfaces()
                    .SelectMany(i => i.GetCustomAttributes(typeof(InterceptAttribute), true))
                    .Cast<InterceptAttribute>()
                    .Select(att => att.InterceptorService));
            }

            return result.ToArray();
        }

        public static IRegistrationBuilder<TLimit, TActivatorData, TStyle>
            InterceptedBy<TLimit, TActivatorData, TStyle>(
                this IRegistrationBuilder<TLimit, TActivatorData, TStyle> builder,
                params Service[] interceptorServices)
        {
            Requires.NotNull(builder, "builder");
            Requires.NotNull(interceptorServices, "interceptorServices");

            if (interceptorServices.Any(s => s == null))
                throw new ArgumentNullException("interceptorServices");

            object existing;
            if (builder.RegistrationData.Metadata.TryGetValue(InterceptorsPropertyName, out existing))
                builder.RegistrationData.Metadata[InterceptorsPropertyName] =
                    ((IEnumerable<Service>)existing).Concat(interceptorServices).Distinct();
            else
                builder.RegistrationData.Metadata.Add(InterceptorsPropertyName, interceptorServices);

            return builder;
        }

        public static IRegistrationBuilder<TLimit, TActivatorData, TStyle>
            InterceptedBy<TLimit, TActivatorData, TStyle>(
                this IRegistrationBuilder<TLimit, TActivatorData, TStyle> builder,
                params string[] interceptorServiceNames)
        {
            Requires.NotNull(interceptorServiceNames, "interceptorServiceNames");

            if (interceptorServiceNames.Any(n => n == null))
                throw new ArgumentNullException("interceptorServiceNames");

            return InterceptedBy(builder, interceptorServiceNames.Select(n => new KeyedService(n, typeof(IInterceptor))).ToArray());
        }

        public static IRegistrationBuilder<TLimit, TActivatorData, TStyle>
            InterceptedBy<TLimit, TActivatorData, TStyle>(
                this IRegistrationBuilder<TLimit, TActivatorData, TStyle> builder,
                params Type[] interceptorServiceTypes)
        {
            Requires.NotNull(interceptorServiceTypes, "interceptorServiceTypes");

            if (interceptorServiceTypes.Any(t => t == null))
                throw new ArgumentNullException("interceptorServiceTypes");

            return InterceptedBy(builder, interceptorServiceTypes.Select(t => new TypedService(t)).ToArray());
        }
    }
}
