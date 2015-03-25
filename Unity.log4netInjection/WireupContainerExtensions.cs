using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.log4netInterception
{
    public static class WireupContainerExtensions
    {
        public static IUnityContainer Wireup(this IUnityContainer container, PointcutConfiguration config)
        {
            if (config == null)
            {
                config = PointcutConfiguration.UseDefaultAttributes();
            }

            container.RegisterInstance(config);

            container = container.AddNewExtension<Interception>();

            PolicyDefinition policyDef = container.Configure<Interception>()
                                                  .AddPolicy("logging");

            foreach (IMatchingRule rule in config.MatchingRules)
            {
                policyDef = policyDef.AddMatchingRule(rule);
            }

            policyDef.AddCallHandler<Log4NetCallHandler>(new ContainerControlledLifetimeManager(),
                                                         new InjectionConstructor());

            container.RegisterType<Log4NetCallHandler>(new PerResolveLifetimeManager());
            container.RegisterType<ICallHandler, Log4NetCallHandler>("LoggingCallHandler", new PerResolveLifetimeManager());
            return container;
        }

        public static IUnityContainer RegisterTypeWithLogging<TInterface, TType>(this IUnityContainer container)
            where TType : TInterface
        {
            container.RegisterType<TInterface, TType>(
                new InterceptionBehavior<PolicyInjectionBehavior>(),
                new Interceptor<InterfaceInterceptor>());

            return container;
        }

        public static IUnityContainer RegisterTypeWithLogging<TInterface, TType, TInterceptor>(this IUnityContainer container)
            where TType : TInterface 
            where TInterceptor : IInterceptor
        {
            container.RegisterType<TInterface, TType>(
                new InterceptionBehavior<PolicyInjectionBehavior>(),
                new Interceptor<TInterceptor>());

            return container;
        }

        public static IUnityContainer RegisterTypeWithLogging<TInterface, TType>(this IUnityContainer container, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers )
            where TType : TInterface
        {
            injectionMembers =
                injectionMembers.Union(new InjectionMember[]  {new InterceptionBehavior<PolicyInjectionBehavior>(), new Interceptor<InterfaceInterceptor>()})
                                .ToArray();
            container.RegisterType<TInterface, TType>(lifetimeManager, injectionMembers);

            return container;
        }

        public static IUnityContainer RegisterTypeWithLogging<TInterface, TType, TInterceptor>(this IUnityContainer container, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
            where TType : TInterface
            where TInterceptor : IInterceptor
        {
            injectionMembers =
                injectionMembers.Union(new InjectionMember[] { new InterceptionBehavior<PolicyInjectionBehavior>(), new Interceptor<TInterceptor>() })
                                .ToArray();
            container.RegisterType<TInterface, TType>(lifetimeManager, injectionMembers);

            return container;
        }

        public static IUnityContainer RegisterTypeWithLoggingAndInterceptor<TType, TInterceptor>(this IUnityContainer container)
            where TInterceptor : IInterceptor
        {
            container.RegisterType<TType>(
                new InterceptionBehavior<PolicyInjectionBehavior>(),
                new Interceptor<TInterceptor>());

            return container;
        }

        public static IUnityContainer RegisterTypeWithLoggingAndInterceptor<TType, TInterceptor>(this IUnityContainer container, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
            where TInterceptor : IInterceptor
        {
            injectionMembers =
                injectionMembers.Union(new InjectionMember[] { new InterceptionBehavior<PolicyInjectionBehavior>(), new Interceptor<TInterceptor>() })
                                .ToArray();
            container.RegisterType<TType>(lifetimeManager, injectionMembers);

            return container;
        }

        /// <summary>
        /// Creates a wrapper proxy arround the supplied instance and intercepts calls for logging.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TType"></typeparam>
        /// <param name="container"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static IUnityContainer WrapAndRegisterInstanceWithLogging<TInterface, TType>(this IUnityContainer container,
            TType instance)
            where TType : class, TInterface
            where TInterface : class
        {
            var pointcutConfig = container.Resolve<PointcutConfiguration>();
            if (pointcutConfig == null)
            {
                throw new Exception(
                    "WireUp() must be called on this unity container before registering types for logging");
            }

            var interceptor = new InterfaceInterceptor();

            var request = new CurrentInterceptionRequest(interceptor, typeof(TInterface), typeof(TType));
            
            var policies = new InjectionPolicy[] { new RuleDrivenPolicy(pointcutConfig.MatchingRules.ToArray(), new string[] { "LoggingCallHandler" }) };

            var behaviour = new PolicyInjectionBehavior(request, policies, container);

            var instanceToRegister = Intercept.ThroughProxy(typeof(TInterface), instance, interceptor, new[] {behaviour});

            container.RegisterInstance((TInterface)instanceToRegister);

            return container;
        }

    }
}