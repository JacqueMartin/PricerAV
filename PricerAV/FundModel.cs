using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricerAV
{
    public class FundModel
    {
        public string FilePath { get; set; }
        public List<FundTickModel> Ticks { get; set; }
        public string ISIN { get; set; }

        public FundModel(string ISIN)
        {
            this.ISIN = ISIN;
            Ticks = new List<FundTickModel>();
        }

        public override string ToString()
        {
            var str = string.Empty;
            foreach (var tick in Ticks)
            {
                str += String.Format("Date: {0}, Value: {1}\n", tick.Date.ToString("dd/MM/yyyy"), tick.Value.ToString());
            }
            return str;
        }

        public FundTickModel ClosedTickFromDate(DateTime dateTime)
        {
            var historicalAssetValue = Ticks.Where(y => y.Date.Date == dateTime.Date);

            while (!historicalAssetValue.Any())
            {
                dateTime = dateTime.AddDays(1);
                historicalAssetValue = Ticks.Where(y => y.Date.Date == dateTime);
                if (dateTime > Ticks.OrderBy(x => x.Date).LastOrDefault().Date)
                    return null;

            }

            return historicalAssetValue.LastOrDefault();
        }
    }

    public class FundTickModel
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }
}
