using System.Collections.Generic;

namespace SweetSavory.Models
{
    public class Flavor
    {
        public int FlavorId { get; set; }
        public string FlavorType { get; set; }
        public ICollection<FlavorTreat> Treats { get; set; }

        public Flavor()
        {
            this.Treats = new HashSet<FlavorTreat>();
        }
    }
}