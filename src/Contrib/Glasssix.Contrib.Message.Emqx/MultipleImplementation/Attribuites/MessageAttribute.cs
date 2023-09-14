using System;

namespace Glasssix.Contrib.Message.Emqx.MultipleImplementation.Attribuites
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MessageAttribute : Attribute
    {
        public string RouteKey { get; set; }
        public string Group { get; set; }

        public MessageAttribute(string routeKey, string group)
        {
            RouteKey = routeKey;
            Group = group;
        }

        public MessageAttribute(string routeKey)
        {
            RouteKey = routeKey;
        }
    }
}
