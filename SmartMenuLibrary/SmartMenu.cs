using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SmartMenuLibrary
{
    public class SmartMenu
    {

        // Variabler
        public string menuName = "";
        public string menuDescription = "";
        public List<string> menuList = new List<string>();
        public List<string> menuID = new List<string>();
        public List<string> errors = new List<string>();
        bool pathSet = false;
        bool langSet = false;
        string langSelected;
        public List<string> fileNames = new List<string>();


        public void LoadMenu(bool test = false)
        {
            string path = @"lang\";
            fileNames = Directory.GetFiles(path, "*.txt").Select(Path.GetFileName).ToList();

            while (!langSet && !test)
            {
                if (fileNames.Count > 1)
                {
                    Console.WriteLine("Please choose a language below:\n");
                    for (int i = 0; i < fileNames.Count; i++)
                    {
                        //Udprinter navnet på sproget uden .txt delen
                        Console.WriteLine("\t" + (i + 1) + ". " + fileNames[i].Substring(0, fileNames[i].Length - 4));
                    }
                    Console.Write("\nEnter option from 1 to " + fileNames.Count + ": ");
                    if (int.TryParse(Console.ReadLine(), out int userLangSelect))
                    {
                        langSelected = fileNames[userLangSelect - 1];
                        langSet = true;
                    }
                    else
                    {
                        Console.WriteLine("This is not an option...");
                        Console.ReadKey(true);
                    }
                }
                else if (fileNames.Count == 1)
                {
                    langSelected = fileNames[0];
                    langSet = true;
                }
                else
                {
                    throw new Exception("No files found in the lang directory");
                }
            }

            //Til test only
            while (!langSet && test)
            {
                if (fileNames.Count > 1)
                {
                    langSelected = fileNames[0];
                    langSet = true;
                }
                else if (fileNames.Count == 1)
                {
                    langSelected = fileNames[0];
                    langSet = true;
                }
                else
                {
                    throw new Exception("No files found in the lang directory");
                }
            }


            // Henter tekstfilen fra path og laver strings ud fra Navn og Beskrivelse
            path += langSelected;
            if (File.Exists(path) && SetErrors(langSelected))
            {
                string[] lines = File.ReadAllLines(path);
                menuName = lines[0];
                menuDescription = lines[1];
                for (int i = 2; i < lines.Length; i++)
                {
                    // Opdeler vores string der hvor ";" står i tekstfilen
                    if (lines[i].Contains(";"))
                    {
                        string[] temp = lines[i].Split(';');
                        menuList.Add(temp[0]);
                        menuID.Add(temp[1]);
                    }
                    else continue;
                }

                pathSet = true;
            }
            else
            {
                //Giver en error hvis filen ikke findes
                Console.WriteLine(path + errors[0]);
                Console.ReadKey(true);
            }
        }

        public void Activate(IBindings bindings)
        {
            while (pathSet)
            {
                Console.Clear();
                Console.WriteLine(menuName + "\n"); //Udskriver menuen
                for (int i = 0; i < menuList.Count; i++)
                {
                    Console.WriteLine("\t" + (i + 1) + ". " + menuList[i]);
                }
                Console.Write("\n" + menuDescription + " ");
                if (int.TryParse(Console.ReadLine(), out int input))
                {
                    if (input > 0 && input <= menuList.Count)
                    {
                        // Kalder en binding ud fra menuID
                        bindings.Call(menuID[(input - 1)]);
                    }
                    else if (input == 0)
                    {
                        break;
                    }
                    else
                    {
                        //Giver error hvis menuen ikke findes
                        Console.WriteLine(errors[2]);
                    }
                }
                else
                {
                    //Ugyldigt input
                    Console.WriteLine(errors[1]);
                }

                Console.ReadKey(true);
            }
        }

        //Sæt sprog på Errors
        public bool SetErrors(string lang)
        {
            string path = @"lang\errors\";
            List<string> temp = Directory.GetFiles(path, "*.txt").Select(Path.GetFileName).ToList();
            if (File.Exists(path + lang))
            {
                errors = File.ReadAllLines(path + lang).ToList();
                return true;
            }
            else if (temp.Count > 1)
            {
                errors = File.ReadAllLines(path + temp[0]).ToList();
                return true;
            }
            else
            {
                Console.WriteLine("Error files was not found...");
                return false;
            }
        }
    }
}

