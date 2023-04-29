using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Assignment2
{
    internal class Bid
    {
        public string BidderEmail { get; set; }
        public string BidAmount { get; set; }
        public string Item { get; set; }

        public House House { get; set; } //House class used for delivery info

        public string[] CollectInfo { get; set; }

        public Bid(string bidderEmail, string bidAmount, string itemName, House deliveryAddressInfo, string[] deliveryInfo) //constructor
        {
            //set; public variables
            BidderEmail = bidderEmail;
            BidAmount = bidAmount;
            Item = itemName;
            House = deliveryAddressInfo; 
            CollectInfo = deliveryInfo;
        }



        public static Bid TopBid(List<Bid> bids) //returns the top bid from a list of bids
        {
            //simple algorithm used to find greatest value
            Bid topBid = bids[0]; //topbid initialised as first value in bids list
            for (int i = 0; i < bids.Count; i++) //counter controlled loop
            {
                decimal topBidDecimal = Decimal.Parse(topBid.BidAmount.Remove(0,1)); //decimal parsing for top bid currency amount
                if (Decimal.Parse(bids[i].BidAmount.Remove(0, 1)) > topBidDecimal) //if current bid value is greater than current top bid
                {
                    topBid = bids[i]; //top bid = new bid
                }
            }
            return topBid; //after loop return top bid
        }
    }
}
