using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TrainTrain;

public class WebTicketManager
{
    private const string UriBookingReferenceService = "https://localhost:7264/";
    private const string UriTrainDataService = "https://localhost:7177";
    private readonly ITrainDataService _trainDataService;
    private readonly IBookingReferenceService _bookingReferenceService;

    public WebTicketManager() 
        : this(new TrainDataService(UriTrainDataService), new BookingReferenceService(UriBookingReferenceService))
    {
    }

    public WebTicketManager(ITrainDataService trainDataService, IBookingReferenceService bookingReferenceService)
    {
        _trainDataService = trainDataService;
        _bookingReferenceService = bookingReferenceService;
    }

    public async Task<string> Reserve(string trainId, int seatsRequestedCount)
    {
        var trainTopology = await _trainDataService.GetTrain(trainId);

        var train = ParseTrain(trainId, trainTopology);
        var bookingRef = await _bookingReferenceService.GetBookingReference();
            
        var bookedSeats = train.BookSeats(bookingRef, seatsRequestedCount);
        if (!bookedSeats.Any())
        {
            //? await _bookingReferenceService.DestroyBookingReference(bookingRef);
            return FormatResponse(trainId, string.Empty, bookedSeats);
        }

        await _trainDataService.Reserve(trainId, bookingRef, bookedSeats);
        return FormatResponse(trainId, bookingRef, bookedSeats);
    }
    
    private static Train ParseTrain(string trainId, string trainTopology)
    {
        //var sample =
        //"{\"seats\": {\"1A\": {\"booking_reference\": \"\", \"seat_number\": \"1\", \"coach\": \"A\"}, \"2A\": {\"booking_reference\": \"\", \"seat_number\": \"2\", \"coach\": \"A\"}}}";

        // Forced to workaround with dynamic parsing since the received JSON is invalid format ;-(
        dynamic parsed = JsonConvert.DeserializeObject(trainTopology) ?? new JObject();

        var seats = new List<Seat>();
        foreach (var token in (JContainer)parsed!)
        {
            var allStuffs = (JObject)((JContainer)token).First!;
            foreach (var stuff in allStuffs)
            {
                var seat = stuff.Value?.ToObject<SeatJsonPoco>();
                if (seat == null)
                    continue;
                    
                seats.Add(new Seat(seat.coach, int.Parse(seat.seat_number), seat.booking_reference));
            }
        }

        return new Train(trainId, seats);
    }

    private static string FormatResponse(string trainId, string bookingRef, List<Seat> bookedSeats)
    {
        return $"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"{bookingRef}\", \"seats\": {FormatSeats(bookedSeats)}}}";
    }

    private static string FormatSeats(IEnumerable<Seat> seats)
    {
        var sb = new StringBuilder("[");

        var firstTime = true;
        foreach (var seat in seats)
        {
            if (!firstTime)
                sb.Append(", ");
            else
                firstTime = false;

            sb.Append($"\"{seat.SeatName}\"");
        }

        sb.Append("]");
        return sb.ToString();
    }
}