using System.Collections;
using System.Collections.Generic;

namespace Uplift.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Category> CategoryList { get; set; }

        public IEnumerable<Service> ServicesList { get; set; }
    }
}
