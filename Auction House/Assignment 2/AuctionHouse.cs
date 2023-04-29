using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class AuctionHouse
    {

        private List<Client> clients = new List<Client>();

        public List<Client> Clients { get; set; }


        // An AuctionHouse is the main database that the program parses and receives information from, it is comprised of a list of Client objects.
        public AuctionHouse(List<Client> clients)
        {
            Clients = clients;
        }
        // Add client object to AuctionHouse client list field.
        public void RegisterClient(Client client)
        {
            Clients.Add(client);
        }

        // checks if password input string matches client password string.
        public bool IsPasswordMatch(string email, string password)
        {
            foreach (Client client in Clients) // foreach loop through clients in auctionhouse
            {
                if (client.ClientSignUp.Email == email) // for each Client's clientsignup information return the email and check if it is equal to the input email, if true: 
                {
                    if (client.ClientSignUp.Password == password) // check if password matches
                    {
                        return true;
                    }

                }
                
            }
            return false;
        }
        //return client list
        public List<Client> GetClientList()
        {
            return Clients;
        }

        public Client GetClient(string email) //index client list in auctionhouse for a matching string email. Returns the match else it returns null.
        {
            foreach (Client client in Clients)
            {
                if (client.ClientSignUp.Email == email)
                {
                    return client;
                    break;
                }
            }
            return null;

        }
        public Advert FindAdvert(Bid bid) // returns advert for a bid input
        {
            
            string bidItemName = bid.Item; // gets bid string field 'Item'
            foreach (Client client in Clients)
            {
                foreach (Advert advert in client.Adverts)
                if (advert.Name == bidItemName) // references item to find the matching advert from clients
                {
                        return advert;
                        break;
                }
            }
            return null;
        }

        public List<Bid> FindBids(string itemName) // returns the list of bids for an item name 
        {

            List<Bid> bids = new List<Bid>();
            foreach (Client client in clients) //implements a loop through clients
            {
                foreach (Bid bid in client.Bids) // implements a nested loop through bids for each client to find any matches of 'item name'
                    if (bid.Item == itemName)
                    {
                        bids.Add(bid);
                    }
            }
            return bids;
        }

        public bool IsTopBid(Bid bid, Advert advert)
        {
            if (advert.Bid == "-") // check if default bid
            {
                return true;
            }
            else if (Decimal.Parse(bid.BidAmount.Remove(0, 1)) > Decimal.Parse(advert.Bid.Remove(0, 1))) // parse currency string values to dec and check if the current top bid is lower than the new bid
            {
                return true;
            }
            else return false;
        }

        public bool IsPurchasedProduct(Advert ad, Client client)//check if client owns this advert
        {
            if (ad.Bid == client.ClientSignUp.Email) //if client email == ad topbid email address this means it has been purchased by the client
            {
                return true;
            }
            else return false;
        }

        public Bid FindBid(string bidEmail, string itemName) //return bid from bid email and the advert item name
        {
            Client bidder = GetClient(bidEmail); //implements GetClient function to find the matching Client object for the string input bidEmail
            foreach(Bid bid in bidder.Bids) //loops through all Bids in Client's bid list
            {
                if (bid.Item == itemName) //if matching string fields
                {
                    return bid; // return bid
                }
            }
            return null;
        }
    }
}
