using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Market_Simulator {
    public enum Segments { RapidlyRising, Rising, Falling, RapidlyFalling }
    public enum Stance { ShortTermLong, ShortTermShort}
    class AlgorithmsTrader1 :AlgoTrader {
        public List<StockTurn> StockTurns = new List<StockTurn>();
        public Client client = new Client(69, 05);
        //Takes a look at last two cycles
        //establishes trend
        //purchases or sell depending on position eg long or short
        public AlgorithmsTrader1(long stocksOwned) {
            StocksOwned = stocksOwned;
        }

        public void RunTurn(List<Trade> Trades) {
            if (Trades.Count == 0) {
                return;
            }
            StockTurns.Add(new StockTurn(Trades));
            CreateNewStance(true);
        }
        void CreateNewStance(bool ShortTerm) {
            if (ShortTerm) {
                if (StockTurns.Count < 4) {
                    return;
                }
                int TotalLast3Turns = 0;
                for (int i = 1; i <= 3; i++) {
                    TotalLast3Turns += (int)StockTurns[StockTurns.Count - i].Trend + 1;
                }
                float AverageLast3Turns = (float)TotalLast3Turns / 3f;
                if (AverageLast3Turns <= 1.6f) {
                    new MarketStance(Stance.ShortTermLong, MathsHelper.Lerp(10, 100, 1f - (AverageLast3Turns - 1f)), client, this);
                }
            }
        }

        class MarketStance {
            public Stance stance;
            public Client client;
            AlgoTrader Owner;
            //TODO: Take market price at which baught at then if price falls below sell and of price increase to a certain point buy
            //Add to a list in main trader which run through on on turn
            public MarketStance(Stance s, long Quanity, Client c, AlgoTrader owner) {
                stance = s;
                client = c;
                Owner = owner;
                switch (stance) {
                    case Stance.ShortTermLong:
                        ShortTermLong(Quanity);
                        break;
                }
            }

            void ShortTermLong(long Quanity) {
                Console.WriteLine("yes");
                client.OnBuy += OnBuy;
                Pool.AddBid(new Bids(StockTicker.CurrentPrice, Quanity, client));
            }

            public void OnBuy(long Quanity) {
                Owner.StocksOwned += Quanity;
                Console.WriteLine("Stocks Baught By Algo! " + Quanity);
            }
        }
    }

    class StockTurn {
        public double OpeningPrice;
        public double LowPrice;
        public double HighPrice;
        public double ClosePrice;
        public double AveragePrice;
        public Segments Trend;
        public StockTurn(List<Trade> trades) {
            OpeningPrice = trades[0].TradePrice;
            LowPrice = trades.OrderBy((Trade t) => t.TradePrice).ToList()[0].TradePrice;
            HighPrice = trades.OrderByDescending((Trade t) => t.TradePrice).ToList()[0].TradePrice;
            ClosePrice = trades[trades.Count - 1].TradePrice;
            double TotalPrice = 0;
            foreach (Trade t in trades) {
                TotalPrice += t.TradePrice;
            }
            AveragePrice = TotalPrice / (double)trades.Count;
            Trend = AssignSegment();
            Console.WriteLine(Trend);
        }
        Segments AssignSegment() {
            if (ClosePrice > OpeningPrice) {
                if (ClosePrice > AveragePrice) {
                    return Segments.RapidlyRising;
                } else {
                    return Segments.Rising;
                }
            } else {
                if (ClosePrice > AveragePrice) {
                    return Segments.Falling;
                } else {
                    return Segments.RapidlyFalling;
                }
            }
        }
    }

    public class AlgoTrader {
        public long StocksOwned;
    }
}
