using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exporter.DTO
{
    public class CovidSummary
    {
        public int DeathsNumber { get; set; }
        public int ActiveNumber { get; set; }
        public string country { get; set; }

        public string Date { get; set; }
    }
}
