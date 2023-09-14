namespace Glasssix.Contrib.Expect
{
    public class Expect
    {
        public static Session Spawn(ISpawnable spawnable)
        {
            spawnable.Init();
            return new Session(spawnable);
        }
    }
}
