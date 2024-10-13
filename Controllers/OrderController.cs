using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderService;
using BusinessOrder = OrderService.Models.Business.Order;
using DtoOrder = OrderService.Models.Dto.Order;


namespace OrderManagerApi.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IGenericService<BusinessOrder> _genericService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IGenericService<BusinessOrder> genericService, IOrderService orderService, IMapper mapper)
        {
            _genericService = genericService;
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
                var newId = await _genericService.AddAsync(businessEntity);
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
            var data = await _orderService.GetByIdAsync(id);

            if (data == null) return NotFound();

           // var businessEntity = _mapper.Map<BusinessOrder>(entity);
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
