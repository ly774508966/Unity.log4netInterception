using System;

namespace Unity.log4netInterception
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DontLogThisAttribute : Attribute
    {
    }
}