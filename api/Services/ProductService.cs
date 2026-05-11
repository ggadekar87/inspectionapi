using api.model;

namespace api.Service
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;

        public ProductService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var response = await _http.GetAsync("https://api.example.com/products");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<Product>>();
        }
    }

}
