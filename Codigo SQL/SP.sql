USE [DB_MaestroDetalle]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_guardarVenta](
@venta_xml xml
)

as
begin

 insert into Venta(NumeroDocumento,RazonSocial,Total)

 select
 nodo.elemento.value('NumeroDocumento[1]', 'varchar(20)')[NumeroDocumento],
 nodo.elemento.value('RazonSocial[1]', 'varchar(50)')[RazonSocial],
 nodo.elemento.value('Total[1]', 'decimal(10,2)')[Total]
 from @venta_xml.nodes('Venta') nodo(elemento)
 
 declare @idventa_generado int = (select max(idventa) from Venta)

 insert into DETALLE_VENTA(IdVenta, Producto, Precio, Cantidad, Total)
 select
 @idventa_generado[idventa],
 nodo.elemento.value('Producto[1]', 'varchar(50)')[Producto],
 nodo.elemento.value('Precio[1]', 'decimal(10,2)')[Precio],
 nodo.elemento.value('Cantidad[1]', 'int')[Cantidad],
 nodo.elemento.value('Total[1]', 'decimal(10,2)')[Total]
 from @venta_xml.nodes('Venta/Detalle_Venta/Item') nodo(elemento)
end