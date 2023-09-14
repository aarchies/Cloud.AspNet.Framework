using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Glasssix.Contrib.Caller.Internal
{
    /// <summary>
    /// 调用者依赖编排
    /// </summary>
    internal static class CallerDependExtensions
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="callerTypes">继承CallerBase的所有调用者</param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public static List<Type> Arrangement(this List<Type> callerTypes)
        {
            List<Type> types = callerTypes.GetCallerByNotDependCaller();
            if (types.Count == 0)
                throw new Exception(ErrorMessages.CIRCULAR_DEPENDENCY);

            return callerTypes.CallersArrangement(types, 1);
        }

        private static List<Type> CallersArrangement(this List<Type> allTypes, List<Type> existTypes, int executeTimes)
        {
            List<Type> types = existTypes;
            var dependCallerTypes = allTypes.Except(existTypes);
            foreach (var type in dependCallerTypes)
            {
                var constructorInfo = type.GetConstructors().Max();
                //var constructorInfo = type.GetConstructors().MaxBy(con => con.GetParameters().Length)!;
                bool isExist = true;
                foreach (var parameterType in constructorInfo.GetParameters().Select(parameter => parameter.ParameterType))
                {
                    if (typeof(CallerBase).IsAssignableFrom(parameterType) && !types.Contains(parameterType))
                    {
                        isExist = false;
                    }
                }
                if (isExist)
                    types.Add(type);
            }

            if (types.Count != allTypes.Count)
            {
                if (executeTimes >= allTypes.Count)
                    throw new Exception(ErrorMessages.CIRCULAR_DEPENDENCY);

                return allTypes.CallersArrangement(types, executeTimes + 1);
            }
            return types;
        }

        /// <summary>
        /// 获取不依赖于其他调用者的调用者对象
        /// </summary>
        /// <param name="callerTypes"></param>
        /// <returns></returns>
        private static List<Type> GetCallerByNotDependCaller(this List<Type> callerTypes)
        {
            List<Type> types = new List<Type>();
            callerTypes.ForEach(type =>
            {
                if (!type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).IsDependCaller())
                    types.Add(type);
            });
            return types;
        }

        private static bool IsDependCaller(this ConstructorInfo[] constructorInfos)
        {
            var constructorInfo = constructorInfos.Max()!;
            return constructorInfo.IsDependCaller();
        }

        private static bool IsDependCaller(this ConstructorInfo constructorInfo)
            => constructorInfo.GetParameters().Any(parameter => typeof(CallerBase).IsAssignableFrom(parameter.ParameterType));
    }
}