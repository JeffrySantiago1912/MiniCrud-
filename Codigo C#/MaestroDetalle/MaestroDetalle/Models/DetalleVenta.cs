using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaestroDetalle.Models
{
    public class DetalleVenta
    {
        public int IdDetalleVenta { get; set; }
        public string Producto { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
    }
}
