namespace Ejercicio4api.Entities;

public class Drone
{
    private int id;
    private string codigo = string.Empty;
    private string modelo = string.Empty;
    private int bateria;
    private string estado = "Disponible";

    public int Id
    {
        get => id;
        set => id = value;
    }

    public string Codigo
    {
        get => codigo;
        set => codigo = value;
    }

    public string Modelo
    {
        get => modelo;
        set => modelo = value;
    }

    public int Bateria
    {
        get => bateria;
        set => bateria = value;
    }

    public string Estado
    {
        get => estado;
        set => estado = value;
    }
}
