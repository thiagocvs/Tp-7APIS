namespace Ejercicio3api.Entities;

public class Desafio
{
    private int id;
    private string titulo = string.Empty;
    private string dificultad = string.Empty;
    private int puntajeMaximo;
    private bool activo;

    public int Id
    {
        get => id;
        set => id = value;
    }

    public string Titulo
    {
        get => titulo;
        set => titulo = value;
    }

    public string Dificultad
    {
        get => dificultad;
        set => dificultad = value;
    }

    public int PuntajeMaximo
    {
        get => puntajeMaximo;
        set => puntajeMaximo = value;
    }

    public bool Activo
    {
        get => activo;
        set => activo = value;
    }
}
