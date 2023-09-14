using System;
using System.Reflection;

namespace Glasssix.Utils.Convert
{

    public static class AttributeHelper
    {

        /// <summary>
        /// 获取成员变量对象中指定自定义特性名称的对象
        /// </summary>
        /// <param name="field">成员对象</param>
        /// <param name="attributeName">自定义特性名称</param>
        /// <returns>自定义特性对象</returns>
        public static object GetCustomAttribute(FieldInfo field, string attributeName)
        {
            //参数检测
            if (field == null) throw new ArgumentNullException("GetCustomAttribute.field");
            if (string.IsNullOrEmpty(attributeName)) throw new ArgumentNullException("GetCustomAttribute.attributeName");

            //获取属性对象所有自定义特性
            object attribute = null;
            object[] attributes = field.GetCustomAttributes(true);
            if (attributes.Length <= 0) return attribute;

            foreach (object obj in attributes)
            {
                if (obj.GetType().Name == attributeName)
                {
                    attribute = obj;
                    break;
                }
            }

            return attribute;
        }

        /// <summary>
        /// 获取属性对象中指定自定义特性名称的对象
        /// </summary>
        /// <param name="pi">属性对象</param>
        /// <param name="attributeName">自定义特性名称</param>
        /// <returns>自定义特性对象</returns>
        public static object GetCustomAttribute(PropertyInfo pi, string attributeName)
        {
            //参数检测
            if (pi == null) throw new ArgumentNullException("GetCustomAttribute.pi");
            if (string.IsNullOrEmpty(attributeName)) throw new ArgumentNullException("GetCustomAttribute.attributeName");

            //获取属性对象所有自定义特性
            object attribute = null;
            object[] attributes = pi.GetCustomAttributes(true);
            if (attributes.Length <= 0) return attribute;

            foreach (object obj in attributes)
            {
                if (obj.GetType().Name == attributeName)
                {
                    attribute = obj;
                    break;
                }
            }

            return attribute;
        }

        /// <summary>
        /// 获取方法对象中指定自定义特性名称的对象
        /// </summary>
        /// <param name="pi">方法对象</param>
        /// <param name="attributeName">自定义特性名称</param>
        /// <returns>自定义特性对象</returns>
        public static object GetCustomAttribute(MethodInfo mi, string attributeName)
        {
            //参数检测
            if (mi == null) throw new ArgumentNullException("GetCustomAttribute.mi");
            if (string.IsNullOrEmpty(attributeName)) throw new ArgumentNullException("GetCustomAttribute.attributeName");

            //获取属性对象所有自定义特性
            object attribute = null;
            object[] attributes = mi.GetCustomAttributes(true);
            if (attributes.Length <= 0) return attribute;

            foreach (object obj in attributes)
            {
                if (obj.GetType().Name == attributeName)
                {
                    attribute = obj;
                    break;
                }
            }

            return attribute;
        }

        /// <summary>
        /// 获取方法对象中指定自定义特性名称的对象
        /// </summary>
        /// <param name="pi">方法对象</param>
        /// <param name="attributeName">自定义特性名称</param>
        /// <returns>自定义特性对象</returns>
        public static object GetCustomAttribute(MethodBase mi, string attributeName)
        {
            //参数检测
            if (mi == null) throw new ArgumentNullException("GetCustomAttribute.mi");
            if (string.IsNullOrEmpty(attributeName)) throw new ArgumentNullException("GetCustomAttribute.attributeName");

            //获取属性对象所有自定义特性
            object attribute = null;
            object[] attributes = mi.GetCustomAttributes(true);
            if (attributes.Length <= 0) return attribute;

            foreach (object obj in attributes)
            {
                if (obj.GetType().Name == attributeName)
                {
                    attribute = obj;
                    break;
                }
            }

            return attribute;
        }

        /// <summary>
        /// 获取事件对象中指定自定义特性名称的对象
        /// </summary>
        /// <param name="ei">事件对象</param>
        /// <param name="attributeName">自定义特性名称</param>
        /// <returns>自定义特性对象</returns>
        public static object GetCustomAttribute(EventInfo ei, string attributeName)
        {
            //参数检测
            if (ei == null) throw new ArgumentNullException("GetCustomAttribute.ei");
            if (string.IsNullOrEmpty(attributeName)) throw new ArgumentNullException("GetCustomAttribute.attributeName");

            //获取属性对象所有自定义特性
            object attribute = null;
            object[] attributes = ei.GetCustomAttributes(true);
            if (attributes.Length <= 0) return attribute;

            foreach (object obj in attributes)
            {
                if (obj.GetType().Name == attributeName)
                {
                    attribute = obj;
                    break;
                }
            }

            return attribute;
        }
    }
}
