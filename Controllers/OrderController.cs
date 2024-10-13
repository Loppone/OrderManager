using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderService;
using BusinessOrder = OrderService.Models.Business.Order;
using BusinessOrderProduct = OrderService.Models.Business.OrderProduct;
using DtoOrder = OrderService.Models.Dto.Order;
using DtoOrderProduct = OrderService.Models.Dto.OrderProduct;


namespace OrderManagerApi.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IGenericService<BusinessOrder> _genericServiceOrder;
        private readonly IGenericService<BusinessOrderProduct> _genericServiceOrderProduct;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(
            IGenericService<BusinessOrder> genericServiceOrder,
            IGenericService<BusinessOrderProduct> genericServiceOrderProduct,  
            IOrderService orderService, 
            IMapper mapper)
        {
            _genericServiceOrder = genericServiceOrder;
            _genericServiceOrderProduct = genericServiceOrderProduct;
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _orderService.GetAllAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _orderService.GetByIdAsync(id);

            if (data == null) return NotFound();

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DtoOrder entity)
        {
            // Map su Business
            var businessEntity = _mapper.Map<BusinessOrder>(entity);

            try
            {
                var newId = await _genericServiceOrder.AddAsync(businessEntity);
                return CreatedAtAction(nameof(GetById), new { id = newId }, newId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DtoOrder entity)
        {
            var order = await _orderService.GetByIdAsync(id);

            if (order == null) return NotFound();
            
            var data = _mapper.Map<BusinessOrder>(entity);
            data.Id = order.Id;

            await _genericServiceOrder.UpdateAsync(data);

            return NoContent();
        }

        [HttpPost("{id}/add-product")]
        public async Task<IActionResult> AddProduct(int id, DtoOrderProduct entity)
        {
            var businessEntity = _mapper.Map<BusinessOrderProduct>(entity);

            try
            {
                var newId = await _genericServiceOrderProduct.AddAsync(businessEntity);
                return CreatedAtAction(nameof(GetById), new { id = newId }, newId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                await _genericServiceOrder.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpDelete("{id}/delete-product")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _genericServiceOrderProduct.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
