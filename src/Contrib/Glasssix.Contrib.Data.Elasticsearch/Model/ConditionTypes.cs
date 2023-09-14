namespace Glasssix.Contrib.Data.Elasticsearch.Model
{
    public enum ConditionTypes
    {
        Equal = 1,
        NotEqual,
        Great,
        Less,
        GreatEqual,
        LessEqual,
        In,
        NotIn,
        Regex,
        NotRegex,
        Exists,
        NotExists
    }
}