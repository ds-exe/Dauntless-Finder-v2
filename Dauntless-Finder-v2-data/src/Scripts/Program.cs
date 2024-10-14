namespace Dauntless_Finder_v2.DataHandler.src.Scripts;

public class Program
{
    private static void Main(string[] args)
    {
        GenerateData.GenerateJsonData();

        GenerateIndexedData.GenerateArmourData();
    }
}
