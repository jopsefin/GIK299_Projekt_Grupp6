using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Projekt_G6
{
    class Program
    {
        static List<BlogPost> posts = new List<BlogPost>();
        static List<BlogPost> objs = new List<BlogPost>();
        static void Main(string[] args)
        {
            Console.WriteLine("Hej och välkommen till Blogg-verktyget! ");
            Console.WriteLine("Välj i menyn vad du vill göra genom att ange siffran för ditt val följt av enter.");
 
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

            CreatePost();

            //Tar alla blogposter och konverterar till json för att sedan spara det till filen.
            var jsonData = ConvertToJson(posts);
            WriteAllText(jsonData);
            ReadJsonFromFile();
        }
        private static void BlogpostList()
        {   

            if(objs.Count > 0)
            {
                ReadJsonFromFile();
            }
            
            //Om posts är tom, dvs man har inte skapat några inlägg, får man meddelande om det.
            //Annars skrivs alla tidigare inlägg ut.
            if (posts.Count == 0)
            {
                Console.WriteLine("Du har inga sparade inlägg. ");
            }
            else
            {
                posts.ForEach(Console.WriteLine);
            }
        }
        private static void BlogpostSearch()
        {
            string search = Console.ReadLine();

            int i = posts.FindIndex(x => x.Title.Contains(search));
            Console.WriteLine(posts[i]);
        }
        private static void CreatePost()
        {
            Console.Clear();

            BlogPost post = new BlogPost();

            DateTime date = DateTime.Now;

            post.Date = date;

            Console.WriteLine("Datum: {0}", date);

            Console.Write("Ange rubrik: ");
            post.Title = Console.ReadLine();

            Console.WriteLine("Inlägg: ");
            post.Content = Console.ReadLine();

            posts.Add(post);

            Console.WriteLine(post);
        }
        //Skapar upp filen om den inte finns, annars läser vad som finns i den.
        static void CreateOrReadFile()
        {
            string path = "SavedBlogPosts.json";

            if (!File.Exists(path))
            {
                using (FileStream fs = File.Create(path))
                {
                    fs.Close();
                }
            }
            else
            {
                ReadJsonFromFile();

                //Addar innehållet från filen till posts.
                if (posts.Count == 0) 
                {
                    objs.ForEach(posts.Add);
                }
            }
        }

        //Inspiration till Json-serialisering och -deserialisering kommer från Föreläsning 11 med Thomas.
        //Läser filen och lägger in inläggen i listan objs.
        static void ReadJsonFromFile()
        {
            string path = "SavedBlogPosts.json";

            string json = File.ReadAllText(path);

            objs = JsonSerializer.Deserialize<List<BlogPost>>(json);
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
    }
}