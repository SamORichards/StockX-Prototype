using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Stock_Market_Simulator {
    static class MarketMakerManager {
        public static long NumberOfStocksOwned;
        static List<MarketMaker> marketMakers = new List<MarketMaker>() {
            new MarketMaker(long.MaxValue, 0.20f),
            new MarketMaker(long.MaxValue - 1, 0.20f),
            new MarketMaker(long.MaxValue - 2, 0.20f),
            new MarketMaker(long.MaxValue - 3, 0.20f),
            new MarketMaker(long.MaxValue - 4, 0.20f)
        };
        public static void AddJob(List<Offers> offers) {
            foreach (Offers o in offers) {
                marketMakers.OrderBy((MarketMaker m) => m.QueueSize).ToList()[0].AddJob(o);
            }
        }
        public static void AddJob(List<Bids> bids) {
            foreach (Bids b in bids) {
                marketMakers.OrderBy((MarketMaker m) => m.QueueSize).ToList()[0].AddJob(b);
            }
        }
        public static void RunMarketMakers() {
            //foreach (MarketMaker mm in marketMakers) {
            //    if (mm.QueueSize != 0) {
            //        Thread thread = new Thread(new ThreadStart(mm.RunTurn));
            //        thread.Start();
            //    }
            //}
        }
    }
}
