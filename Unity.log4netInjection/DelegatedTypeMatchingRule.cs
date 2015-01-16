using System;
using System.Reflection;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.log4netInterception
{
    internal class DelegatedTypeMatchingRule : IMatchingRule
    {
        private readonly Func<Type, bool> _delegate;

        public DelegatedTypeMatchingRule(Func<Type, bool> @delegate)
        {
            _delegate = @delegate;
        }

        public bool Matches(MethodBase member)
        {
            return _delegate(member.ReflectedType);
        }
    }
}