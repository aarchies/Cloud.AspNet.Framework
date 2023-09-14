namespace Glasssix.Utils.Configuration
{
    public interface IConfigurationRepository
    {
        SectionTypes SectionType { get; }

        void AddChangeListener(IRepositoryChangeListener listener);

        Properties Load();

        void RemoveChangeListener(IRepositoryChangeListener listener);
    }
}