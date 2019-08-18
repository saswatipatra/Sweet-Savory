using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SweetSavory.Models
{
    [Table("Treats")]   
     public class Treat
    {
       
        
        [Key]
        public int TreatId { get; set; }
        public string TreatType { get; set; }
        public string Description { get; set; }
        public bool Recivied { get; set; }
        public DateTime DeliveryDate { get; set; }

        public ICollection<FlavorTreat> Flavors { get; }
        public virtual ApplicationUser User { get; set; }

         public Treat()
        {
            this.Flavors = new HashSet<FlavorTreat>();
            this.Recivied = false;
        }
    }
}