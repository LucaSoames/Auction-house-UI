using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class Client
    {
        private ClientSignUp clientSignUp;
        public ClientSignUp ClientSignUp { get; set; } // client log in information

        private House house; 

        public House House { get; set; } // client delivery information

        private List<Advert> adverts;

        public List<Advert> Adverts { get; set; } //clients products listed or purchased

        private List<Bid> bids;

        public List<Bid> Bids { get; set; } //clients bids on items

        public Client(ClientSignUp clientInfo, House clientHouse, List<Advert> advertsInput, List<Bid> bidsList)
        {
            House = clientHouse;
            ClientSignUp = clientInfo;
            Adverts = advertsInput;
            Bids = bidsList;
        }


        public static bool IsFirstAd(Client client) //check if the client has any adverisements
        {
            if (client.Adverts.Count == 1)
            {
                Advert clientAd = client.Adverts[0];
                if (clientAd.Name == "-") //check for default value
                {
                    return true;
                }
                else return false;

            }
            else
            {
                return false;
            }
        }
    }
}
