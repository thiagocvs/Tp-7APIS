namespace Ejercicio5api.Entities;

public class Actividad
{
    private int id;
    private string nombre = string.Empty;
    private string tipo = string.Empty;
    private DateTime horario;
    private int duracionMinutos;
    private int capacidad;
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

    public string Tipo
    {
        get => tipo;
        set => tipo = value;
    }

    public DateTime Horario
    {
        get => horario;
        set => horario = value;
    }

    public int DuracionMinutos
    {
        get => duracionMinutos;
        set => duracionMinutos = value;
    }

    public int Capacidad
    {
        get => capacidad;
        set => capacidad = value;
    }

    public bool Activa
    {
        get => activa;
        set => activa = value;
    }
}
