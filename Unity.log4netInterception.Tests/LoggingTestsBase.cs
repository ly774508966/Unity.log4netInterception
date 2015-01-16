using System;
using System.Linq;
using System.Threading;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net.Appender;
using log4net.Core;

namespace Unity.log4netInterception.Tests
{
    public abstract class LoggingTestsBase<TCar> where TCar : ICar
    {
        protected IUnityContainer _container;
        protected MemoryAppender _myAppender;

        [TestMethod]
        public virtual void WritesLogOnMethodCall()
        {
            var car = _container.Resolve<ICar>();
            car.Drive(5);
            LoggingEvent[] events = _myAppender.GetEvents();
            Assert.IsNotNull(
                events.SingleOrDefault(
                    e =>
                    e.MessageObject.ToString() == String.Format("Calling {0}.Drive(Int32 speed = 5)", typeof (TCar))));
        }

        [TestMethod]
        public virtual void WritesLogOnMethodReturn()
        {
            var car = _container.Resolve<ICar>();
            car.Drive(5);

            Thread.Sleep(100); // Logging is Async

            LoggingEvent[] events = _myAppender.GetEvents();
            Assert.IsNotNull(
                events.FirstOrDefault(
                    e =>
                    e.MessageObject.ToString() ==
                    String.Format("Returning True from {0}.Drive(Int32 speed = 5)", typeof (TCar))));
        }


        [TestMethod]
        public virtual void DoesNotLogDontLogMethod()
        {
            var car = _container.Resolve<ICar>();
            car.DontLogMe();
            Thread.Sleep(100); // Logging is Async

            LoggingEvent[] events = _myAppender.GetEvents();
            Assert.IsFalse(events.Any());
        }

        [TestMethod]
        public virtual void WritesExceptionWhenExceptionThrown()
        {
            var car = _container.Resolve<ICar>();
            NotImplementedException ex = null;
            try
            {
                car.Throw();
            }
            catch (NotImplementedException e)
            {
                ex = e;
            }

            Thread.Sleep(100); // Logging is Async

            LoggingEvent[] events = _myAppender.GetEvents();
            Assert.IsNotNull(events.SingleOrDefault(e => e.ExceptionObject == ex));
        }
    }
}