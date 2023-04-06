//Encodingクラスを使うためのインポート
using CsvHelper;
using System.Text;
namespace FirebaseSample_Csharp
{
    public class CSVSample
    {
        private String filePath;
        private String titleString = "";

        //引数で受け取った値でパラメータを初期化するコンストラクタ
        public CSVSample(String filePath)
        {
            this.filePath = filePath;
        }

        public CSVSample(String filePath, String titleString)
        {
            this.filePath = filePath;
            this.titleString = titleString;
        }

        //PageFlipHistory クラスを引数で受け取って、それを元にCSVファイルを作成する関数
        //PageFlipHistory のパラメータは以下の通り
        /*    public string dateTime { get; set; }
            public int mode { get; set; }
            public int page { get; set; }*/
        //変数と、CSVの項目名は同じとする。
        //ファイルが見つからなかった場合は作成し、1行目に項目名を記述する
        public void CsvWriteTest(PageFlipHistory readingHistory)
        {
            //filePathで指定されたファイルが存在しなかった時に作成する
            bool isFileExists = File.Exists(filePath);
            using (var writer = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                if (!isFileExists)
                    writer.WriteLine("dateTime,mode,page");

                //CSVファイルに記述する
                writer.WriteLine($"{readingHistory.dateTime},{readingHistory.mode},{readingHistory.page}");
            }
        }


        //CSVファイルを読み込み、その中身を文字列として返す関数
        public string CsvReadTest()
        {
            using (var reader = new StreamReader(filePath, Encoding.UTF8))
            {
                //CSVの1行目を読み込む
                reader.ReadLine();
                //CSVの2行目以降を読み込む
                string? line;
                string result = "";
                while ((line = reader.ReadLine()) != null)
                {
                    result += line + "\r";
                }
                return result;
            }
        }

        public void AddLineTop(string line)
        {
            var lines = File.ReadAllLines(filePath);
            var newLines = new String[lines.Length + 1];
            newLines[0] = line;
            lines.CopyTo(newLines, 1);
            File.WriteAllLines(filePath, newLines);
        }

        //ファイルの1行目が指定した文字列と違った時に、AddLineTopを呼び出して1行目を追加する
        public void AddLineTopIfNotMatch(string line)
        {
            var lines = File.ReadAllLines(filePath);
            if (lines[0] != line)
            {
                AddLineTop(line);
            }
        }

        //CSVファイルを読み取って、それを元にJSON形式の文字列を作成して、その文字列を返す関数
        //CVSファイルのデータは次の形式で記述されている。
        //dateTime,mode,page
        //以下に例を示す
        //testReadLog.csv:{"2023-04-03 16:15:00",0,20｝→  JSON{dateTime: "2023-04-03 16:15:00",mode: 0,mode: 20}
        //1行目に項目名が記述されている。
        //項目名と、変数名は同じとする。
        public string CsvReadToJson()
        {
            using (var reader = new StreamReader(filePath, Encoding.UTF8))
            {
                //CSVの1行目を読み込む
                var line = reader.ReadLine();
                var items = line.Split(",");
                //CSVの2行目以降を読み込む
                var result = "";
                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(",");
                    result += "{";
                    for (int i = 0; i < items.Length; i++)
                    {
                        result += $"\"{items[i]}\": \"{values[i]}\"";
                        if (i != items.Length - 1)
                            result += ",";
                    }
                    result += "},";
                }
                return result;
            }
        }

        //CSVの1行のデータから、PageFlipHistory クラスのインスタンスを作成する関数
        //1行の文字列を引数で受け取って、インスタンスを返す
        //CSVの1行目は項目名であることが前提である。項目名と、変数名は同じとする。
        public PageFlipHistory CsvLineToReadingHistory(string line)
        {
            var values = line.Split(",");
            var readingHistory = new PageFlipHistory();
            readingHistory.dateTime = values[0];
            readingHistory.mode = int.Parse(values[1]);
            readingHistory.page = int.Parse(values[2]);
            return readingHistory;
        }

        //1行ずつ文字列を読み込んで、リストにして返す関数
        //設定した、タイトルの文字列と同じ行は無視する
        public List<string> ReadLinesToList()
        {
            var lines = File.ReadAllLines(filePath);
            var list = new List<string>();
            foreach (var line in lines)
            {
                if (line != this.titleString)
                    list.Add(line);
            }
            return list;
        }

        //CSVファイルの中身をリセットする関数
        //もし、titleString の変数が空文字列でなかったら、1行目にそれを書き込んで他はすべて削除する
        //空文字列であったら、ファイルの1行目だけを残して後は削除する
        public void ResetFile()
        {
            var lines = File.ReadAllLines(filePath);
            if (this.titleString != "")
            {
                var newLines = new String[1];
                newLines[0] = this.titleString;
                File.WriteAllLines(filePath, newLines);
            }
            else
            {
                var newLines = new String[1];
                newLines[0] = lines[0];
                File.WriteAllLines(filePath, newLines);
            }
        }

    }
}