   public class VistaDesarrollador
{
    public int DesarrolladorId { get; set; }
    public string NombreCompleto { get; set; }
    public string Email { get; set; }
    public string Telefono { get; set; }
    public string DNI { get; set; }
    public string Observacion { get; set; }
    public int PuestoId { get; set; }
    public string? NombrePuesto { get; set; } // Nuevo campo para mostrar en la vista
}