using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiNet6.Models;

public partial class Categoria
{
  public int Id { get; set; }

  public string? Descripcion { get; set; }

  [JsonIgnore]
  public virtual ICollection<Producto> Producto { get; set; } = new List<Producto>();
}
