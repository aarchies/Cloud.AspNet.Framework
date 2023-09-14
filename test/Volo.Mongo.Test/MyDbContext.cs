using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Mongo.Test
{
    [ConnectionStringName("Mongo")]
    public class MyDbContext : AbpMongoDbContext
    {
        public IMongoCollection<Question> Questions => Collection<Question>();
        public IMongoCollection<Book> Book => Collection<Book>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>(b =>
            {
                b.CollectionName = "Questions";
            });

            modelBuilder.Entity<Book>(b =>
            {
                b.CollectionName = "Book";
            });
        }
    }
}
