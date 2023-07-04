namespace TrainTrain;

public static class ThresholdManager
{
    public static double GetMaxRes()
    {
        return 0.70;
    }

    public static bool HasReachMaxCapacity(int expectedSeatsBookedCount, int totalSeatsCount)
    {
        return expectedSeatsBookedCount >= Math.Floor(GetMaxRes() * totalSeatsCount);
    }
}