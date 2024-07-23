namespace LogisticsApp.Utilities;

public static class GenerateOTP
{
    public static string Generate()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}