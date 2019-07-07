using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PremiumCalculator.Models
{
    public class Rating
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Factor { get; set; }
    }
}