using katalog.SbisData;

namespace katalog.Services.Interfaces
{
    public interface ISbisService
    {
        public Task Auth();

        public Task<List<Product>?> GetCategories();
    }
}
