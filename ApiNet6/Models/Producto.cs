using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNet6.Models;

public partial class Producto
{
  public int Id { get; set; }

  public string? CodigoBarra { get; set; }

  public string? Descripcion { get; set; }

  public string? Marca { get; set; }

  public int? IdCategoria { get; set; }

  public decimal? Precio { get; set; }

  [ForeignKey("IdCategoria")]
  public virtual Categoria? Categoria { get; set; }
}
