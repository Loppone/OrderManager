using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService;
using BusinessUser = UserService.Models.Business.User;
using DtoUser = UserService.Models.Dto.User;


namespace OrderManagerApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IGenericService<BusinessUser> _genericService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IGenericService<BusinessUser> genericService, IUserService userService, IMapper mapper)
        {
            _genericService = genericService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _userService.GetAllAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _userService.GetByIdAsync(id);

            if (data == null) return NotFound();

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DtoUser entity)
        {
            // Map su Business
            var businessEntity = _mapper.Map<BusinessUser>(entity);

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
        public async Task<IActionResult> Update(int id, DtoUser entity)
        {
            var data = await _userService.GetByIdAsync(id);

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
