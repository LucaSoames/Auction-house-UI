using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class CsvOps
    {


        // >>>>>>>>>>>READ IN DATABASE<<<<<<<<<<<<<<<<<<<<<//

        public const string fileName = "AuctionHouseDatabase.txt";

        public static AuctionHouse GetAuctionHouseFromCsv()
        {
            string[] csvLines = readInCsv(); //get csv lines
            List<Client> clientList = new List<Client>(); // instantiate list of clients
            if (csvLines.Length == 0 || csvLines == null || csvLines[0] != "")
            {
                foreach (string line in csvLines)
                {
                    string[] splitValues = line.Split('☺'); //split by class seperator
                    Client client = GetClientFromCsv(splitValues); // get client function method
                    clientList.Add(client); //add each client to lsit
                }
            }
            AuctionHouse auctionHouse = new AuctionHouse(clientList); //create auction house based on list
            return auctionHouse;

        }


        public static string[] readInCsv() // read in csv from file path
        {
            string databasePath = Path.GetFullPath(fileName);

            string[] clients = System.IO.File.ReadAllLines(databasePath);

            return clients;
        }
       
        public static ClientSignUp GetClientInfoFromCsv(string[] values)
        {           
            string[] splitValues = values[0].Split('﹁'); //split csv line based on char

            ClientSignUp clientSignUp = new ClientSignUp(splitValues[0], splitValues[1], splitValues[2]); // client sign up class constructed 
            return clientSignUp;
        }

        public static House GetHouseFromCsv(string[] values)
        {
            string[] splitValues = values[1].Split('﹁');//split csv line based on char

            //unitNumberInput, int streetNumberInput, string streetNameInput, string streetSuffixInput, string cityInput, string stateInput, int postCodeInput all from line 
            //parse int variables
            int unitNumberInput = int.Parse(splitValues[0]);
            int streetNumberInput = int.Parse(splitValues[1]);
            int postCodeInput = int.Parse(splitValues[6]);
            //house created from csv split line
            House clientHouse = new House(unitNumberInput, streetNumberInput, splitValues[2], splitValues[3], splitValues[4], splitValues[5], postCodeInput);
            return clientHouse;
        }

        //Client bidder,string bidAmount, Advert item)


        public static List<Advert> GetAdvertsFromCsv(string[] values)
        {
            List<Advert> adverts = new List<Advert>();

            string[] splitValues = values[2].Split('!'); //split line by !

            foreach(string value in splitValues)
            {
                string[] s = value.Split('﹁'); // split by char
                Advert ad = new Advert(s[0], s[1], s[2], s[3]);
                adverts.Add(ad) ;
                //string nameInput, string descriptionInput, double priceInput, Bid topBid
            }
            return adverts ;
        }

        public static List<Bid> GetBidsFromCsv(string[] values)
        {
            List<Bid> bids = new List<Bid>();
            string[] splitValues = values[3].Split('?'); //split bids string by ?

            foreach (string value in splitValues)
            {
                //split string arrays to get individual bid info from bid list
                string[] ss = value.Split('!'); 
                string[] s = ss[0].Split('﹁');
                string[] bidInfo = new string[] { s[0], s[1], s[2] };
                s = ss[1].Split('﹁');
                //parse necessary variables
                int unitNumberInput = int.Parse(s[0]);
                int streetNumberInput = int.Parse(s[1]);
                string streetNameInput = s[2];
                string streetSuffixInput = s[3];
                string cityInput = s[4];
                string stateInput = s[5];
                int postCodeInput = int.Parse(s[6]);
                //construct house for delivery info
                House deliveryAddressInfo = new House(unitNumberInput, streetNumberInput, streetNameInput, streetSuffixInput, cityInput, stateInput, postCodeInput);

                s = ss[2].Split('﹁'); //split by special char
                string[] deliveryInfo = { s[0], s[1] }; 
                //construct bid
                Bid bid = new Bid(bidInfo[0], bidInfo[1] , bidInfo[2], deliveryAddressInfo, deliveryInfo);
                    bids.Add(bid);
            }
            return bids ;
        }

        public static Client GetClientFromCsv(string[] values)
        {
            Client client = new Client(GetClientInfoFromCsv(values), GetHouseFromCsv(values), GetAdvertsFromCsv(values), GetBidsFromCsv(values));
            return client;
        }


        //>>>>>>>>>>>UPLOAD TO DATABASE>>>>>>>>>>>//

        //Append signUpInfo
        public static void UploadDataToTxt(AuctionHouse auctionHouse)
        {
            //upon log exiting program
            List<string> csvLines = new List<string>();
            foreach (Client client in auctionHouse.Clients)
            {
                string[] csvLineArray = {client.ClientSignUp.Name, client.ClientSignUp.Email, client.ClientSignUp.Password};
                string csvLine = string.Join('﹁', csvLineArray);
                // cleint house info
                string[] houseInfoArray = {
                client.House.UnitNumber.ToString(),
                client.House.StreetNumber.ToString(),
                client.House.StreetName,
                client.House.StreetSuffix,
                client.House.City,
                client.House.State,
                client.House.PostCode.ToString() };
                string houseInfoString = string.Join('﹁', houseInfoArray);
                
                // List<Advert> advertsInput
                List<string> adverts = new List<string>();
                foreach( Advert ad in client.Adverts)
                {
                    //ad infor name, desc etc..
                    string[] adInfo = { ad.Name, ad.Description, ad.Price, ad.Bid };
                    string adString = string.Join('﹁', adInfo);
                    adverts.Add(adString);
                    //string nameInput, string descriptionInput, string priceInput, string topBidEmail
                }
                string advertsString = string.Join('!',adverts.ToArray());
                //, List<Bid> bidsList)
                List<string> bids = new List<string>();
                foreach (Bid bid in client.Bids)
                {
                    string[] bidInfo = { bid.BidderEmail, bid.BidAmount, bid.Item };
                    string bidString = string.Join('﹁', bidInfo);
                    //house delivery info for bid
                    string[] bidHouse = {
                        bid.House.UnitNumber.ToString(),
                        bid.House.StreetNumber.ToString(),
                        bid.House.StreetName,
                        bid.House.StreetSuffix,
                        bid.House.City,
                        bid.House.State,
                        bid.House.PostCode.ToString() };
                    string bidHouseString = string.Join('﹁', bidHouse);
                    string collectInfo = string.Join('﹁', bid.CollectInfo);

                    string[] bidArray = { bidString, bidHouseString, collectInfo };
                    bidString = string.Join('!', bidArray);


                    bids.Add(bidString);
                    //string nameInput, string descriptionInput, string priceInput, string topBidEmail
                }
                string bidsString = string.Join('!', bids.ToArray()); //join by class separator

                csvLineArray = new string[] { csvLine , houseInfoString , advertsString, bidsString };
                csvLine = string.Join('☺', csvLineArray); //join by separator
                csvLines.Add(csvLine); //add to list

            }
            string[] csvLinesArray = csvLines.ToArray();
            File.WriteAllLines(Path.GetFullPath(fileName), csvLinesArray);
        }

        //>>>>>>>>>>>>>>>>>>>>>

    }
}
