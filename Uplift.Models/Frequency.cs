using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uplift.Models
{
    public class Frequency
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int FrequencyCount { get; set; }
    }
}
