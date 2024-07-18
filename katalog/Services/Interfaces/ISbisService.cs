using katalog.SbisData;

namespace katalog.Services.Interfaces
{
    public interface ISbisService
    {
        public Task Auth();

        public Task<List<Product>?> GetCategories();

        public Task<List<Product>?> GetProducts();

        public Task<List<Product>?> GetProducts(int hierarchicalParent);
        public Task<List<Balances>> GetRemains();
        public Task<List<Company>> GetCompanies();
        public Task<List<Warehouse>> GetWarehouses();
        public Task<List<Product>?> GetProductsSearched(string search);

        public Task<byte[]> GetImage(string url);
    }
}
