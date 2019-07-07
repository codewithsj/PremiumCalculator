using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PremiumCalculator.Models
{
    public class Premium
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public string DOB { get; set; }

        public string OccupationName { get; set; }

        public int? OccupationID { get; set; }

        public double DeathSumInsured { get; set; }
    }
}