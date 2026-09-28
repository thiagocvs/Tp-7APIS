namespace Ejercicio5api.Entities;

public class Participante
{
    private int id;
    private string nombre = string.Empty;
    private string email = string.Empty;
    private int edad;

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

    public string Email
    {
        get => email;
        set => email = value;
    }

    public int Edad
    {
        get => edad;
        set => edad = value;
    }
}
