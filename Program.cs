using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
namespace Projekt_G6
{
    class Program
    {
        static List<BlogPost> Posts;
        static void Main(string[] args)
        {
            Console.WriteLine("Hej och välkommen till Blogg-verktyget! ");
            Console.WriteLine("Välj i menyn vad du vill göra genom att ange siffran för ditt val följt av enter.\n ");
 
            CreateOrReadFile();

            MainMenu();
        }
        private static void MainMenu()
        {
            bool menu = true;
            while (menu)
            {
                Console.WriteLine("\n-------------MENY-------------");
                Console.WriteLine("1 - Skapa nytt blogginlägg ");
                Console.WriteLine("2 - Se tidigare blogginlägg ");
                Console.WriteLine("3 - Sök tidigare blogginlägg ");
                Console.WriteLine("4 - Avsluta programmet ");
                Console.WriteLine("------------------------------\n");
                string menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case "1":
                        Blogpost();
                        break;
                    case "2":
                        BlogpostList();
                        break;
                    case "3":
                        BlogpostSearch();
                        break;
                    case "4":
                        SavePostsToFile();
                        Console.WriteLine("Avslutar programmet - Tack och hej!");
                        menu = false;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Du måste ange en siffra mellan 1-4, försök igen! ");
                        break;
                }
            }
        }
        private static void Blogpost()
        {
            Console.Clear();
            Console.WriteLine("\n-----------------------------------------------------------------INSTRUKTIONER-----------------------------------------------------------------");
            Console.WriteLine("- Datum sätts automatiskt med dagens datum och nuvarande tid. ");
            Console.WriteLine("- Ange rubrik och tryck enter. ");
            Console.WriteLine("- Skriv ditt inlägg. OBS! För att skapa en radbrytning, tryck enter en gång. För nytt stycke, tryck enter en gång, mellanslag och enter igen. ");
            Console.WriteLine("- För att avsluta inlägget, tryck enter två gånger. ");
            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------- \n");
            BlogPost post = new BlogPost();

            DateTime date = DateTime.Now;

            post.Date = date;

            Console.WriteLine("Datum: {0}", date);

            Console.Write("Ange rubrik: ");
            post.Title = Console.ReadLine();

            Console.WriteLine("Inlägg: ");

            //Funktion för radbrytning och nytt stycke.
            //Så länge användaren matar in något kommer det addas till string content.
            //För att få radbrytning trycker användaren enter en gång = ny rad.
            //För att göra nytt stycke, kan man ange t.ex. mellanslag följt av enter = ny tom rad.
            //För att avsluta inlägget gör man enter två gånger (dvs, String.IsNullOrEmpty blir sant) 
            string line;
            string content = "";
            while(!String.IsNullOrEmpty(line = Console.ReadLine()))
            {
                content = content + "\n" + line; 
            }

            post.Content = content;

            Posts.Add(post);

            Console.WriteLine("Ditt inlägg är nu skapat. ");
        }
        private static void BlogpostList()
        {   
            Console.Clear();
            
            //Om posts är tom, dvs man har inte skapat några inlägg, får man meddelande om det.
            //Annars skrivs alla tidigare inlägg ut.
            if (Posts.Count == 0)
            {
                Console.WriteLine("Du har inga sparade inlägg. ");
            }
            else
            {
                Console.WriteLine("Här är alla dina inlägg.\n ");
                Posts.ForEach(Console.WriteLine);
            }
        }
        private static void BlogpostSearch()
        {
            Console.Clear();
            Console.WriteLine("Sök efter inlägg med hjälp av rubrik. ");
            Console.Write("Ange rubik: ");
            string search = Console.ReadLine();

            List<BlogPost> searchresult = Posts.FindAll(x => x.Title.ToUpper().Contains(search.ToUpper()));
            
            if(searchresult.Count == 0)
            {
                Console.WriteLine("Hittar inget sådant inlägg. ");
            }
            else 
            {
                searchresult.ForEach(Console.WriteLine);
            }
        }
        //Skapar upp filen om den inte finns, annars läser vad som finns i den.
        static void CreateOrReadFile()
        {
            string path = "SavedBlogPosts.json";

            if (!File.Exists(path))
            {
                Posts = new List<BlogPost>();
                using (FileStream fs = File.Create(path))
                {
                    fs.Close();
                }
            }
            else
            {
                Posts = ReadJsonFromFile();
            }
        }

        //Inspiration till Json-serialisering och -deserialisering kommer från Föreläsning 11 med Thomas.
        //Läser filen och lägger in inläggen i listan objs.
        static List<BlogPost> ReadJsonFromFile()
        {
            string path = "SavedBlogPosts.json";

            string json = File.ReadAllText(path);

            return JsonSerializer.Deserialize<List<BlogPost>>(json);
        }
        //Serialiserar till Json
        static string ConvertToJson(List<BlogPost> data)
        {
            string json = JsonSerializer.Serialize(data);
            return json;
        }
        static void WriteAllText(string text)
        {
            string path = "SavedBlogPosts.json";
            File.WriteAllText(path, text);
        }
        static void SavePostsToFile()
        {
            var jsonData = ConvertToJson(Posts);
            WriteAllText(jsonData);
        }
    }
}