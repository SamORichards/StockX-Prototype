using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Market_Simulator {
    enum JobType { Bid, Offer }
    class MarketMaker {
        List<Job> Queue = new List<Job>();
        public int QueueSize { private set { } get { return Queue.Count; } }
        Client client;

        public MarketMaker(long ID, float LMMPercentage) {
            if (LMMPercentage > 1f) {
                throw new Exception("LMMPercentage is greater than 1!");
            }
            client = new Client(ID, LMMPercentage, OnBuy);
        }

        public void AddJob(Bids bid) {
            Queue.Add(new Job(JobType.Bid, bid));
        }

        public void AddJob(Offers offer) {
            Queue.Add(new Job(JobType.Offer, null, offer));
        }
        public void OnBuy(long Quanity) {
            MarketMakerManager.NumberOfStocksOwned += Quanity;
        }

        public void RunTurn() {
            for (int i = 0; i < Queue.Count; i++) {
                if (Queue[i].Quanity < 0) {
                    throw new Exception("Less than 0 Quanity in Job");
                }
                switch (Queue[i].jobType) {
                    case JobType.Bid:
                        RunBid(Queue[i]);
                        break;
                    case JobType.Offer:
                        RunOfffer(Queue[i]);
                        break;
                }
            }
            Queue.RemoveAll((Job j) => j.Quanity == 0);
        }

        void RunBid(Job job) {
            if (job.Bid.Quanity <= MarketMakerManager.NumberOfStocksOwned) {
                MarketMakerManager.NumberOfStocksOwned -= job.Quanity;
                new Trade(ref job.Bid, job.Quanity);
            } else {
                //TODO: Count how many times takes and maybe cancel transaction if can't be completed
                Pool.AddBid(new Bids(Double.PositiveInfinity, 10, client));
                MarketMakerManager.NumberOfStocksOwned += job.Quanity;
                new Trade(ref job.Bid, job.Quanity);
            }
        }

        void RunOfffer(Job job) {
            MarketMakerManager.NumberOfStocksOwned += job.Offer.Quanity;
            job.Offer.Quanity = 0;
        }

    }
    class Job {
        public JobType jobType;
        public Bids Bid;
        public Offers Offer;
        public bool OfferPlaced = false;
        public long Quanity {
            get {
                if (jobType == JobType.Bid) {
                    return Bid.Quanity;
                } else {
                    return Offer.Quanity;
                }
            }
        }
        public Job(JobType jobType, Bids Bid = null, Offers Offer = null) {
            this.jobType = jobType;
            this.Bid = Bid;
            this.Offer = Offer;
        }
    }
}
