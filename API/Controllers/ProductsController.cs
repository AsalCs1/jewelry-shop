using core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductRepository repo) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            return Ok(await repo.GetProductsAsync(brand, type, sort));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await repo.GetProductByIdAsync(id);
            if (product == null) 
                return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            repo.AddProduct(product);
            if(await repo.SaveChangesAsync())
            {
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }
            return BadRequest("Problem creating product.");
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Product>> UpdateProduct(Guid id, Product product)
        {
            if (product.Id != id || repo.IsProductExist(id) == false)
                return BadRequest("Can not update product");

            repo.UpdateProduct(product);
            if(await repo.SaveChangesAsync())
            {
                return Ok(product);
            }
            
            return BadRequest("Problem updating product");
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            var product = await repo.GetProductByIdAsync(id);
            if (product == null) 
                return NotFound();

            repo.DeleteProduct(product);
            if (await repo.SaveChangesAsync())
            {
                return Ok();
            }
            return BadRequest("Problem updating product");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            return Ok(await repo.GetProductBrandsAync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            return Ok(await repo.GetProductTypesAsync());
        }
    }
}
