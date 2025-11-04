using API.RequestHelpers;
using core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController(IGenericRepository<Product> repo) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(
            [FromQuery] ProductSpecParams specParams)
        {
            var spec = new ProductSpecification(specParams);

            return await CreatePageResult(repo, spec, specParams.PageIndex, specParams.PageSize);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Product>> GetProducts(Guid id)
        {
            var product = await repo.GetByIdAsync(id);
            if (product == null)return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            repo.Add(product);
            if (await repo.SaveAllAsync())
            {
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }
            return BadRequest("Problem creating the product.");
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Product>> UpdateProduct(Guid id, Product product)
        {
            if (product.Id != id || repo.Exists(id))
                return BadRequest("Cannot update this product");

            repo.Update(product);
            if (await repo.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem updating the product");
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            var product = await repo.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            repo.Remove(product);
            if (await repo.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Problem deleting the product");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var spec = new BrandListSpecification();

            return Ok(await repo.ListAsync(spec));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new TypeListSpecification();

            return Ok(await repo.ListAsync(spec));
        }

        private bool ProductExists(Guid id)
        {
            return repo.Exists(id);
        }
    }
}
