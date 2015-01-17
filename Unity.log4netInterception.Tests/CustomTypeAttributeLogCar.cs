using System;
using System.Diagnostics;

namespace Unity.log4netInterception.Tests
{
    [CustomLog]
    public class CustomTypeAttributeLogCar : ICar
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

        public void DoSomethingWithReferenceType(string s)
        {
           
        }

        public void DontLogMe()
        {
        }
    }
}