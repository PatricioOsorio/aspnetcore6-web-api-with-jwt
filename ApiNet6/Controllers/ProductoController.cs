using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiNet6.Models;
using Microsoft.AspNetCore.Cors;

namespace ApiNet6.Controllers
{
  [EnableCors("CorsRules")]
  [Route("api/[controller]")]
  [ApiController]
  public class ProductoController : ControllerBase
  {
    public readonly DbApiContext _dbContext;

    public ProductoController(DbApiContext context)
    {
      _dbContext = context;
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<IActionResult> GetAllProducts()
    {
      List<Producto> products = new();

      try
      {
        products = await _dbContext.Producto.Include(c => c.Categoria).ToListAsync();
        return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = products });
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status404NotFound, new { message = ex.Message });
      }
    }

    [HttpGet]
    [Route("GetById/{idProduct:int}")]
    public async Task<IActionResult> GetByIdProduct(int idProduct)
    {
      Producto product = await _dbContext.Producto.FindAsync(idProduct);

      if (product == null)
      {
        return BadRequest("Producto no encontrado");
      }

      try
      {
        product = await _dbContext.Producto.Include(c => c.Categoria).Where(p => p.Id == idProduct).FirstOrDefaultAsync();

        return StatusCode(StatusCodes.Status200OK, new { message = "ok", response = product });
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status404NotFound, new { message = ex.Message });
      }
    }

    [HttpPost]
    [Route("Save")]
    public async Task<IActionResult> SaveProduct([FromBody] Producto producto)
    {
      try
      {
        _dbContext.Producto.Add(producto);
        await _dbContext.SaveChangesAsync();

        return StatusCode(StatusCodes.Status200OK, new { message = "ok" });

      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status406NotAcceptable, new { message = ex.Message });
      }
    }

    [HttpPut]
    [Route("Edit")]
    public async Task<IActionResult> EditProduct([FromBody] Producto newProduct)
    {
      Producto product = await _dbContext.Producto.FindAsync(newProduct.Id);

      if (product == null)
      {
        return BadRequest("Producto no encontrado");
      }

      try
      {
        product.CodigoBarra = newProduct.CodigoBarra is null ? product.CodigoBarra : newProduct.CodigoBarra;
        product.Descripcion = newProduct.Descripcion is null ? product.Descripcion : newProduct.Descripcion;
        product.Marca = newProduct.Marca is null ? product.Marca : newProduct.Marca;
        product.Precio = newProduct.Precio is null ? product.Precio : newProduct.Precio;

        _dbContext.Producto.Update(product);
        await _dbContext.SaveChangesAsync();

        return StatusCode(StatusCodes.Status200OK, new { message = "ok" });

      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status406NotAcceptable, new { message = ex.Message });
      }
    }

    [HttpDelete]
    [Route("Delete/{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
      Producto product = await _dbContext.Producto.FindAsync(id);

      if (product == null)
      {
        return BadRequest("Producto no encontrado");
      }

      try
      {
        _dbContext.Producto.Remove(product);
        await _dbContext.SaveChangesAsync();

        return StatusCode(StatusCodes.Status200OK, new { message = "ok" });
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status406NotAcceptable, new { message = ex.Message });
      }
    }
  }
}
