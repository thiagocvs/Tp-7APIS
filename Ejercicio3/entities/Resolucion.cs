namespace Ejercicio3api.Entities;

public class Resolucion
{
    private int id;
    private int participanteId;
    private int desafioId;
    private int puntajeObtenido;
    private DateTime fechaEntrega;

    public int Id
    {
        get => id;
        set => id = value;
    }

    public int ParticipanteId
    {
        get => participanteId;
        set => participanteId = value;
    }

    public int DesafioId
    {
        get => desafioId;
        set => desafioId = value;
    }

    public int PuntajeObtenido
    {
        get => puntajeObtenido;
        set => puntajeObtenido = value;
    }

    public DateTime FechaEntrega
    {
        get => fechaEntrega;
        set => fechaEntrega = value;
    }
}
