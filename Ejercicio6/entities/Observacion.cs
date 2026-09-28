namespace Ejercicio6api.Entities;

public class Observacion
{
    private int id;
    private int objetoEspacialId;
    private DateTime fecha;
    private double distanciaMedida;
    private double velocidad;
    private string comentario = string.Empty;

    public int Id
    {
        get => id;
        set => id = value;
    }

    public int ObjetoEspacialId
    {
        get => objetoEspacialId;
        set => objetoEspacialId = value;
    }

    public DateTime Fecha
    {
        get => fecha;
        set => fecha = value;
    }

    public double DistanciaMedida
    {
        get => distanciaMedida;
        set => distanciaMedida = value;
    }

    public double Velocidad
    {
        get => velocidad;
        set => velocidad = value;
    }

    public string Comentario
    {
        get => comentario;
        set => comentario = value;
    }
}
