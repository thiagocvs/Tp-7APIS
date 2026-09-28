namespace Ejercicio2api.Entities;

public class Medicion
{
    private int id;
    private int estacionId;
    private double temperatura;
    private double humedad;
    private double velocidadViento;
    private DateTime fechaHora;

    public int Id
    {
        get => id;
        set => id = value;
    }

    public int EstacionId
    {
        get => estacionId;
        set => estacionId = value;
    }

    public double Temperatura
    {
        get => temperatura;
        set => temperatura = value;
    }

    public double Humedad
    {
        get => humedad;
        set => humedad = value;
    }

    public double VelocidadViento
    {
        get => velocidadViento;
        set => velocidadViento = value;
    }

    public DateTime FechaHora
    {
        get => fechaHora;
        set => fechaHora = value;
    }
}
