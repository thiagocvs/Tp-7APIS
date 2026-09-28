namespace Ejercicio2api.Entities;

public class Estacion
{
    private int id;
    private string nombre = string.Empty;
    private string localidad = string.Empty;
    private bool activa;

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

    public string Localidad
    {
        get => localidad;
        set => localidad = value;
    }

    public bool Activa
    {
        get => activa;
        set => activa = value;
    }
}
