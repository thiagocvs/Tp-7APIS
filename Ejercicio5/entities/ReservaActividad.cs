namespace Ejercicio5api.Entities;

public class ReservaActividad
{
    private int id;
    private int actividadId;
    private int participanteId;
    private DateTime fechaReserva;

    public int Id
    {
        get => id;
        set => id = value;
    }

    public int ActividadId
    {
        get => actividadId;
        set => actividadId = value;
    }

    public int ParticipanteId
    {
        get => participanteId;
        set => participanteId = value;
    }

    public DateTime FechaReserva
    {
        get => fechaReserva;
        set => fechaReserva = value;
    }
}
