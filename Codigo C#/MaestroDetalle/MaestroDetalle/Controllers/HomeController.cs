using MaestroDetalle.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;



namespace MaestroDetalle.Controllers
{
    public class HomeController : Controller
    {
        private readonly string cadenaSQL;

        public HomeController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        public IActionResult Index()
        {
            return View();
        }
        
        //Creado    
        [HttpPost]
        public JsonResult GuardarVenta([FromBody] Venta body)
            {
            //Informacion de la venta
            XElement venta = new XElement("Venta",  
                
                new XElement("NumeroDocumento", body.NumeroDocumento),
                new XElement("RazonSocial", body.RazonSocial),
                new XElement("Total", body.Total));

            //Informacion DetalleVenta
            XElement oDetalleVenta = new XElement("Detalle_Venta");

            //Recorrer los elementos dentro de Detall_Venta
            foreach(DetalleVenta item in body.LstDetalleVenta)
            {
                oDetalleVenta.Add(new XElement("Item",
                    new XElement("Producto", item.Producto),
                    new XElement("Precio", item.Precio),
                    new XElement("Cantidad", item.Cantidad),
                    new XElement("Total", item.Total)
                    ));
            }

            venta.Add(oDetalleVenta);

            //Guardar toda esa informacion y mandarsela al SP creado en SQL y llenar las tablas
            using(SqlConnection conexion = new SqlConnection(cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_guardarVenta", conexion);
                cmd.Parameters.Add("venta_xml", SqlDbType.Xml).Value = venta.ToString();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.ExecuteNonQuery();
            }


            return Json(new {respuesta = true}); //Leera el ajax que esta en el index linea 149
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
