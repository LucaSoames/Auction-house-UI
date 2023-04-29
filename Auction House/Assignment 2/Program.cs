using System;
using System.Text.RegularExpressions;
using System.IO;
//story 1
namespace Assignment2
{
    
    class Program
    {
        static void Main(string[] args)
        {
                AuctionHouse auctionHouse = CsvOps.GetAuctionHouseFromCsv(); //get auctionhouse database from csv file


                string[] options = //options variable
                {
                    "(1) Register",
                    "(2) Sign In",
                    "(3) Exit."
                };

                IDictionary<string, string> dictionary = MainMenuDictionary();//initialise dictionary

                MainMenu menu = new MainMenu(dictionary, options, auctionHouse); //new mainmenu object class
            

                Welcome(); //welcome dialogue
            
                menu.Run(menu.Options); //run mainmenu

                CsvOps.UploadDataToTxt(auctionHouse); //upload all data to database csv 
                Farewell(); //display farewell message
                Environment.Exit(0);
        }

        private static IDictionary<string, string> MainMenuDictionary() //adds keyvalue pairs to dictionary
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add(new KeyValuePair<string, string>("fileName", "AuctionHouseDatabase"));
            dictionary.Add(new KeyValuePair<string, string>("menuName", "Main Menu"));
            dictionary.Add(new KeyValuePair<string, string>("menuLength", "---------"));
            dictionary.Add(new KeyValuePair<string, string>("signInName", "Sign In"));
            dictionary.Add(new KeyValuePair<string, string>("signInLength", "-------"));

            return dictionary;
        }

        static void Welcome() //welcome dialogue
        {
            Console.WriteLine("+------------------------------+");
            Console.WriteLine("| Welcome to the Auction House |");
            Console.WriteLine("+------------------------------+");
        }
        static void Farewell() //farewell dialogue
        {
            Console.WriteLine("Thank you for visiting the Auction House!");
        }
    }
}
