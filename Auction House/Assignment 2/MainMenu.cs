using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace Assignment2
{
    internal class MainMenu : UIArgs
    {
        public AuctionHouse auctionHouse;


        const string fileName = "AuctionHouseDatabase.csv";

        const int register = 0, signIn = 1, exit = 2;

        

        public MainMenu(IDictionary<string, string>strings, string[] options , AuctionHouse auctionHouse) : base (strings, options) //constructor for main menu inherits UIArgs class 
        {
            this.auctionHouse = auctionHouse;
        }


        public virtual void Run(string[] options) //main menu run method
        {
            while (true)
            {
                int choice = Menu(options); //get user input 

                
                if (choice == exit) break; //return to main

                Process(choice); //controller from choice of user

            }
        }

        public int Menu(string[] options)
        {
            while (true) //trap in loop
            {
                ShowMenu(options); //shows options
                int option; 

                if (GetOpt(out option, 1, options.Length)) //gets option from user
                {
                    return option - 1;
                }
            }
        }

        public void ShowMenu(string[] options)
        { 
            //shows menu dialogue
            Console.WriteLine();
            Console.WriteLine($"{StringArgs["menuName"]}");
            Console.WriteLine($"{StringArgs["menuLength"]}");
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{options[i]}");
            }
            Console.WriteLine();
            Console.WriteLine($"Please select an option between 1 and {options.Length}");
            Console.Write(">");
        }

        bool GetOpt(out int option, int low, int high)
        {
            //get option from user
            string userInput = Console.ReadLine();

            if (userInput == null)
            {
                // End of input reached
                option = exit + 1;
                return false;
            }

            return int.TryParse(userInput, out option)
                && option >= low
                && option <= high;
        }

        private void Process(int choice) //controlling switch statement
        {
            switch (choice)
            {
                case register:
                    Register(); 
                    break;
                case signIn:
                    SignIn(); 
                    break;
                case exit:
                    break;
                default:
                    break;
            }
        }

        public void Register() 
        {

            while (true) //trap in while loop
            {
                //call functions to get user info
                string userName = GetName(); //get name call function

                string userEmail = GetEmailAddress();

                string passWord = GetPassword();
                //parse name to class hence Client Class
                ClientSignUp currentUser = new ClientSignUp(userName, userEmail, passWord);
                House newUserHouse = new House(0, 0, "-", "-", "-", "-", 0);
                List<Advert> newUserAdverts = new List<Advert>();
                Advert nullAd = new Advert("-", "-", "-", "-");
                newUserAdverts.Add(nullAd);
                List<Bid> newUserBids = new List<Bid>();
                string[] nullInfo = { "-", "-" };
                Bid nullBid = new Bid("-", "-", "-", newUserHouse, nullInfo);
                newUserBids.Add(nullBid);

                Client newUser = new Client(currentUser, newUserHouse, newUserAdverts, newUserBids);


                auctionHouse.RegisterClient(newUser); // register new client

                Console.WriteLine();
                Console.WriteLine("Client {0}({1}) has successfully registerd at Auction House.", userName, userEmail);

                break;

                //end story 3
            }

        }

        public void SignIn() 
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine($"{StringArgs["signInName"]}");
                Console.WriteLine($"{StringArgs["signInLength"]}");
                Console.WriteLine();
                //call login in functions 
                string signInEmail = SignInEmail();
                Client currentUser = SignInPassword(signInEmail);



                if (Testing.IsFirstSignIn(currentUser) == true) //check first sign in
                {
                    currentUser = GetBillingAddress(currentUser); //get home address information from user if first sign in
                }

                IDictionary<string, string> clientDictionary = ClientMenuDictionary(); // initialise client menu dictionary variable

                string[] clientMenuOptions =
                {
                "(1) Advertise Product",
                "(2) View My Product List",
                "(3) Search For Advertised Products",
                "(4) View Bids On My Products",
                "(5) View My Purchased Items",
                "(6) Log off"
                };

                ClientMenu clientMenu = new ClientMenu(clientDictionary, clientMenuOptions, auctionHouse, currentUser); //client menu constructor
                clientMenu.ClientMenuUI(); //enter client menu UI
                break;
            }

        }

        private static IDictionary<string, string> ClientMenuDictionary() //client menu dictionary intialisation
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add(new KeyValuePair<string, string>("menuName", "Client Menu"));
            dictionary.Add(new KeyValuePair<string, string>("menuLength", "-----------"));

            return dictionary;
        }

        private static Client GetBillingAddress(Client client)
        {
            string name = client.ClientSignUp.Name; //get name
            Console.WriteLine();
            Console.WriteLine($"Personal Details for {name}({client.ClientSignUp.Email})");
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("Please provide your home address.");
            //calls address information functions
            int unitNumber = GetUnitNumber(); 
            int streetNumber = GetStreetNumber();
            string streetName = GetStreetName();
            string streetSuffix = GetStreetSuffix();
            string city = GetCity();
            string state = GetState();
            int postcode = GetPostcode();
            //creates client house from information
            client.House = new House(unitNumber, streetNumber, streetName, streetSuffix, city, state, postcode);

            return client;
        }

        public static int GetUnitNumber() //gets unit number from user though a dialogue
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Unit number(0 = none):");
                Console.Write(">");
                string userInput = Console.ReadLine();
                int unitNumber;
                bool res = int.TryParse(userInput, out unitNumber);
                if (res == false)
                {
                    Console.WriteLine("     Unit number must be a non-negative integer.");
                }
                else if (unitNumber < 0)
                {
                    Console.WriteLine("     Unit number must be a non-negative integer.");
                }
                else
                {
                    return unitNumber;
                    break;
                }


            }
        }

        public static int GetStreetNumber() //gets street number from user though a dialogue
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Street number:");
                Console.Write(">");
                string userInput = Console.ReadLine();
                int streetNumber;
                bool res = int.TryParse(userInput, out streetNumber);
                if (res == false)
                {
                    Console.WriteLine("     Street number must be a positive integer.");
                }
                else if (streetNumber <= 0)
                {
                    Console.WriteLine("     Street number must be a positive integer.");
                }
                else
                {
                    return streetNumber;
                    break;
                }


            }
        }

        public static string GetStreetName() //gets street name from user though a dialogue
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Street Name:");
                Console.Write(">");
                string userInput = Console.ReadLine();

                if (userInput.Length < 1)
                {
                    Console.WriteLine("     Street name must be non-blank text string of arbitrary length");
                }
                else
                {
                    return userInput;
                    break;
                }
            }
        }

        public static string GetStreetSuffix() //gets street suffix from user though a dialogue
        {
            string[] streetSuffixes = { "Av", "Cct", "Cr", "Ct", "Dr",
                "Esp", "Gr", "Hts", "Hwy", "Pde", "Pl", "Rd", "St", "Tce", "Avenue", "Crescent", "Close", "Circus", "Dene", "Drive", "Gardens", "Grove", "Hill", "Lane", "Mead", "Mews", "Place", "Rise", "Row", "Road", "Street", "Square",
"Terrace",  "Vale", "Way", "Wharf", "Boardwalk", "Boulevard", "Circuit", "Court", "Crest", "Drive", "Esplanande", "Highway", "Heights", "Parade", "Parkway", "Plaza", "Point", "Alley", "Park", "Trail", "Circle", "Junction",
            "Center", "Crossing", "Pass"}; //known possible street suffixes
            while (true) //trap in loop
            {
                Console.WriteLine();
                Console.WriteLine("Street Suffix:");
                Console.Write(">");
                string userInput = Console.ReadLine();
                for (int i = 0; i < streetSuffixes.Length; i++)
                {
                    if (String.Equals(userInput, streetSuffixes[i], StringComparison.OrdinalIgnoreCase)) // compare strings to values in array ignoring case
                    {
                        return streetSuffixes[i];
                        break;
                    }
                }
                Console.WriteLine("     Invalid street suffix.");

            }
        }

        public static string GetCity() //gets city from user though a dialogue
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("City:");
                Console.Write(">");
                string userInput = Console.ReadLine();

                if (userInput.Length < 0)
                {
                    Console.WriteLine("     City must be non-blank text string of arbitrary length");
                }
                else
                {
                    return userInput;
                    break;
                }
            }

        }

        public static string GetState() //gets state  from user though a dialogue
        {

            string[] states = { "QLD", "NSW", "VIC", "TAS", "SA", "WA", "NT", "ACT" }; //compare array for states
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("State (ACT, NSW, NT, QLD, SA, TAS, VIC, WA):");
                Console.Write(">");
                string userInput = Console.ReadLine();
                for (int i = 0; i < states.Length; i++)
                {
                    if (String.Equals(userInput, states[i], StringComparison.OrdinalIgnoreCase)) //compares ignoring case
                    {
                        return userInput;
                        break;
                    }
                }
                Console.WriteLine("     Invalid State/Territory.");
            }

        }

        public static int GetPostcode() //gets postcode from user though a dialogue
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Postcode:");
                Console.Write(">");
                string userInput = Console.ReadLine();
                int Postcode;
                bool res = int.TryParse(userInput, out Postcode); //try parse to int
                if (res == false)
                {
                    Console.WriteLine("     Postcode must be an integer between 1000 and 9999 inclusive.");
                }
                else if (Postcode <= 9999 & Postcode >= 1000) // input must be bewteen 1000-9999
                {
                    return Postcode;
                    break;
                }
                else
                {
                    Console.WriteLine("     Postcode must be an integer between 1000 and 9999 inclusive.");
                }
            }
        }

        private static string GetName() //gets name from user though a dialogue
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Please enter you name");
                Console.Write(">");

                string userInput = Console.ReadLine();

                if (Testing.IsTextString(userInput) == true) //check for text string
                {
                    return userInput;
                    break;
                }
                else
                {
                    Console.WriteLine("     The supplied value is not a valid name");
                }
            }

        }

        private static string SignInEmail() //gets email address from user though a dialogue
        {
            while (true)
            {
                Console.WriteLine("Please enter your email address");
                Console.Write(">");
                string userInput = Console.ReadLine();

                if (Testing.IsEmailValid(userInput) == true) //tests using testing code for valid email address
                {
                    return userInput;
                    break;
                }
                else
                {
                    Console.WriteLine("     The supplied value is not a valid email address.");
                }
            }
        }

        private Client SignInPassword(string email) //gets password and returns client from this information
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Please enter your password");
                Console.Write(">");
                string userInput = Console.ReadLine();

                if (auctionHouse.IsPasswordMatch(email, userInput) == true) //check if password matches email address in auctionhouse database
                {
                    Client client = auctionHouse.GetClient(email); //get client from database based on email input string
                    return client;
                    break;
                }
                else
                {
                    Console.WriteLine("Password is invalid");
                }


            }
        }
        private static string GetEmailAddress() // get email address from user 
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Please enter your email address");
                Console.Write(">");
                string userInput = Console.ReadLine();

                if (Testing.IsEmailValid(userInput) == true & !Testing.IsInCsv(userInput)) //check if it isnt already in txt file and matches valid formatting for email address
                {
                    return userInput;
                    break;
                }
                else if (Testing.IsInCsv(userInput) == true)
                {
                    Console.WriteLine("     The supplied address is already in use.");
                }
                else
                {
                    Console.WriteLine("     The supplied value is not a valid email address.");
                }
            }

        }

        private static string GetPassword()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Please choose a password");
                Console.WriteLine("* At least 8 characters");
                Console.WriteLine("* No white space characters");
                Console.WriteLine("* At least one upper-case characters");
                Console.WriteLine("* At least one lower-case characters");
                Console.WriteLine("* At least one digit");
                Console.WriteLine("* At least special character");
                Console.Write(">");


                string userInput = Console.ReadLine();

                if (Testing.IsPasswordValid(userInput) == true) //check if password is valid using testing methods
                {
                    return userInput;
                    break;
                }
                else
                {
                    Console.WriteLine("     The supplied value is not a valid password.");
                }
            }

        }

    }
}
