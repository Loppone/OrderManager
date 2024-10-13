using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductService;
using BusinessProduct = ProductService.Models.Business.Product;
using DtoProduct = ProductService.Models.Dto.Product;


namespace OrderManagerApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IGenericService<BusinessProduct> _genericService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IGenericService<BusinessProduct> genericService, IProductService productService, IMapper mapper)
        {
            _genericService = genericService;
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _productService.GetAllAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _productService.GetByIdAsync(id);

            if (data == null) return NotFound();

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DtoProduct entity)
        {
            // Map su Business
            var businessEntity = _mapper.Map<BusinessProduct>(entity);

            try
            {
                var newId = await _genericService.AddAsync(businessEntity);
                return CreatedAtAction(nameof(GetById), new { id = newId }, newId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DtoProduct entity)
        {
            var data = await _productService.GetByIdAsync(id);

            if (data == null) return NotFound();

            await _genericService.UpdateAsync(data);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _genericService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
