using System.Reflection;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.log4netInterception
{
    internal class AllTrueMathcingRule : IMatchingRule
    {
        public bool Matches(MethodBase member)
        {
            return true;
        }
    }
}