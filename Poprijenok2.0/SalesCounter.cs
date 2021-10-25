using Poprijenok2._0.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poprijenok2._0
{
    public class SalesCounter
    {
        private Agent agent = new Agent();

        public int SalesCount(Agent selectedAgent)
        {
            var currentSales = Poprijenok2Entities.GetContext().ProductSale.ToList();

            var sales = currentSales.Count(s => s.AgentID == selectedAgent.ID);

            return sales;
        }
    }
}
