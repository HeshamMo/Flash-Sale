using FlashSale.InventoryManager.Application.Dtos;
using FlashSale.InventoryManager.Application.Inerfaces;
using Microsoft.AspNetCore.Mvc;


namespace FlashSale.InventoryManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController:ControllerBase
{
    private readonly IInventoryService _service;

    public InventoryController(IInventoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _service.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var item = await _service.GetByIdAsync(id);
        if(item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProductDto dto)
    {
        if(id != dto.Id) return BadRequest();
        var updatedProduct = await _service.UpdateAsync(dto);
        if(updatedProduct is null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await _service.DeleteAsync(id);
        if(!ok) return NotFound();
        return NoContent();
    }

}
