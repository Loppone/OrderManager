using AutoMapper;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public abstract class GenericController<TDTO, TBUS> : ControllerBase 
    where TDTO : class
    where TBUS : class
{
    private readonly IGenericService<TBUS> _service;
    private readonly IMapper _mapper;

    public GenericController(IGenericService<TBUS> service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Create(TDTO entity)
    {
        // Map su Business
        var businessEntity = _mapper.Map<TBUS>(entity);
        var newId = await _service.AddAsync(businessEntity);

        return Created($"/api/{typeof(TDTO).Name.ToLower()}s/{newId}", entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TDTO entity)
    {
        if (id != ((dynamic)entity).Id) return BadRequest();

        var businessEntity = _mapper.Map<TBUS>(entity);
        await _service.UpdateAsync(businessEntity);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
