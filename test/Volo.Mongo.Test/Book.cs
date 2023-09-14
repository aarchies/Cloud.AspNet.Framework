using Volo.Abp.Domain.Entities;

namespace Volo.Mongo.Test
{
    public class Book : AggregateRoot<Guid>
    {
        public string Name { get; set; }

        public int Type { get; set; }
    }

}
