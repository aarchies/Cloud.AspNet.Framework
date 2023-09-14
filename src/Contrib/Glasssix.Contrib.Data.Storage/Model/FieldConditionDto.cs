namespace Glasssix.Contrib.Data.Storage.Model
{
    public class FieldConditionDto
    {
        public string Name { get; set; }

        public ConditionTypes Type { get; set; }

        public object Value { get; set; }
    }
}