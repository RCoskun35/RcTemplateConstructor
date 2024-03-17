using Application.Repositories;
using Application.ViewModels.AzureViewModels.Product;
using Domain.Entities.AzureEntites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductController : ControllerBase
    {
        private readonly IAzureRepository<Product> _productsRepository;

        public ProductController(IAzureRepository<Product> productsRepository)
        {
            _productsRepository = productsRepository;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                return Ok(await _productsRepository.GetAsync(id));
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _productsRepository.GetAllAsync());
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add(VM_Product product)
        {
            try
            {
                var addEntity = new Product
                {
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = product.Quantity,
                };
                await _productsRepository.CreateEntityAsync(addEntity);
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            try
            {
              
                await _productsRepository.UpdateEntityAsync(product);
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpPost("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _productsRepository.DeleteEntityAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
