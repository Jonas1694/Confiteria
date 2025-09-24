using ArquitecturaModel.Model;

namespace ArquitecturaModel.ViewModels
{
    public class GridProductoVewModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public double Stock { get; set; }
        public Marcas Marcas { get; set; } = new Marcas();
        public DateTime Fecha { get; set; }
        public string SucursalName { get; set; }
    }
}
