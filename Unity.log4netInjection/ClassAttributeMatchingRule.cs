using System;
using System.Reflection;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.log4netInterception
{
    internal class ClassAttributeMatchingRule : IMatchingRule
    {
        private readonly Type _attributeType;
        private readonly bool _inherit;

        public ClassAttributeMatchingRule(Type attributeType, bool inherit)
        {
            _attributeType = attributeType;
            _inherit = inherit;
        }

        public bool Matches(MethodBase member)
        {
            return member.ReflectedType.GetCustomAttribute(_attributeType, _inherit) != null;
        }
    }
}