using core.Entities;

namespace Core.Interfaces;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort); 
    Task<Product?> GetProductByIdAsync(Guid id);
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
    Task<bool> SaveChangesAsync();
    bool IsProductExist(Guid id);
    Task<IReadOnlyList<string>> GetProductBrandsAync();
    Task<IReadOnlyList<string>> GetProductTypesAsync();
}
