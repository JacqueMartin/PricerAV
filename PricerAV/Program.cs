using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricerAV
{
    public class FundInvestor
    {
        private EnterInFundInvestor FirstInvestor;
        public DateTime StartDate { get; set; }
        public DateTime CurrentDate { get; set; }
        public FundInvestor(FundModel fund, DateTime startDate)
        {
            StartDate = startDate;
            CurrentDate = startDate;
            FirstInvestor = new EnterInFundInvestor(fund)
            {
                MaximumCash = 10000,
                NumberOfWeekToInvest = 52/2,
                StartDate = StartDate
            };
        }


        public List<MarketAction> Next()
        {
            var actions = new List<MarketAction>();

            var first = FirstInvestor.ActionToMarket(CurrentDate);

            if (first != null)
                actions.Add(first);


            CurrentDate = CurrentDate.AddDays(1);
            return actions;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            var history = new FundHistory();
            history.LoadFund(@"C:\work\Perso\git\Historical_data\FR0010298596.csv");
            history.LoadFund(@"C:\work\Perso\git\Historical_data\FR0010838284.csv");

            var Investors = new List<FundInvestor>();

            var Investor1 = new FundInvestor(history.Funds.First(), new DateTime(2010, 1, 1));
            var Investor2 = new FundInvestor(history.Funds.Last(), new DateTime(2010, 1, 1));

            Investors.Add(Investor1);
            Investors.Add(Investor2);

            var portofolio = new Portofolio();

            var totalOrder = 0;
            foreach (var investor in Investors)
            {
                while (investor.CurrentDate != new DateTime(2019, 2, 25))
                {
                    var actions = investor.Next();
                    foreach (var action in actions)
                    {
                        if (action.OrderToDo == OrderType.Buy)
                        {
                            var historicalValue = history.ValueAtTime(action.ISIN, action.Date);
                            portofolio.AddAction(action, historicalValue);
                            Console.WriteLine("ISIN: " + action.ISIN + "-> " + action.OrderToDo + " in: " + action.Date + " €: " + historicalValue);
                            totalOrder++;
                        }
                    }

                }
            }

            Console.WriteLine("Number of order summit: " + totalOrder);
            Console.WriteLine("Portofolio value : " + portofolio.PortofolioValue(history, new DateTime(2019, 1, 1)));

            //Console.Write(loader.Funds.Last().ToString());
            Console.ReadLine();
            
        }
    }
}
