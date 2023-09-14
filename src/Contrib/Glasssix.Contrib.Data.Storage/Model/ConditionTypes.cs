namespace Glasssix.Contrib.Data.Storage.Model
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