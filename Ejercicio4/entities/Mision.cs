namespace Ejercicio4api.Entities;

public class Mision
{
    private int id;
    private int droneId;
    private string descripcion = string.Empty;
    private double distanciaKm;
    private DateTime fecha;
    private bool completada;

    public int Id
    {
        get => id;
        set => id = value;
    }

    public int DroneId
    {
        get => droneId;
        set => droneId = value;
    }

    public string Descripcion
    {
        get => descripcion;
        set => descripcion = value;
    }

    public double DistanciaKm
    {
        get => distanciaKm;
        set => distanciaKm = value;
    }

    public DateTime Fecha
    {
        get => fecha;
        set => fecha = value;
    }

    public bool Completada
    {
        get => completada;
        set => completada = value;
    }
}
