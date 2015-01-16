using System;
using System.Diagnostics;

namespace Unity.log4netInterception.Tests
{
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
}