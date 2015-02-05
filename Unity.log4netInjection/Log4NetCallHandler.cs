using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.InterceptionExtension;
using log4net;

namespace Unity.log4netInterception
{
    internal class Log4NetCallHandler : ICallHandler
    {
        private readonly ILog _log = LogManager.GetLogger(typeof (Log4NetCallHandler));

        private Task _logTask;

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (_logTask != null)
            {
                _logTask.Wait();
            }

            _logTask = LogAsync(input.Target.GetType(), input.MethodBase, input.Arguments);

            IMethodReturn result = getNext().Invoke(input, getNext);

            _logTask.Wait();

            if (result.Exception != null)
            {
                _logTask = LogExceptionAsync(input.MethodBase, result.Exception);
            }
            else
            {
                _logTask = LogAsync(input.Target.GetType(), input.MethodBase, input.Arguments, true, result.ReturnValue);
            }

            return result;
        }

        public int Order { get; set; }


        private Task LogExceptionAsync(MethodBase methodBase, Exception exception)
        {
            return Task.Factory.StartNew(() =>
                                         _log.Error("Exception.", exception)
                );
        }

        private Task LogAsync(Type targetType, MethodBase methodBase, IParameterCollection parameterCollection,
                              bool isReturn = false, object retval = null)
        {
            return
                Task.Factory.StartNew(
                    () =>
                    _log.Debug(String.Format("{0} {1}.{2}({3})",
                                            isReturn ? String.Format("Returning {0} from", retval ?? "null") : "Calling",
                                            targetType, methodBase.Name, FormatArguments(parameterCollection)))
                    );
        }

        private string FormatArguments(IParameterCollection @params)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < @params.Count; i++)
            {
                var argVal = @params[i];
                var arg = @params.GetParameterInfo(i);
                string argName = arg.Name;
                string argType = arg.ParameterType.Name;

                sb.AppendFormat("{0} {1} = {2}", argType, argName, argVal == null ? "null" : argVal.ToString());
                if (i < @params.Count - 1)
                {
                    sb.Append(", ");
                }
            }

            return sb.ToString();
        }
    }
}