namespace katalog.Services.Interfaces
{
    public interface ISbisService
    {
        public Task Auth(string login, string password);
    }
}
