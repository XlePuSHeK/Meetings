
namespace Service
{
    public class OutputActionValue
    {
        public static void WriteLog(string message)
        {
            var filePath = @"log.txt";
            using (var stream = new StreamWriter(filePath, true))
            {
                stream.Write(DateTime.Now + $"\t\t{message}\t\t" + Environment.NewLine);
            }
        }
        public static void Print(string message)
        {
            Console.WriteLine(message);
        }
    }
}
