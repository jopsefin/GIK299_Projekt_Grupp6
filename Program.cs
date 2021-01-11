using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
namespace Projekt_G6
{
    class Program
    {
        static List<BlogPost> Posts;
        static string path = "SavedBlogPosts.json";
        static void Main(string[] args)
        {   
            Console.Clear();
            Console.WriteLine("Hej och välkommen till Grupp 6 Blogg-verktyg! ");
            Console.WriteLine("Detta verktyg har skapats av Edvin Owetz, Josefin Karlsson och Mikael Olsson.");
            Console.WriteLine("Det har gjorts som en del av kursen GIK299.\n");
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
                Console.Write("Ange val: ");
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
                        Console.WriteLine("-------------------FEL MENYVAL----------------------");
                        Console.WriteLine("  Du måste ange en siffra mellan 1-4, försök igen! ");
                        Console.WriteLine("----------------------------------------------------\n");
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
            //Så länge användaren matar in något kommer det addas till content tillsammans med \n.
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

            Console.WriteLine("--------NYTT INLÄGG---------");
            Console.WriteLine("  Ditt inlägg är nu skapat! ");
            Console.WriteLine("----------------------------");
        }
        private static void BlogpostList()
        {   
            Console.Clear();
            
            //Om posts är tom, dvs man har inte skapat några inlägg, får man meddelande om det.
            //Annars skrivs alla tidigare inlägg ut.
            if (Posts.Count == 0)
            {
                Console.WriteLine("--------VISA INLÄGG---------");
                Console.WriteLine(" Du har inga sparade inlägg ");
                Console.WriteLine("----------------------------\n");
                
            }
            else
            {
                Console.WriteLine("--------VISA INLÄGG---------");
                Console.WriteLine("  Här är alla dina inlägg ");
                Console.WriteLine("----------------------------\n");
                Posts.ForEach(Console.WriteLine);
            }
        }
        private static void BlogpostSearch()
        {
            Console.Clear();
            Console.WriteLine("-----------SÖKA EFTER INLÄGG----------");
            Console.WriteLine(" Sök efter inlägg med hjälp av rubrik ");
            Console.WriteLine("--------------------------------------\n");
            Console.Write("Ange sökord: ");
            string search = Console.ReadLine();

            //Sätter search samt Title till versaler så att sökningen inte är case sensitive.
            //Alla inlägg vars rubrik innehåller sökordet läggs till i listan searchresult.
            //Finns inget inlägg med sökordet, får man meddelande om detta. Annars listas alla inlägg vars rubrik innehåller sökordet.
            List<BlogPost> searchresult = Posts.FindAll(x => x.Title.ToUpper().Contains(search.ToUpper()));
            
            if(searchresult.Count == 0)
            {
                Console.WriteLine("\n------------SÖKRESULTAT--------------");
                Console.WriteLine("  Inga rubriker innehåller sökordet ");
                Console.WriteLine("-------------------------------------\n");
            }
            else 
            {
                bool num = true;
                
                while(num)
                {
                    Console.WriteLine("\n------------SÖKRESULTAT--------------\n");

                    int index = 1;
                    foreach (BlogPost x in searchresult)
                    {
                        Console.WriteLine("{0} - {1}", index, x.Title);
                        index++;
                    }

                    int choice;
                
                    Console.WriteLine("\nVälj vilket inlägg du vill titta på genom att ange siffran framför inlägget följt av enter eller välj 0 för att gå tillbaka till huvudmenyn: ");
                    
                    if (Int32.TryParse(Console.ReadLine(), out choice))
                    {
                        if ((choice > 0 && choice < searchresult.Count+1 ))
                        {
                            Console.Clear();
                            Console.WriteLine("Här är inlägget du valde att titta på:\n{0}", searchresult[choice-1]);
                        }
                        else if(choice == 0) 
                        {
                            Console.Clear();
                            num = false;
                        }
                        else 
                        {
                            Console.WriteLine("Du måste göra ett giltigt val. Försök igen.");   
                        }        
                    }
                    else
                    {
                        Console.WriteLine("Du måste göra ett giltig val. Försök igen.");
                    }
                }
            }
        }
        //Skapar upp filen om den inte finns, annars läser vad som finns i den och lägger till i Posts.
        private static void CreateOrReadFile()
        {
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
        private static List<BlogPost> ReadJsonFromFile()
        {
            string json = File.ReadAllText(path);
            
            return JsonSerializer.Deserialize<List<BlogPost>>(json);
        }
        //Serialiserar till Json
        private static string ConvertToJson(List<BlogPost> data)
        {
            string json = JsonSerializer.Serialize(data);
            return json;
        }
        //Skriver till fil.
        private static void WriteAllText(string text)
        {
            File.WriteAllText(path, text);
        }
        //Tar innehållet i Posts, konverterar till jsonData och skriver till filen.
        private static void SavePostsToFile()
        {
            var jsonData = ConvertToJson(Posts);
            WriteAllText(jsonData);
        }
    }
}