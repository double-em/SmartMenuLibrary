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
        private string menuName = "";
        private string menuDescription = "";
        private List<string> menuList = new List<string>();
        private List<string> menuID = new List<string>();
        private List<string> errors = new List<string>();
        private bool pathSet = false;
        public bool langSet = false;


        public void LoadMenu(string path)
        {
            string lang = "e";
            SetErrors(lang);
            while (!langSet)
            {
                // Valg af sprog
                Console.Clear();
                Console.WriteLine("Choose/vælg E for english or/eller D for dansk menu");
                lang = Console.ReadLine();
                lang = lang.ToLower();
                SetErrors(lang);

                if (lang == "e")
                {
                    string[] temppath = path.Split('.');
                    path = temppath[0] + "EN" + "." + temppath[1];
                    break;
                }
                else if (lang == "d")
                {
                    string[] temppath = path.Split('.');
                    path = temppath[0] + "DA" + "." + temppath[1];
                    break;
                }

                else
                {
                    Console.WriteLine(errors[1]);
                    Console.ReadKey(true);
                }
            }

            // Henter tekstfilen fra path og laver strings ud fra Navn og Beskrivelse
            path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\" + path));
            if (File.Exists(path))
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
        void SetErrors(string lang)
        {
            string pathErrors = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\MenuErrors.txt"));
            string[] temppathErrors = pathErrors.Split('.');

            if (lang == "d")
            {
                pathErrors = temppathErrors[0] + "DA" + "." + temppathErrors[1];
            }
            else
            {
                pathErrors = temppathErrors[0] + "EN" + "." + temppathErrors[1];
            }

            if (File.Exists(pathErrors))
            {
                foreach (string item in File.ReadAllLines(pathErrors))
                {
                    errors.Add(item);
                }
            }
            else
            {
                Console.WriteLine(pathErrors + " could not be found...");
                Console.ReadKey(true);
            }
        }
        public string MenuID(int iDNumber)
        {
            return menuID[iDNumber];
        }
        public string MenuList(int listNumber)
        {
            return menuList[listNumber];
        }
        public string getmenuDescription()
        {
            return menuDescription;
        }
        public string getmenuName()
        {
            return menuName;
        }
    }
}

