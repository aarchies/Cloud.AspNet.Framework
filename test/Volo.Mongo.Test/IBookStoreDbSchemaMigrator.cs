namespace Volo.Mongo.Test;

public interface IBookStoreDbSchemaMigrator
{
    Task MigrateAsync();
}
