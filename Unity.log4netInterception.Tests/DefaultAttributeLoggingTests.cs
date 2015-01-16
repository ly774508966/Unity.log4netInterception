using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;

namespace Unity.log4netInterception.Tests
{
    [TestClass]
    public class DefaultAttributeLoggingTests : LoggingTestsBase<DefaultCar>
    {
        [TestInitialize]
        public void Init()
        {
            _container = new UnityContainer();
            _container = _container.Wireup(PointcutConfiguration.UseDefaultAttributes());
            _container.RegisterTypeWithLogging<ICar, DefaultCar>();
            _myAppender = new MemoryAppender();

            Logger root = ((Hierarchy) LogManager.GetRepository()).Root;
            root.RemoveAllAppenders();
            root.AddAppender(_myAppender);
            root.Repository.Configured = true;
        }
    }
}