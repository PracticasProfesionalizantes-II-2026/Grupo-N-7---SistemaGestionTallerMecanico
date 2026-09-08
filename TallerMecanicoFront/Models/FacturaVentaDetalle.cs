namespace TallerMecanicoFront.Models;

public class FacturaVentaDetalle
{
    public int IdFactura { get; set; }
    public int IdTurno { get; set; }
    public decimal TotalManoObra { get; set; }
    public List<InsumoFacturaVenta> Insumos { get; set; } = new();
    public decimal TotalFactura { get; set; }
}

public class InsumoFacturaVenta
{
    public int IdInsumo { get; set; }
    public string NombreInsumo { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Total { get; set; }
}