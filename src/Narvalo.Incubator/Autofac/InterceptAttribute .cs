using Autofac.Core;

namespace Narvalo.Autofac
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Castle.DynamicProxy;

    [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class InterceptAttribute : Attribute
    {
        readonly Service _interceptorService;

        public InterceptAttribute(Service interceptorService)
        {
            Requires.NotNull(interceptorService, "interceptorService");

            _interceptorService = interceptorService;
        }

        public InterceptAttribute(string interceptorServiceName)
            : this(new KeyedService(interceptorServiceName, typeof(IInterceptor)))
        {
        }

        public InterceptAttribute(Type interceptorServiceType)
            : this(new TypedService(interceptorServiceType))
        {
        }

        public Service InterceptorService
        {
            get
            {
                return _interceptorService;
            }
        }
    }
}
