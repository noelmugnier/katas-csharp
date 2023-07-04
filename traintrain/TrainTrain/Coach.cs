namespace TrainTrain;

public record Coach
{
    private readonly string _name;
    private readonly List<Seat> _seats;

    public Coach(string name, List<Seat> seats)
    {
        _name = name ?? string.Empty;
        _seats = seats ?? new ();
    }

    internal int ReservedSeatsCount => _seats.Count(s => !s.IsAvailableForBooking);
    internal int SeatsCount => _seats.Count;
    private int AvailableSeats => _seats.Count(s => s.IsAvailableForBooking);

    public List<Seat> BookSeats(string bookingRef, int seatsRequestedCount)
    {
        if (!HasEnoughAvailableSeats(seatsRequestedCount))
            return new();
        
        var availableSeats = GetAvailableSeats(seatsRequestedCount);
        return BookAvailableSeats(availableSeats, bookingRef);
    }

    public bool HasEnoughAvailableSeats(int seatsRequestedCount)
    {
        return seatsRequestedCount <= AvailableSeats && !HasReachMaxCapacity(seatsRequestedCount);
    }

    private List<Seat> GetAvailableSeats(int seatsRequestedCount)
    {
        return _seats.Where(s => s.IsAvailableForBooking).Take(seatsRequestedCount).ToList();
    }

    private static List<Seat> BookAvailableSeats(List<Seat> availableSeats, string bookingRef)
    {
        return availableSeats.Select(s => s.BookSeat(bookingRef)).ToList();
    }
        
    private bool HasReachMaxCapacity(int seatsRequestedCount)
    {
        return ThresholdManager.HasReachMaxCapacity(ReservedSeatsCount + seatsRequestedCount, SeatsCount);
    }
}