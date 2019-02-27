using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricerAV
{
    public class Portofolio
    {
        private List<Order> Orders;
        public Portofolio()
        {
            Orders = new List<Order>();
        }

        public void Buy(string ISIN, double quantity, DateTime orderDate)
        {
            Orders.Add(new Order()
            {
                ISIN = ISIN,
                Quantity = quantity,
                OrderType = OrderType.Buy,
                DateOfAction = orderDate
            });
        }

        public double PortofolioValue(FundHistory history, DateTime dateToEvaluated)
        {
            double value = 0.0;
            foreach (var order in Orders)
            {
                var ticketHistory = history.Funds.Where(x => x.ISIN == order.ISIN).FirstOrDefault();

                if (order.OrderType == OrderType.Buy)
                    value += order.Quantity * ticketHistory.ClosedTickFromDate(dateToEvaluated).Value;

                if (order.OrderType == OrderType.Sell)
                    value -= order.Quantity * ticketHistory.ClosedTickFromDate(dateToEvaluated).Value;

            }
            return value;
        }

        public void AddAction(MarketAction action, double ValueAtOrderDate)
        {
            if (action.OrderToDo == OrderType.Buy)
                Buy(action.ISIN, action.Amount / ValueAtOrderDate , action.Date);
        }
    }

    public class Order
    {
        public string ISIN { get; set; }
        public double Quantity { get; set; }
        public DateTime DateOfAction { get; set; }

        public OrderType OrderType { get; set; }
        public Order()
        {

        }
    }
}
