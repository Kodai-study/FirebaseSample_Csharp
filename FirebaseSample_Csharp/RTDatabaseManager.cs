using CsvHelper;
using Firebase.Database;
using Firebase.Database.Query;
using System.Globalization;
using Newtonsoft.Json;
using System;
using System.Text;

namespace FirebaseSample_Csharp
{
    public class RTDatabaseManager
    {
        private readonly String databasePath;
        private FirebaseClient firebaseClient;
        private IReadOnlyCollection<FirebaseObject<SampleWriteData>> databaseSnapShot;
        //配列の一番最後の添え字を保存する変数
        private int arrayIndex;

        //コンストラクタで、パラメータを初期化
        //firebaseClientは、URLを指定してインスタンスを作成
        public RTDatabaseManager()
        {
            this.databasePath = "https://modular-source-342310-default-rtdb.firebaseio.com/";
            this.firebaseClient = new FirebaseClient(databasePath);
            this.databaseSnapShot = this.firebaseClient
            .Child("sample")
            .OnceAsListAsync<SampleWriteData>().Result;
            this.arrayIndex = databaseSnapShot.Count;
        }

        /// <summary>
        /// データベースの配列に、データを追加する。
        /// </summary>
        /// <param name="data"> 追加するオブジェクトデータ。 これによって追加されるデータは、
        /// 「(変数名): (要素)」 で、JSON形式で保存される。</param>
        /// <returns> 非同期タスク。ネットワーク通信を行うため、処理の終了まで時間がかかる </returns>
        public async Task PushDataWithRTDB(Object data)
        {
            //FirebaseClient のインスタンスを作成する
            //定数で定義されている、データベースのURLから firebaseClient のインスタンスを作成する
            this.firebaseClient = new FirebaseClient(this.databasePath);
            Console.WriteLine(databaseSnapShot.Count);
            foreach (var item in databaseSnapShot)
            {
                Console.WriteLine(item.Key);
                Console.WriteLine(item.Object);
            }
            string json = JsonConvert.SerializeObject(data);
            await firebaseClient.Child("sample").
                Child(arrayIndex.ToString()).PutAsync(json);
            this.arrayIndex++;
        }

        void GetData()
        {
            //データを取得
            this.databaseSnapShot = firebaseClient.Child("sample")
                .OnceAsListAsync<SampleWriteData>().Result;

            //データを表示
            foreach (var item in databaseSnapShot)
            {
                Console.WriteLine(item.Object.Name);
            }
        }


        //CSVファイルを読み取って、それを元にJSON形式の文字列を作成して、その文字列を返す関数
        //CVSファイルのデータは次の形式で記述されている。
        //(name),(age),(type)
        //以下に例を示す
        //sample.csv:{"sato",20,0｝→  JSON{name: "sato",age: 20,type: 0}
        public string CsvReadTest()
        {
            //CSVファイルを読み込む
            using var reader = new StreamReader("sample.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            //CSVファイルのデータを読み込んで、string型の配列に格納する
            var records = csv.GetRecords<dynamic>().ToList();
            //JSON形式の文字列を作成する
            StringBuilder jsonString = new("[");
            foreach (var item in records)
            {
                jsonString.Append('{');
                jsonString.Append("\"name\":");
                jsonString.Append("\"" + item.name + "\",");
                jsonString.Append("\"age\":");
                jsonString.Append(item.age + ",");
                jsonString.Append("\"type\":");
                jsonString.Append(item.type);
                jsonString.Append("},");
            }
            jsonString = jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append(']');
            return jsonString.ToString();
        }
    }
}