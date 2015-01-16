using System;
using System.Diagnostics;

namespace Unity.log4netInterception.Tests
{
    public class CustomLogCar : ICar
    {
        [CustomLog]
        public bool Drive(int speed)
        {
            Debug.WriteLine("Im Driving!");
            return true;
        }

        [CustomLog]
        public void Throw()
        {
            throw new NotImplementedException();
        }

        public void DontLogMe()
        {
        }
    }
}