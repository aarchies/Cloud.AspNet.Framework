namespace Glasssix.Utils.Configuration
{
    public class VariableInfo
    {
        public VariableInfo(string key, string variable, string defaultValue)
        {
            Key = key;
            Variable = variable;
            DefaultValue = defaultValue;
        }

        public string DefaultValue { get; set; }
        public string Key { get; set; }

        public string Variable { get; set; }
    }
}