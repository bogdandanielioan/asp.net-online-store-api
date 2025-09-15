namespace OnlineSchool.System.Exceptions
{
    public class ItemsDoNotExist : Exception
    {
        public ItemsDoNotExist(string? message) : base(message) { }
    }
}
