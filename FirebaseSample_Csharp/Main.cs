using System;

namespace FirebaseSample_Csharp
{
    public class MainProgram
    {
        public static void Main()
        {
            bool isWifiConnect = true;
            RTDatabaseManager mineObject = new();
            Console.Write(mineObject.CsvReadTest());

            string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            PageFlipHistory pageFlipData = new(dateTime, 2, 2);
            CSVSample sample = new("testReadLog.csv", "dateTime,mode,page");

            if (!isWifiConnect)
            {
                sample.AddLineTopIfNotMatch("dateTime,mode,page");
                sample.CsvWriteTest(pageFlipData);
                Console.WriteLine(sample.CsvReadToJson());
            }
            else
            {
                List<String> logList = sample.ReadLinesToList();
                List<PageFlipHistory> readingHistoryList = new();
                foreach (var log in logList)
                {
                    var data = CreateObjectFromCsvFactory.
                        CreateRealHistoryFromCsv(log);
                    mineObject.PushDataWithRTDB(data).Wait();
                }
                mineObject.PushDataWithRTDB(pageFlipData).Wait();
                sample.ResetFile();
            }
        }
    }
}