using Microsoft.AspNetCore.Mvc;
using OrderService;
using OrderService.Models;

namespace OrderManagerApi.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : GenericController<Order>
    {
        private readonly IGenericService<Order> _genericService;
        private readonly IOrderService _orderService;

        public OrderController(IGenericService<Order> service, IOrderService orderService) : base(service)
        {
            _genericService = service;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _orderService.GetAllAsync();

            return Ok(data);
        }
    }
}
