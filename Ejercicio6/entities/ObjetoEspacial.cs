namespace Ejercicio6api.Entities;

public class ObjetoEspacial
{
    private int id;
    private string nombre = string.Empty;
    private string tipo = string.Empty;
    private double distancia;
    private int nivelRiesgo;
    private DateTime fechaDescubrimiento;
    private bool activo;

    public int Id
    {
        get => id;
        set => id = value;
    }

    public string Nombre
    {
        get => nombre;
        set => nombre = value;
    }

    public string Tipo
    {
        get => tipo;
        set => tipo = value;
    }

    public double Distancia
    {
        get => distancia;
        set => distancia = value;
    }

    public int NivelRiesgo
    {
        get => nivelRiesgo;
        set => nivelRiesgo = value;
    }

    public DateTime FechaDescubrimiento
    {
        get => fechaDescubrimiento;
        set => fechaDescubrimiento = value;
    }

    public bool Activo
    {
        get => activo;
        set => activo = value;
    }
}
