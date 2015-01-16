# Unity.log4netInterception
Basic log4net Policy injection framework for Unity

Adds AOP style logging via Policy Injection (& Interface Interception) using Log4net.
Hardly configurable.
Youre stuck with Log4net.
Youre stuck with Unity.
Youre stuck with my shitty logging format.

This idea could be turned into something a bit nicer for fluently configuring Policy Injection/AOP types of stuff w/ Unity.

Maybe I will...

Works like this:

    [TestClass]
    public class DefaultAttributeLoggingTests : LoggingTestsBase<DefaultCar>
    {
        [TestInitialize]
        public void Init()
        {
            IUnityContainer container = new UnityContainer();
            container = container.Wireup(PointcutConfiguration.UseDefaultAttributes());
            
            
            container.RegisterTypeWithLogging<ICar, DefaultCar>();
        }

    }


    [LogThis]
    public class DefaultCar : ICar
    {
        public bool Drive(int speed)
        {
            Debug.WriteLine("Im Driving!");
            return true;
        }

        public void Throw()
        {
            throw new NotImplementedException();
        }

        [DontLogThis]
        public void DontLogMe()
        {
        }
    }
    
    How you choose what methods to log is completely up to you via a limited fluent API on the PointcutConfiguration object.
    
    Have it in ya.
