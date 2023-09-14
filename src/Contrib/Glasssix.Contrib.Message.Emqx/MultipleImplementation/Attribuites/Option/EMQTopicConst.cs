using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Glasssix.Contrib.Message.Emqx.MultipleImplementation.Attribuites.Option
{
    public struct EMQTopicConst
    {
        public EMQTopicConst(string topic)
        {
            Topic = topic;

            topic = topic.Replace("$", @"\$")
                         .Replace("^", @"\^");

            if (!new[] { '+', '#' }.Contains(topic.First()))
                topic = "^" + topic;

            if (topic.Last() != '#')
                topic += "$";

            var regex = topic
                .Replace("+", "[^/]*")
                .Replace("/#", ".*")
                .Replace("#", ".*")
                .Replace(".", @"\.");

            Group = "DEFAULT";
            Regex = new Regex(regex, RegexOptions.Compiled);
        }

        public EMQTopicConst(string topic, string group)
        {
            Topic = topic;
            Group = group;
            topic = topic.Replace("$", @"\$").Replace("^", @"\^");

            if (!new[] { '+', '#' }.Contains(topic.First()))
                topic = "^" + topic;

            if (topic.Last() != '#')
                topic += "$";

            var regex = topic
                .Replace("+", "[^/]*")
                .Replace("/#", ".*")
                .Replace("#", ".*")
                .Replace(".", @"\.");

            Regex = new Regex(regex, RegexOptions.Compiled);
        }

        public EMQTopicConst(string topic, string group, Regex regex)
        {
            Topic = topic;
            Group = group;
            Regex = regex;
        }

        public string Topic { get; set; }
        public Regex Regex { get; set; }
        public string? Group { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }

}
