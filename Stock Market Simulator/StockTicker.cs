using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Market_Simulator {

    class StockTicker {
        static AlgorithmsTrader1 algoTrader = new AlgorithmsTrader1(1000);
        static double lowestPrice = 5.51f;
        static double highestPrice = 0f;
        public static List<Trade> TradesThisTurn = new List<Trade>();
        public static double CurrentPrice = 5.50f;
        static void Main(string[] args) {
            Stopwatch mainTime = new Stopwatch();
            int TotalStocksInCirculation = 10000;
            mainTime.Start();
            int i = 0;
            while (1 == 1) {
                if (mainTime.Elapsed.TotalSeconds > 0.2f) {
                    i++;
                    if (i == 5) {
                        algoTrader.RunTurn(TradesThisTurn);
                        TradesThisTurn.Clear();
                        MarketMakerManager.RunMarketMakers();
                        i = 0;
                    }
                    Random r = new Random((int)DateTime.Now.Ticks);
                    int NumOfBuyers = r.Next(10);
                    int NumOfOffers = r.Next(10);
                    for (int j = 0; j < NumOfBuyers; j++) {
                        Pool.AddBid(new Bids(Math.Round(CurrentPrice, 2), r.Next(100), new Client(j)));
                    }
                    for (int j = 0; j < NumOfOffers; j++) {
                        Pool.AddOffer(new Offers(Math.Round(CurrentPrice, 2), r.Next(100), new Client(j)));
                    }
                    UpdateStockPrice(ref CurrentPrice, Pool.NumberOfBuyer, Pool.NumberOfOffers, TotalStocksInCirculation);
                    mainTime.Stop();
                    Pool.RunMatchMaker();
                    mainTime.Reset();
                    mainTime.Start();
                }
            }
        }


        private static void UpdateStockPrice(ref double startPrice, int numOfBuyers, int numOfOffers, int totalStocksInCirculation) {
            Console.WriteLine("The Number Of Bids is {0}, and the Number Of Offers is: {1}", numOfBuyers, numOfOffers);
            float ChangeInPrice = ((float)(numOfBuyers - numOfOffers) / (float)totalStocksInCirculation);
            startPrice += (ChangeInPrice * 10);
            Console.WriteLine("New Price is " + startPrice);
            if (startPrice > highestPrice) {
                highestPrice = startPrice;
            }
            if (startPrice < lowestPrice) {
                lowestPrice = startPrice;
            }
        }
    }
}
