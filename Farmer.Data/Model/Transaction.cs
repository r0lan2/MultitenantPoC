using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Farmer.Data.Model
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        public int SiteId { get; set; }
        public string TransactionType { get; set; }

    }
}
