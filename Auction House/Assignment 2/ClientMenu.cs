using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Assignment2
{
    class ClientMenu : MainMenu
    {
        const int advertiseProduct = 0, viewProduct = 1, searchProducts = 2, viewBids = 3, viewItems = 4, exit = 5; //set constants for switch statement

        private Client currentUser; // set current user field

        public ClientMenu(IDictionary<string, string> strings, string[] options, AuctionHouse auctionHouse, Client currentUser) : base(strings, options, auctionHouse) //inheret main menu fields
        {
            this.currentUser = currentUser; //set current user in constructor
        }
        public void ClientMenuUI() //client menu initialisation
        {
            Run(Options); //run options for client menu
        }

        public override void Run(string[] options) //override Run for main menu
        {
            while (true)
            {
                int choice = Menu(options); //get user response to option


                if (choice == exit) break; //if user wishes to log off break out of nested loop - client menu, return to main menu

                Process(choice); //sent choice variable to switch statement controller.

            }
        }

        public int Menu(string[] options)
        {
            while (true) //trap program/user in while loop
            {
                ShowMenu(options); //show options to user
                int option;

                if (GetOpt(out option, 1, options.Length)) //get option
                {
                    return option - 1; //return int value -1 (parsing)
                }
            }
        }

        public void ShowMenu(string[] options)
        {
            //display dialogue
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
            //get user option as an out variable
            string userInput = Console.ReadLine(); //read userinput to string

            if (userInput == null)
            {
                // End of input reached
                option = exit + 1;
                return false;
            }

            return int.TryParse(userInput, out option) //try parse user input to int
                && option >= low
                && option <= high;
        }

        private void Process(int choice) //switch case controller, implements constants
        {
            switch (choice)
            {
                case advertiseProduct:
                    AdvertiseProduct();
                    break;
                case viewProduct:
                    ViewMyProducts();
                    break;
                case searchProducts:
                    SearchProducts();
                    break;
                case viewBids:
                    ViewBids();
                    break;
                case viewItems:
                    ViewItems();
                    break;
                case exit:
                    break;
                default:
                    break;
            }
        }

        public void AdvertiseProduct() //user can advertise a product
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine($"Product Advertisement for {currentUser.ClientSignUp.Name}({currentUser.ClientSignUp.Email})"); //display dialogue for currentUsers information
                // call functions to get necessary product variables
                string productName = GetName();
                string productDescription = GetDescription(productName);
                string productPrice = GetPrice();

                //check if first ad
                if (Client.IsFirstAd(currentUser) == true)
                {
                    currentUser.Adverts[0] = new Advert(productName, productDescription, productPrice, "-"); //overwrite default ad
                }
                else
                {
                    currentUser.Adverts.Add(new Advert(productName, productDescription, productPrice, "-"));
                }
                Console.WriteLine();
                Console.WriteLine($"Successfully added product {productName} {productDescription} {productPrice}");

                break;
            }

        }

        private static string GetDescription(string productName)
        {
            while (true) //trap in loop
            {
                Console.WriteLine();
                Console.WriteLine("Product description");
                Console.Write(">");
                string userInput = Console.ReadLine(); //get user input
                //if statements to check if input is correct
                if (userInput == null)
                {
                    Console.WriteLine("     Product description must be non-blank text string.");
                }
                else if (userInput == productName)
                {
                    Console.WriteLine("     Product description must be different from the product name");
                }
                else
                {
                    return userInput;
                }
            }
        }

        private static string GetName()
        {
            while (true) //trap in loop
            {
                Console.WriteLine();
                Console.WriteLine("Product Name");
                Console.Write(">");
                string userInput = Console.ReadLine(); //get user input
                //check if correct input
                if (Testing.IsTextString(userInput) == true)
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

        private static string GetPrice()
        {
            while (true)//trap in loop
            {
                Console.WriteLine();
                Console.WriteLine("Product price ($d.cc):");
                Console.Write(">");
                string userInput = Console.ReadLine(); //get user input
                //check if correct input
                if (Testing.IsCurrency(userInput))
                {
                    return userInput;

                }
                else
                {
                    Console.WriteLine("     Invalid Input: A currency value is requred e.g. $54.95, $9.99, $2314.15.");
                }
            }

        }

        private static void HeaderYourAds(Client client)
        {
            Console.WriteLine();
            Console.WriteLine($"Product List for {client.ClientSignUp.Name}({client.ClientSignUp.Email})"); //dialogue for user
            Console.WriteLine("------------------------------------------------");
        }

        private static void HeaderAds() // header ads with console.writeline
        {
            Console.WriteLine("| Item # | Product name | Description | List price | Bidder name | Bidder email | Bid amt |");
        }
        private void ViewMyProducts()
        {

            HeaderYourAds(currentUser); //dialogue
            if (Client.IsFirstAd(currentUser) == true) //check for first ad
            {
                Console.WriteLine();
                Console.WriteLine("You have no advertised products at the moment.");
            }
            else
            {
                ProductsListed(); //call function
            }
        }



        private void ProductsListed()
        {
            Console.WriteLine();
            HeaderAds(); // display header
            int counter = 0; //user counter for item #
            for (int i = 0; i < currentUser.Adverts.Count; i++) //counter controlled loop
            {
                string[] bidInfo = { "-", "-", "-" }; //default displayed bid info
                Advert currentAd = currentUser.Adverts[i]; //get advert
                if (auctionHouse.IsPurchasedProduct(currentAd, currentUser) == false) //check if purchased product
                {
                    counter++; //increment
                    List<Bid> currentAdBids = auctionHouse.FindBids(currentAd.Name); // get bid list
                    if (currentAdBids.Any() == true) //check if empty
                    {
                        Bid topBid = Bid.TopBid(currentAdBids); //get topbid
                        bidInfo = new string[] { auctionHouse.GetClient(topBid.BidderEmail).ClientSignUp.Name,
                         topBid.BidderEmail,
                         topBid.BidAmount };
                    }
                    Console.WriteLine($"{counter} {currentAd.Name} {currentAd.Description} {currentAd.Price}" +
                        $" {bidInfo[0]} {bidInfo[1]} {bidInfo[2]}"); //display line
                }

            }
        }

        private void SearchProducts()
        {
            // ALL shows everything in system
            // doesnt show current Users products\
            Console.WriteLine();
            Console.WriteLine("Product search for {0}({1})", currentUser.ClientSignUp.Name, currentUser.ClientSignUp.Email);
            Console.WriteLine("-------------------------------------------------");
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Please supply a search phrase (ALL to see all products)");
                Console.Write(">");
                string userInput = Console.ReadLine(); //get user input 

                List<Client> clients = auctionHouse.GetClientList(); //get auctionhouse clients
                Console.WriteLine();
                Console.WriteLine("Search results");
                Console.WriteLine("----------------------");
                HeaderAds(); //header
                //initialise variables
                string[] productInfo;
                List<string[]> strings = new List<string[]>();
                for (int i = 0; i < clients.Count; i++) //counter controlled loop
                {
                    if (clients[i] != currentUser)
                    {
                        for (int j = 0; j < clients[i].Adverts.Count; j++) //nested loop for adverts
                        {
                            string[] bidInfo = { "-", "-", "-" };
                            //check
                            Advert currentAd = clients[i].Adverts[j]; // get current ad
                            if (currentAd.Name != "-")
                            {
                                if (auctionHouse.IsPurchasedProduct(currentAd, clients[i]) == false) //check for purchased product
                                {
                                    List<Bid> currentAdBids = auctionHouse.FindBids(currentAd.Name); //get bid list
                                    if (currentAdBids.Any() == true) //check if empty
                                    {
                                        Bid topBid = Bid.TopBid(currentAdBids);
                                        bidInfo = new string[] { auctionHouse.GetClient(topBid.BidderEmail).ClientSignUp.Name, topBid.BidderEmail, topBid.BidAmount }; //bid info dto created
                                    }
                                    string[] adInfo = { currentAd.Name, currentAd.Description, currentAd.Price };
                                    adInfo = adInfo.Concat(bidInfo).ToArray();
                                    strings.Add(adInfo); //add all to strings list
                                }
                            }
                        }
                    }
                }
                //Sort List Array by alphabetical for the first 3 columns
                strings = strings
                    .OrderBy(arr => arr[0])
                    .ThenBy(arr => arr[1])
                    .ThenBy(array => array[2])
                    .ToList();
                // Search key word all
                if (userInput == "ALL")
                {
                    for (int i = 0; i < strings.Count(); i++) //display all items in strings
                    {
                        string[] adInfo = strings[i];
                        Console.WriteLine($"{i + 1} {adInfo[0]} {adInfo[1]} {adInfo[2]} {adInfo[3]} {adInfo[4]} {adInfo[5]}");
                    }
                    BidOnItem(strings); // go to bid interface
                    break;
                }
                // are queried to see if the search phrase appears in either product name or product description.
                else if (Testing.IsTextString(userInput) == true) //check for text string
                {
                    List<string[]> items = new List<string[]>();
                    for (int i = 0; i < strings.Count(); i++)
                    {
                        string[] adInfo = strings[i];

                        if (adInfo[0].Contains(userInput) || adInfo[1].Contains(userInput)) //check if search terms are contained
                        {
                            items.Add(adInfo);
                            Console.WriteLine($"{i + 1} {adInfo[0]} {adInfo[1]} {adInfo[2]} {adInfo[3]} {adInfo[4]} {adInfo[5]}");

                        }
                    }
                    BidOnItem(items); //used items list for bid interface
                    break;
                }
                else
                {
                    Console.WriteLine("invalid search phrase.");
                }
            }
        }

        private void BidOnItem(List<string[]> strings)
        {
            while (true) //trap in loop 
            {
                Console.WriteLine();
                Console.WriteLine("Would you like to place a bid on any of these items (yes or no)?");
                Console.Write(">");
                string userInput = Console.ReadLine();  // get user input
                if (userInput == "yes" || userInput == "no")
                {
                    if (userInput == "yes")
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Please enter a non-negative integer between 1 and {strings.Count}:");
                        Console.Write(">");
                        userInput = Console.ReadLine(); //get user input
                        int userInt;
                        if (int.TryParse(userInput, out userInt)) // parse to int
                        {
                            if (userInt >= 1 && userInt <= strings.Count)
                            {
                                string[] adInfo = strings[userInt - 1]; //index strings list for ad info string[] dto
                                PlaceBid(adInfo); //enter place bid dialogue
                                break;

                            }
                            else
                            {
                                Console.WriteLine($"        Input must be a non-negative integer between 1 and {strings.Count}:");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"        Input must be a non-negative integer between 1 and {strings.Count}:");
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("     Invalid input");
                }
            }
        }

        public void PlaceBid(string[] adInfo)
        {
            while (true) //trap in loop
            {

                if (adInfo[5] == "-")  // default value
                {
                    adInfo[5] = "$0.00";
                }
                Console.WriteLine();
                Console.WriteLine($"Bidding for {adInfo[0]} (regular price {adInfo[2]}), current highest bid {adInfo[5]}");

                Console.WriteLine();
                Console.WriteLine("How much do you bid?");
                Console.Write(">");
                string userInput = Console.ReadLine();
                if (Testing.IsCurrency(userInput)) //test if currency value for user input
                {
                    //parse bid currency values to decimals
                    decimal userInputDec = Decimal.Parse(userInput.Remove(0, 1));
                    decimal topBidDec = Decimal.Parse(adInfo[5].Remove(0, 1));
                    if (userInputDec > topBidDec) { //check if userinput is greater than current top bit
                        //overrite top bit indo
                        string[] bidInfo = { currentUser.ClientSignUp.Email, userInput, adInfo[0] };
                        Console.WriteLine();
                        Console.WriteLine($"Your bid of {bidInfo[1]} for {bidInfo[2]} is placed.");
                        DeliveryInstructions(bidInfo); //get delivery instructions
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"      Bid must be greater than the current top bid {adInfo[5]}");
                    }
                }
                else
                {
                    Console.WriteLine("     Invalid input");
                }
            }

        }
        const int clickAndCollect = 0, homeDelivery = 1;
        public void DeliveryInstructions(string[] bidInfo)
        {

            while (true) //trap in loop 
            {
                Console.WriteLine();
                Console.WriteLine("Delivery Instructions");
                Console.WriteLine("---------------------");
                Console.WriteLine("(1) Click and collect");
                Console.WriteLine("(2) Home Delivery");
                Console.WriteLine();
                Console.WriteLine("Please select an option between 1 and 2");
                Console.Write(">");

                string userInput = Console.ReadLine(); //get user input
                int userInt;
                if (int.TryParse(userInput, out userInt)) //try parse user input
                {
                    userInt = userInt - 1;
                    if (userInt == 0 || userInt == 1)
                    {
                        ProcessDelivery(userInt, bidInfo); //enter delivery controller
                        break;
                    }
                    else
                    {
                        Console.WriteLine("     Input must be an integer between 1 and 2");
                    }
                }
                else
                {
                    Console.WriteLine("     Input must be an integer between 1 and 2");
                }
            }
        }

        private void ProcessDelivery(int choice, string[] bidInfo) //switch statement to control user input
        {
            switch (choice)
            {
                case clickAndCollect:
                    ClickAndCollect(bidInfo);
                    break;
                case homeDelivery:
                    HomeDelivery(bidInfo);
                    break;
                default:
                    break;
            }
        }

        private void AddBid(string[] bidInfo, House house, string[] deliveryInfo) // add bid 
        {
            currentUser.Bids.Add(new Bid(bidInfo[0], bidInfo[1], bidInfo[2], house, deliveryInfo)); //new bid for current user based on info
            Bid recentBid = currentUser.Bids[currentUser.Bids.Count - 1]; //get recent bid
            Advert ad = auctionHouse.FindAdvert(recentBid); //get ad
            if (auctionHouse.IsTopBid(recentBid, ad) == true) //check if top bid
            {
                if (ad.Bid != "-") //check default
                {
                    auctionHouse.GetClient(ad.Bid).Bids.Remove(auctionHouse.FindBid(ad.Bid, ad.Name)); //get client from bid then remove previous bid 
                }
                ad.Bid = bidInfo[0]; //new bid override
            }
        }
        public void ClickAndCollect(string[] bidInfo)
        {
            while (true) //trap in loop 
            {
                DateTime deliveryStart; //delivery start datetime variable
                Console.WriteLine();
                Console.WriteLine("Delivery window start (dd/mm/yyyy hh:mm)");
                Console.Write(">");
                string userInput = Console.ReadLine();
                DateTime currentTime = DateTime.Now;
                string[] deliveryInfo = new string[2];
                if (DateTime.TryParseExact(userInput, "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out deliveryStart)) //try parse datetime variable culture info = local
                {
                    if (Testing.Is1HourTimeSpan(currentTime, deliveryStart)) //test if current time is atleast an hour from proposed collection time
                    {
                        //get end of delivery window
                        deliveryInfo[0] = userInput; 
                        Console.WriteLine();
                        Console.WriteLine("Delivery window end (dd/mm/yyyy hh:mm))");
                        Console.Write(">");
                        userInput = Console.ReadLine();
                        DateTime deliveryEnd;
                        if (DateTime.TryParseExact(userInput, "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out deliveryEnd)) //try parse datetime
                        {
                            if (Testing.Is1HourTimeSpan(deliveryStart, deliveryEnd)) //check 1 hour timespan
                            {

                                deliveryInfo[1] = userInput;
                                string[] startInfo = deliveryInfo[0].Split(' ');
                                string[] endInfo = deliveryInfo[1].Split(' ');
                                Console.WriteLine();
                                Console.WriteLine("Thank you for your bid. If successful" +
                                    $" the item will be provided via collection between {startInfo[1]} on {startInfo[0]} and {endInfo[1]} on {endInfo[0]}.");
                                House house = new House(0, 0, "-", "-", "-", "-", 0); // default house
                                AddBid(bidInfo, house, deliveryInfo); //addbid
                                break;
                            }
                            else
                            {
                                Console.WriteLine("     Delivery window start must be at least one hour in the future.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("     Please enter a valid data and time.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("     Delivery window start must be at least one hour in the future.");
                    }
                }
                else
                {
                    Console.WriteLine("     Please enter a valid data and time.");
                }



            }
        }

        public void HomeDelivery(string[] bidInfo)
        {
            //get home delivery info
            int unitNumber = MainMenu.GetUnitNumber();
            int streetNumber = MainMenu.GetStreetNumber();
            string streetName = MainMenu.GetStreetName();
            string streetSuffix = MainMenu.GetStreetSuffix();
            string city = MainMenu.GetCity();
            string state = MainMenu.GetState();
            int postcode = MainMenu.GetPostcode();

            Console.WriteLine();
            Console.WriteLine("Thank you for your bid. If successful" +
    $" the item will be provided via Delivery to {unitNumber}/{streetNumber} {streetName} {streetSuffix}, {city} {state} {postcode}.");

            House house = new House(unitNumber, streetNumber, streetName, streetSuffix, city, state, postcode);
            string[] deliveryInfo = { "-", "-" };
            AddBid(bidInfo, house, deliveryInfo); //add bid
        }
    

        private void ViewBids()
        {
            Console.WriteLine();
            Console.WriteLine($"List Product Bids for {currentUser.ClientSignUp.Name}({currentUser.ClientSignUp.Email})");
            Console.WriteLine("-------------------------------------------------------");
            List<Advert> itemsWithBids = new List<Advert>(); //list of items which have bids placed on them instantiated
            foreach (Advert ad in currentUser.Adverts)
            {
                if (ad.Bid != "-" && auctionHouse.IsPurchasedProduct(ad, currentUser) == false) //check if isnt default bid value nor is a purchased product
                {
                    itemsWithBids.Add(ad); //add to list to be displayed
                } 
            }
            if(itemsWithBids.Count == 0) //check if list has any items
            {
                Console.WriteLine("     No bids were found");
            }
            else
            {
                Console.WriteLine();
                HeaderAds(); //header
                for (int i = 0; i < itemsWithBids.Count; i++)
                {
                    //for each item get information of client and bid
                    string topBidEmail = itemsWithBids[i].Bid;
                    Client bidder = auctionHouse.GetClient(topBidEmail);
                    Bid bid = auctionHouse.FindBid(topBidEmail, itemsWithBids[i].Name);
                    Console.WriteLine($"{i+1} {itemsWithBids[i].Name} {itemsWithBids[i].Description} {itemsWithBids[i].Price} {bidder.ClientSignUp.Name} {topBidEmail} {bid.BidAmount}"); //dialogue
                }
                Console.WriteLine();
                Console.WriteLine("Would you like to sell something (yes or no)?");
                Console.Write(">");
                string userInput = Console.ReadLine();
                if (userInput == "yes" || userInput == "no")
                {
                    if (userInput == "yes")
                    {
                        SellItem(itemsWithBids); //enter sell item dialogue
                    }
                }
                else
                {
                    Console.WriteLine("     Invalid input");
                }
            }


        }

        public void SellItem(List<Advert> adverts) //sell item based on users list of adverts 
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine($"Please enter an integer between 1 and {adverts.Count}:");
                Console.Write(">");
                string userInput = Console.ReadLine();
                int userInt;
                if (int.TryParse(userInput, out userInt)) //parse user input to int
                {
                    if (userInt >= 1 && userInt <= adverts.Count)
                    {
                        Advert selectedAd = adverts[userInt-1];
                        PostItem(selectedAd); //process sale
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"      Input must be a non-negative integer between 1 and {adverts.Count}:");
                    }
                }
                else
                {
                    Console.WriteLine($"      Input must be a non-negative integer between 1 and {adverts.Count}:");
                }
            }
        }

        public void PostItem(Advert advert)
        {
            //get buyer client info
            Client buyer = auctionHouse.GetClient(advert.Bid);
            Bid bid = auctionHouse.FindBid(buyer.ClientSignUp.Email, advert.Name);
            //successfull sale
            Console.WriteLine();
            Console.WriteLine($"You have sold {advert.Name} to {buyer.ClientSignUp.Name} for {bid.BidAmount}."); 
            currentUser.Adverts.Remove(advert); //remove ad from current user
            buyer.Adverts.Add(advert); //add to buyer client adverts
        }
        private void ViewItems()
        {
            Console.WriteLine();
            Console.WriteLine($"Purchased Items for {currentUser.ClientSignUp.Name}({currentUser.ClientSignUp.Email})");
            Console.WriteLine("-----------------------------------------------------------------");
            List<Advert> purchasedProducts = new List<Advert>(); //get purchased products list
            foreach(Advert advert in currentUser.Adverts)
            {
                if(auctionHouse.IsPurchasedProduct(advert, currentUser)) // is purchased product?
                {
                    purchasedProducts.Add(advert); //add to list 
                }
            }
            Console.WriteLine();
            //display lsit
            if (purchasedProducts.Count > 0) // if list isn't empty
            {
                HeaderAds(); //header
                for (int i = 0; i < purchasedProducts.Count; i++) //counter controlled loop
                {
                    //get info
                    Advert product = purchasedProducts[i]; 
                    Bid bidInfo = auctionHouse.FindBid(product.Bid, product.Name);
                    Console.WriteLine($"{i+1} {product.Name} {product.Description} {product.Price} {bidInfo.BidAmount} {Testing.DeliveryType(bidInfo)}"); //display ads
                }
            }
            else
            {
                Console.WriteLine("You have no purchased products at the moment.");
            }
        }

    }
}
