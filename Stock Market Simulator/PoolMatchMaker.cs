using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Market_Simulator {
    class PoolMatchMaker {

        public void ProRataWithLMM(List<BidPriceLevel> BidsPool, List<OfferPriceLevel> OffersPool) {
            BidsPool = BidsPool.OrderBy((BidPriceLevel b) => b.Price).ToList();
            OffersPool = OffersPool.OrderByDescending((OfferPriceLevel o) => o.Price).ToList();
            foreach (BidPriceLevel BPL in BidsPool) {
                for (int j = 0; j < OffersPool.Count; j++) {
                    if (OffersPool[j].Price <= BPL.Price) {
                        while (BPL.bids.Count != 0 && OffersPool[j].offers.Count != 0) {
                            Bids b = BPL.bids[0];
                            List<Offers> offers = OffersPool[j].offers;
                            //DisplayDetails(b, offers);
                            LMMRound(ref b, ref offers);
                            //DisplayDetails(b, offers);
                            ProRata(ref b, ref offers, 1);
                            //DisplayDetails(b, offers);
                            FIFO(ref b, ref offers);
                            //DisplayDetails(b, offers);
                            BPL.bids[0] = b;
                            OffersPool[j].offers = offers;
                            if (BPL.bids[0].Quanity == 0) {
                                BPL.bids.RemoveAt(0);
                            }
                            OffersPool[j].offers.RemoveAll((Offers o) => o.Quanity == 0);
                        }
                    }
                }
            }
            foreach (BidPriceLevel BPL in BidsPool) {
                foreach (Bids b in BPL.bids) {
                    b.TurnsInPool++;
                }
            }
            foreach (OfferPriceLevel OPL in OffersPool) {
                foreach (Offers o in OPL.offers) {
                    o.TurnsInPool++;
                }
            }
            UpdatePools(BidsPool, OffersPool);
        }

        public void FIFO(ref Bids bid, ref List<Offers> offers) {
            //foreach (Offers o in offers) {
            for (int i = 0; i < offers.Count; i++) {
                if (offers[i].Quanity > bid.Quanity) {
                    Offers o2 = offers[i];
                    new Trade(ref bid, ref o2, bid.Quanity);
                    offers[i] = o2;
                    break;
                } else {
                    Offers o2 = offers[i];
                    new Trade(ref bid, ref o2, offers[i].quanity);
                    offers[i] = o2;
                }
            }
        }

        public void ProRata(ref Bids bid, ref List<Offers> offers, int ProRataMinimumAlloaction) {
            long TotalQuanityOfOffers = 0;
            long BidQuanity = bid.Quanity;
            foreach (Offers o in offers) {
                TotalQuanityOfOffers += o.Quanity;
            }
            for (int i = 0; i < offers.Count; i++) {
                offers[i].ProRata = (float)offers[i].Quanity / (float)TotalQuanityOfOffers;
                int ProRataAmount = (int)(offers[i].ProRata * BidQuanity);
                if (ProRataAmount >= ProRataMinimumAlloaction) {
                    if (ProRataAmount > offers[i].Quanity) {
                        Offers o2 = offers[i];
                        new Trade(ref bid, ref o2, offers[i].quanity);
                        offers[i] = o2;
                    } else {
                        Offers o2 = offers[i];
                        new Trade(ref bid, ref o2, ProRataAmount);
                        offers[i] = o2;
                    }
                }
            }
        }

        public void LMMRound(ref Bids bid, ref List<Offers> offers) {
            long BidQuanity = bid.Quanity;
            for (int i = 0; i < offers.Count; i++) {
                if (offers[i].Owner.LMMPercentage > 0f) {
                    int LMMAmount = (int)(BidQuanity * offers[i].Owner.LMMPercentage);
                    if (LMMAmount > offers[i].Quanity) {
                        Offers o2 = offers[i];
                        new Trade(ref bid, ref o2, offers[i].quanity);
                        offers[i] = o2; 
                    } else {
                        Offers o2 = offers[i];
                        new Trade(ref bid, ref o2, LMMAmount);
                        offers[i] = o2;
                    }
                }
            }
        }

        void UpdatePools(List<BidPriceLevel> BidsPool, List<OfferPriceLevel> OffersPool) {
            foreach (BidPriceLevel BPL in BidsPool) {
                MarketMakerManager.AddJob(BPL.bids.Where((Bids b) => b.TurnsInPool >= 10 && b.Quanity > 0).ToList());
                BPL.bids.RemoveAll((Bids b) => b.TurnsInPool >= 10 || b.Quanity == 0);
            }
            foreach (OfferPriceLevel OPL in OffersPool) {
                MarketMakerManager.AddJob(OPL.offers.Where((Offers o) => o.TurnsInPool >= 10 && o.quanity > 0).ToList());
                OPL.offers.RemoveAll((Offers o) => o.TurnsInPool >= 10 || o.Quanity == 0);
            }
            BidsPool.RemoveAll((BidPriceLevel PBL) => PBL.bids.Count == 0);
            OffersPool.RemoveAll((OfferPriceLevel OPL) => OPL.offers.Count == 0);
            //DisplayDetails(BidsPool, OffersPool);
            Pool.SetBidPool(BidsPool);
            Pool.SetOfferPool(OffersPool);
        }

        void DisplayDetails(Bids bid, List<Offers> offers) {
            Console.WriteLine();
            Console.WriteLine("Price: {0}  Bid Qaunity: {1}", bid.Price, bid.Quanity);
            Console.WriteLine("Offers:");
            foreach (Offers o in offers) {
                Console.WriteLine("Client {0} is offering {1}", o.Owner.ID, o.Quanity);
            }
        }

        void DisplayDetails(List<BidPriceLevel> BidPool, List<OfferPriceLevel> OfferPool) {
            Console.WriteLine("");
            Console.WriteLine("Bids");
            foreach (BidPriceLevel bpl in BidPool) {
                Console.WriteLine(" Price: {0}", bpl.Price);
                long quainity = 0;
                foreach (Bids b in bpl.bids) {
                    quainity += b.Quanity;
                }
                Console.WriteLine("{0}", quainity);
            }
            Console.WriteLine("");
            Console.WriteLine("Offers");
            foreach (OfferPriceLevel opl in OfferPool) {
                Console.WriteLine(" Price: {0}", opl.Price);
                long quainity = 0;
                foreach (Offers o in opl.offers) {
                    quainity += o.Quanity;
                }
                Console.WriteLine("{0}", quainity);
            }
        }
    }
}
