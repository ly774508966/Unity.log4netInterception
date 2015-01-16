using System.Linq;
using System.Threading;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace Unity.log4netInterception.Tests
{
    [TestClass]
    public class CustomTypeAttributeLoggingTests : LoggingTestsBase<CustomTypeAttributeLogCar>
    {
        [TestInitialize]
        public void Init()
        {
            _container = new UnityContainer();
            _container =
                _container.Wireup(PointcutConfiguration.LogEveything()
                                                       .LogTypesWithAttribute(typeof (CustomLogAttribute), true));
            _container.RegisterTypeWithLogging<ICar, CustomTypeAttributeLogCar>();
            _myAppender = new MemoryAppender();

            Logger root = ((Hierarchy) LogManager.GetRepository()).Root;
            root.RemoveAllAppenders();
            root.AddAppender(_myAppender);
            root.Repository.Configured = true;
        }

        [TestMethod]
        public override void DoesNotLogDontLogMethod()
        {
            var car = _container.Resolve<ICar>();
            car.DontLogMe();
            Thread.Sleep(100); // Logging is Async

            LoggingEvent[] events = _myAppender.GetEvents();
            Assert.IsTrue(events.Any());
        }
    }
}