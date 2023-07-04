namespace TrainTrain
{
    public record Seat
    {
        private readonly int _seatNumber;
        private readonly string _bookingRef;
        
        public readonly string CoachName;
        public string SeatName => $"{_seatNumber}{CoachName}";
        public bool IsAvailableForBooking => string.IsNullOrEmpty(_bookingRef);

        public Seat(string coachName, int seatNumber) 
            : this(coachName, seatNumber, string.Empty)
        {
        }

        public Seat(string coachName, int seatNumber, string bookingRef)
        {
            CoachName = coachName;
            _seatNumber = seatNumber;
            _bookingRef = bookingRef;
        }

        public Seat BookSeat(string bookingRef)
        {
            return new Seat(CoachName, _seatNumber, bookingRef);
        }
    }
}