using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.log4netInterception
{

    public class PointcutConfiguration
    {
        private readonly IEnumerable<IMatchingRule> _matchingRules;

        private PointcutConfiguration(params IMatchingRule[] rules)
        {
            _matchingRules = rules;
        }

        internal IEnumerable<IMatchingRule> MatchingRules
        {
            get { return _matchingRules; }
        }

        public static PointcutConfiguration UseDefaultAttributes()
        {
            var pointCut = new PointcutConfiguration(new ClassAttributeMatchingRule(typeof (LogThisAttribute), true));
            return pointCut.DontLogMethodsWhere(m => m.GetCustomAttribute<DontLogThisAttribute>() != null);
        }

        public static PointcutConfiguration LogEveything()
        {
            return new PointcutConfiguration(new AllTrueMathcingRule());
        }

        public PointcutConfiguration LogMethodsWhere(Func<MethodBase, bool> predicate)
        {
            return
                new PointcutConfiguration(
                    _matchingRules.Union(new[] {new DelegatedMethodMatchingRule(predicate)}).ToArray());
        }

        public PointcutConfiguration DontLogMethodsWhere(Func<MethodBase, bool> predicate)
        {
            return
                new PointcutConfiguration(
                    _matchingRules.Union(new[] {new DelegatedMethodMatchingRule(predicate, true)}).ToArray());
        }

        public PointcutConfiguration LogTypesWhere(Func<Type, bool> predicate)
        {
            return
                new PointcutConfiguration(
                    _matchingRules.Union(new[] {new DelegatedTypeMatchingRule(predicate)}).ToArray());
        }

        public PointcutConfiguration LogTypesWithAttribute(Type attributeType, bool inherit)
        {
            return
                new PointcutConfiguration(
                    _matchingRules.Union(new[] {new ClassAttributeMatchingRule(attributeType, inherit)}).ToArray());
        }

        public PointcutConfiguration LogMethodsWithAttribute(Type attributeType, bool inherit)
        {
            return
                new PointcutConfiguration(
                    _matchingRules.Union(new[] {new CustomAttributeMatchingRule(attributeType, inherit),}).ToArray());
        }
    }
}