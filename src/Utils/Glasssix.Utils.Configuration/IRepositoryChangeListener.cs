namespace Glasssix.Utils.Configuration
{
    public interface IRepositoryChangeListener
    {
        void OnRepositoryChange(SectionTypes sectionType, Properties newProperties);
    }
}