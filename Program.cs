using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;


namespace CSharp17
{
    struct Vector2
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    
    [Serializable]
    
    class Player
    {
        public string name { get; set; }
        public int level { get; set;}
        public int gold { get; set; }
        public Vector2 position { get; set; }
    }

    class Item
    {
        public int id;
        public string name;
        public string level;
        public string job;
        public int power;
        public Item(string csv)
        {
            string[] datas = csv.Split(',');
            id = int.Parse(datas[0]);
            name = datas[1];
            level = datas[2];
            job = datas[3];
            power = int.Parse(datas[4]);

        }
    }

    class PlayerData
    {
        public string name;
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            if (false)
            {

                Player player = new Player()
                {
                    name = "플레이어 1004",
                    level = 24,
                    gold = 123132,
                    position = new Vector2() { x = 10, y = 20 }
                };

                //디렉토리 파일생성(폴더생성)
                Directory.CreateDirectory("Database");                  //DataBase폴더 있는가
                Console.WriteLine(Directory.Exists("Database"));        //없으면 새로만든다.


                //플레이어의 데이터를 파일로 저장하기 
                //File.Create("Database/PlayerData.txt");
                string path = "Database/PlayerData.txt";
                StreamWriter writer = new StreamWriter(path);

                string json = JsonSerializer.Serialize(player);
                Console.WriteLine(json);
                writer.WriteLine(json);
                writer.Close();
            }

            StreamReader sr = new StreamReader("Database/PlayerData.txt");
            string readJson = sr.ReadToEnd();
            Player p1 = JsonSerializer.Deserialize<Player>(readJson);
            sr.Close();

            Console.WriteLine($"Name: {p1.name}");
            Console.WriteLine($"Level:{p1.level}");
            Console.WriteLine($"Gold{p1.gold}");
            Console.WriteLine($"Position: {p1.position}");

            //경로상의 ItemDB.csv파일을 읽어 아이템 객체 파일

            //1.csv파일을 가져온다
            //2.가장 첫 행을 제외하고 한 라인(행)씩 데이터를 읽는다
            //3.읽은 데이터를 통해 Item객체를 생성한다.
            //4. 생성한 객체를 배열로 만든다.

            List<Item> list = new List<Item>();

            sr = new StreamReader("Database/ItemDB.csv");
            sr.ReadLine();
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine().Trim();
                if (!string.IsNullOrEmpty(line))                      //공백이 아닌경우에 
                    list.Add(new Item(line));
            }
            sr.Close();
            Console.WriteLine($"불러온 아이템의 개수는 : {list.Count}");
            //item 컬렉션에서 파워가 100이상만 필터링하기
            //search의 자료형은 select절에서 item(Item)자료형을 리턴했기 때문에
            //IEnumable<Item>이다.
            var search = from item in list
                         where item.power >= 100
                         select new { Name = item.name, Power = item.power };

            foreach (var valie in search)
                Console.WriteLine($"이름:{valie.Name},파워: {valie.Power}");


            //Func<Item,bool>predicate
            Item[] itemArray = list.Where(item => item.power >= 100).ToArray();

        } 


    }
}
