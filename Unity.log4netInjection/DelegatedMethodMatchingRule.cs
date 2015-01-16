using System;
using System.Reflection;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.log4netInterception
{
    internal class DelegatedMethodMatchingRule : IMatchingRule
    {
        private readonly Func<MethodBase, bool> _delegate;
        private readonly bool _negate;

        public DelegatedMethodMatchingRule(Func<MethodBase, bool> @delegate, bool negate = false)
        {
            _delegate = @delegate;
            _negate = negate;
        }

        public bool Matches(MethodBase member)
        {
            return _negate ? !_delegate(member) : _delegate(member);
        }
    }
}