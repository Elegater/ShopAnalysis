using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace ShopAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            string shipListFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Input", "ShipList.txt");
            List<string> ships = Regex.Replace(File.ReadAllText(shipListFilePath), @"[^\w\s]", "").ToLower().Split('\t').ToList();

            string inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Input", "ShopData.txt");
            string input = Regex.Replace(File.ReadAllText(inputFilePath).ToLower(), @"[^\w:\s]", "");

            Dictionary<string, string> specialCases = new Dictionary<string, string>()
            {
                {"candyass", "zhetass" }
            };

            foreach(KeyValuePair<string, string> kvp in specialCases)
            {
                input = input.Replace(kvp.Key, kvp.Value);
            }

            List<string> rows = input.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Where(x => Regex.IsMatch(x, @":$")).ToList();
            rows.Insert(1, "");  // The missed shop
            List<Ship> shipShopData = new List<Ship>();
            foreach (string ship in ships)
            {
                shipShopData.Add(new Ship { Name = ship, Appearances = new List<int>() });
            }
            for (int i = 0; i < rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(rows[i]))
                {
                    string[] shopShips = rows[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string ship in shopShips)
                    {
                        shipShopData.Where(x => ship.Contains(x.Name)).First().Appearances.Add(i + 1);
                    }
                }
            }
            int currentShopCount = rows.Count;
            List<Ship> orderedShipShopData = shipShopData.OrderBy(x => x.Appearances.Max()).ToList();

            Console.WriteLine("Last appearances:");
            foreach (Ship s in orderedShipShopData)
            {
                //string appearances = "";
                //foreach (int i in s.Appearances)
                //{
                //    appearances += "," + i.ToString();
                //}
                //appearances = appearances.Trim(',');
                //Console.WriteLine($"{s.Name}: {appearances}");

                Console.WriteLine($"{s.Name}: {currentShopCount - s.Appearances.Max()} shops ago");
            }

            //Console.WriteLine();
            //Console.WriteLine("Ships with 0 appearances");
            //foreach(Ship s in shipShopData.Where(x => x.Appearances.Count == 0))
            //{
            //    Console.WriteLine(s.Name);
            //}
        }
    }

    struct Ship
    {
        public string Name { get; set; }
        public List<int> Appearances { get; set; }
    }
}
