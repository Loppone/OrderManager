using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public abstract class GenericController<T> : ControllerBase where T : class
{
    private readonly IGenericService<T> _service;

    public GenericController(IGenericService<T> service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(T entity)
    {
        var newId = await _service.AddAsync(entity);

        return Created($"/api/{typeof(T).Name.ToLower()}s/{newId}", entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, T entity)
    {
        if (id != ((dynamic)entity).Id) return BadRequest();

        await _service.UpdateAsync(entity);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
