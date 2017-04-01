using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Market_Simulator {
    class Pool {
        static List<BidPriceLevel> BidsPool = new List<BidPriceLevel>();
        static List<OfferPriceLevel> OffersPool = new List<OfferPriceLevel>();
        public static int NumberOfBuyer {
            get {
                return BidsPool.Count;
            }
        }
        public static int NumberOfOffers {
            get {
                return OffersPool.Count;
            }
        }
        public static void AddBid(Bids bid) {
            for (int i = 0; i < BidsPool.Count; i++) {
                if (bid.Price == BidsPool[i].Price) {
                    BidsPool[i].bids.Add(bid);
                    return;
                }
            }
            BidsPool.Add(new BidPriceLevel { Price = bid.Price, bids = new List<Bids>() { bid } });
            BidsPool = BidsPool.OrderBy((BidPriceLevel b) => b.Price).ToList();
        }
        public static void AddOffer(Offers offer) {
            for (int i = 0; i < OffersPool.Count; i++) {
                if (offer.Price == OffersPool[i].Price) {
                    OffersPool[i].offers.Add(offer);
                    return;
                }
            }
            OffersPool.Add(new OfferPriceLevel { Price = offer.Price, offers = new List<Offers>() { offer } });
            OffersPool = OffersPool.OrderByDescending((OfferPriceLevel o) => o.Price).ToList();
        }
        public static void RunMatchMaker() {
            PoolMatchMaker pmm = new PoolMatchMaker();
            pmm.ProRataWithLMM(BidsPool, OffersPool);
            int NumberOfBidsLeft = 0;
            foreach (BidPriceLevel BPL in BidsPool) {
                foreach (Bids bid in BPL.bids) {
                    NumberOfBidsLeft++;
                }
            }
        }
        public static void SetBidPool(List<BidPriceLevel> tempBPL) {
            BidsPool = tempBPL;
        }
        public static void SetOfferPool(List<OfferPriceLevel> tempOPL) {
            OffersPool = tempOPL;
        }
    }
    class BidPriceLevel {
        public double Price;
        public List<Bids> bids;
    }
    class OfferPriceLevel {
        public double Price;
        public List<Offers> offers;
    }
    class Bids {
        public double Price;
        private long quanity;
        public int TurnsInPool = 0;
        public Client Owner;

        public long Quanity {
            get => quanity;
            set {
                if (value < 0) {
                    throw new Exception("Bid quanity is less than 0!");
                }
                quanity = value;
            }
        }

        public Bids(double price, long quanity, Client owner) {
            Price = price;
            Owner = owner;
            Quanity = quanity;
        }
    }
    class Offers {
        public double Price;
        public long quanity;
        public int TurnsInPool = 0;
        public Client Owner;
        public float ProRata;
        public long Quanity {
            get => quanity;
            set {
                if (value < 0) {
                    throw new Exception("Offer quanity is less than 0!");
                }
                quanity = value;
            }
        }
        public Offers(double price, long quanity, Client owner) {
            Price = price;
            Owner = owner;
            Quanity = quanity;
        }
    }
    class Client {
        public long ID { get; private set; }
        public float LMMPercentage { get; private set; }
        public Action<long> OnBuy;
        public Client(long id, float lmmPercentage = 0f, Action<long> onBuy = null) {
            ID = id;
            LMMPercentage = lmmPercentage;
            OnBuy = onBuy;
        }
    }
}
