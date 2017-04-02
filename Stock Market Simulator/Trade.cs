using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Market_Simulator {
    class Trade {
        double _TradePrice;
        public double TradePrice { get {
                return _TradePrice;
            } }
        //This is repsonsible for completing a transaction
        public Trade(ref Bids bid, ref Offers offer, long quanity) {
            UpdateQuanities(ref bid, ref offer, quanity);
            MoneyTransaction(ref bid, ref offer, quanity);
            UpdateDatabase(ref bid, ref offer, quanity);
            AddToTransactionList(ref bid, ref offer, quanity);
        }
        public Trade(ref Bids bid, long quanity) {

            UpdateQuanities(ref bid, quanity);
            MoneyTransaction(ref bid, quanity);
            UpdateDatabase(ref bid, quanity);
            AddToTransactionList(ref bid, quanity);
        }
        void UpdateDatabase(ref Bids bid, ref Offers offer, long quanity) {

        }
        void MoneyTransaction(ref Bids bid, ref Offers offer, long quanity) {
            //if (bid.Price != offer.Price) {
            //    Console.WriteLine("Bid Price and Offer Price not the same!");
            //}
            _TradePrice = bid.Price;
        }
        void UpdateQuanities(ref Bids bid, ref Offers offer, long quanity) {
            bid.Quanity -= quanity;
            offer.quanity -= quanity;
            if (bid.Owner.OnBuy != null) {
                bid.Owner.OnBuy(quanity);
            }
            if (offer.Owner.OnSell != null) {
                offer.Owner.OnSell(quanity);
            }
        }
        void AddToTransactionList(ref Bids bid, ref Offers offer, long quanity) {
            StockTicker.TradesThisTurn.Add(this);
        }


        void UpdateDatabase(ref Bids bid, long quanity) {

        }
        void MoneyTransaction(ref Bids bid, long quanity) {
            //if (bid.Price != offer.Price) {
            //    Console.WriteLine("Bid Price and Offer Price not the same!");
            //}
            _TradePrice = bid.Price;
        }
        void UpdateQuanities(ref Bids bid, long quanity) {
            bid.Quanity -= quanity;
            if (bid.Owner.OnBuy != null) {
                bid.Owner.OnBuy(quanity);
            }
        }
        void AddToTransactionList(ref Bids bid, long quanity) {
            StockTicker.TradesThisTurn.Add(this);
        }
    }
}
