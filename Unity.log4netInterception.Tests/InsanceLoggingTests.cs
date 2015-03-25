using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.log4netInterception;
using Unity.log4netInterception.Tests;

namespace Unity.lo4netInterception.Tests
{
    [TestClass]
    public class InsanceLoggingTests : LoggingTestsBase<DefaultCar>
    {

        [TestInitialize]
        public void Init()
        {
            _container = new UnityContainer();
            _container = _container.Wireup(PointcutConfiguration.UseDefaultAttributes());
            var car = new DefaultCar();
            _container.RegisterInstanceWithLogging<ICar, DefaultCar>(car);
            _myAppender = new MemoryAppender();

            Logger root = ((Hierarchy)LogManager.GetRepository()).Root;
            root.RemoveAllAppenders();
            root.AddAppender(_myAppender);
            root.Repository.Configured = true;
        }
    }
}
