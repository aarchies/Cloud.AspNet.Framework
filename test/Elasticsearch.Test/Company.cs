namespace Apm.Test
{
    public class Company
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public User User { get; set; }
    }


    public class User
    {
        public string Name { get; set; }
        public int Gender { get; set; }
    }
}
