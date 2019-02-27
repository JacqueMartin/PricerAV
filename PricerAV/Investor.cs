using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricerAV
{
    public enum OrderType { Buy, Sell };

    public class EnterInFundInvestor
    {
        public DayOfWeek DayToInvest = DayOfWeek.Monday;
        public double MaximumCash { get; set; }
        public DateTime StartDate { get; set; }
        public int NumberOfWeekToInvest { get; set; }

        public FundModel Fund { get; set; }

        public delegate void Buy();


        public EnterInFundInvestor(FundModel fund)
        {
            Fund = fund;
        }

        public MarketAction ActionToMarket(DateTime date)
        {
            if (date < StartDate)
                return null;

            var cashToInvest = MaximumCash / NumberOfWeekToInvest;

            if (!IsDateToInvest(date))
                return null;

            return new MarketAction()
            {
                Amount = cashToInvest,
                ISIN = Fund.ISIN,
                OrderToDo = OrderType.Buy,
                Date = date
            };
        }

        private bool IsDateToInvest(DateTime date)
        {
            // 1. On investi que les lundi
            // 2. On check si on a pas dépassé la date d'investissement
            if (date < StartDate)
                return false;

            if (!(date.DayOfWeek == DayToInvest))
                return false;

            double weeks = (date - StartDate).TotalDays / 7;
            if (!(weeks < NumberOfWeekToInvest))
                return false;

            return true;
        }


    }

    public class MarketAction
    {
        public OrderType OrderToDo { get; set; }
        public string ISIN { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
    }

}
