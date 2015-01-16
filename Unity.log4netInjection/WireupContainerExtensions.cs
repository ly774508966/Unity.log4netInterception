using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.log4netInterception
{
    public static class WireupContainerExtensions
    {
        public static IUnityContainer Wireup(this IUnityContainer container, PointcutConfiguration config)
        {
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
    }
}