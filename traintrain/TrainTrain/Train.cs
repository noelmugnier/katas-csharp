namespace TrainTrain;

public class Train
{
    private readonly string _trainId;
    private readonly List<Coach> _coaches;
    private int SeatsCount => _coaches.Sum(c => c.SeatsCount);
    private int ReservedSeatsCount => _coaches.Sum(c => c.ReservedSeatsCount);

    public Train(string trainId, List<Seat> seats)
    {
        _trainId = trainId;
        _coaches = seats.GroupBy(s => s.CoachName)
            .Select(groupedSeat => new Coach(groupedSeat.Key, groupedSeat.ToList()))
            .ToList();
    }

    public List<Seat> BookSeats(string bookingRef, int seatsRequestedCount)
    {
        if (HasReachMaxCapacity(seatsRequestedCount))
            return new();

        var coach = _coaches.FirstOrDefault(c => c.HasEnoughAvailableSeats(seatsRequestedCount));
        return coach?.BookSeats(bookingRef, seatsRequestedCount) ?? new();
    }

    private bool HasReachMaxCapacity(int seatsRequestedCount)
    {
        return ThresholdManager.HasReachMaxCapacity(ReservedSeatsCount + seatsRequestedCount, SeatsCount);
    }
}

internal class SeatJsonPoco
{
    public string booking_reference { get; set; }
    public string seat_number { get; set; }
    public string coach { get; set; }
}