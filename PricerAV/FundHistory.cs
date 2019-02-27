using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricerAV
{
    public class FundHistory
    {
        public List<FundModel> Funds { get; set; }

        public FundHistory()
        {
            Funds = new List<FundModel>();
        }

        public bool LoadFund(string PathFund)
        {
            if (string.IsNullOrEmpty(PathFund))
                return false;

            if (!File.Exists(PathFund))
                return false;

            var fund = new FundModel(Path.GetFileNameWithoutExtension(PathFund));
            fund.FilePath = PathFund;

            try
            {
                using (var reader = new StreamReader(PathFund))
                {
                    var isFirstFile = true;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (isFirstFile)
                        {
                            isFirstFile = !isFirstFile;
                            continue;
                        }


                        var values = line.Split(';');

                        var tick = new FundTickModel()
                        {
                            Date = DateTime.Parse(values[0]),
                            Value = double.Parse(values[1])
                        };
                        fund.Ticks.Add(tick);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            Funds.Add(fund);

            return true;
        }

        public double ValueAtTime(string ISIN, DateTime date)
        {
            var funds = Funds.Where(x => x.ISIN == ISIN);
            if (funds.Count() > 1)
                throw new InvalidHistoricalDataException("Multi fund with same ISIN");

            if (!funds.Any())
                throw new InvalidHistoricalDataException("Fund not found");

            return funds.First().ClosedTickFromDate(date).Value;
        }
    }
}
