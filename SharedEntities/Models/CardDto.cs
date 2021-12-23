using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedEntities.Models
{
    public class CardDto
    {
        public Guid? Id { get; set; }
        public Guid UserId { get; set; }

        [StringLength(3)]
        public string CVC { get; set; }

        [StringLength(16)]
        public string Pan { get; set; }

        public bool IsDefault { get; set; }

        public CardDate Date { get; set; }

        public string CardName { get; set; }

    }

    public class CardDate
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
