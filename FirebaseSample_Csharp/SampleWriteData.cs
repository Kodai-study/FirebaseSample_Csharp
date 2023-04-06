/// <summary>
/// RealTimeDatabaseへの書き込みテストで書き込むデータ型。
/// </summary>
namespace FirebaseSample_Csharp
{
    public class SampleWriteData
    {
        public string Name { get; set; }
        //コンストラクタで変数を初期化
        public SampleWriteData()
        {
            this.Name = "Noname";
        }
        //UNIXTIMEから、時刻の文字列を作成する。
        //形式は、 YYYY-MM-DD
        public string UnixTimeToString(long unixTime)
        {
            var dt = DateTimeOffset.FromUnixTimeSeconds(unixTime);
            return dt.ToString("yyyy-MM-dd");
        }
    }

    //以下のJSON形式から、必要な変数を推測して、それらを持ったクラスを作成する
    /*{"test2":{"date":"2023/03/19 12:11:00","memoList":[{"bookMark":0,"memo":"beryGood","pageNumber":1,"resolved":false},{"bookMark":1,"memo":"BeryUsefull","pageNumber":2,"resolved":true}],"name":"ほんわかわかさんセカンド"}*/
    //以下に、JSON形式から推測したクラスを示す
    public class MemoData
    {
        public int bookMark { get; set; }
        public string memo { get; set; }
        public int pageNumber { get; set; }
        public bool resolved { get; set; }
    }
    public class BookData
    {
        public string date { get; set; }
        public List<MemoData> memoList { get; set; }
        public string name { get; set; }

        //引数から変数を初期化するコンストラクタ
        public BookData(string date, List<MemoData> memoList, string name)
        {
            this.date = date;
            this.memoList = memoList;
            this.name = name;
        }
    }

    //次のJSON形式からデータクラスを作成
    /*[[{"dateTime":"2023/03/20 17:36:00","mode":0,"page":1},{"dateTime":"2023/03/22 17:37:00","mode":1,"page":2}]]*/
    public class PageFlipHistory
    {
        public string dateTime { get; set; }
        public int mode { get; set; }
        public int page { get; set; }

        //empty constructor
        public PageFlipHistory() {
            //変数の初期値を設定 データを持たない、空を表す値に設定する
            this.dateTime = "";
            this.mode = -1;
            this.page = -1;
        }

        //引数から変数を初期化するコンストラクタ
        public PageFlipHistory(string dateTime, int mode, int page)
        {
            this.dateTime = dateTime;
            this.mode = mode;
            this.page = page;
        }
    }
}