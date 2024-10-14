using AddressService;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService;
using BusinessAddress = AddressService.Models.Business.Address;
using DtoAddress = AddressService.Models.Dto.Address;


namespace OrderManagerApi.Controllers
{
    [ApiController]
    [Route("api/address")]
    public class AddressController : ControllerBase
    {
        private readonly IGenericService<BusinessAddress> _genericService;
        private readonly IAddressService _addressService;
        private readonly IMapper _mapper;

        public AddressController(IGenericService<BusinessAddress> genericService, IAddressService addressService, IMapper mapper)
        {
            _genericService = genericService;
            _addressService = addressService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _addressService.GetAllAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _addressService.GetByIdAsync(id);

            if (data == null) return NotFound();

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DtoAddress entity)
        {
            // Map su Business
            var businessEntity = _mapper.Map<BusinessAddress>(entity);

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
        public async Task<IActionResult> Update(int id, DtoAddress entity)
        {
            var user = await _addressService.GetByIdAsync(id);

            if (user == null) return NotFound();

            var data = _mapper.Map<BusinessAddress>(entity);
            data.Id = user.Id;

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
