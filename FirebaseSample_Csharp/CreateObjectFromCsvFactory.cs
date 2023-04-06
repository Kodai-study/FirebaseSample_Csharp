using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirebaseSample_Csharp
{
    public class CreateObjectFromCsvFactory
    {
        public static PageFlipHistory CreateRealHistoryFromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            PageFlipHistory readingHistory = new()
            {
                dateTime = values[0],
                mode = int.Parse(values[1]),
                page = int.Parse(values[2])
            };
            return readingHistory;
        }

        //CSVデータの1行の文字列を受け取って、PageFlipHistory のインスタンスを生成する関数
        //CSVファイルの形式は、｛bookMark,memo,pageNumber,resolved｝
        //CSVファイルの1行目は、項目名となっているので、読み飛ばす
        public static MemoData CreateMemoDataFromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            MemoData bookMark = new()
            {
                bookMark = int.Parse(values[0]),
                memo = values[1],
                pageNumber = int.Parse(values[2]),
                resolved = bool.Parse(values[3])
            };
            return bookMark;
        }

    }
}