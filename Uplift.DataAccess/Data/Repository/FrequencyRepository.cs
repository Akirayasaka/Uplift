using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class FrequencyRepository : Repository<Frequency>, IFrequencyRepository
    {
        private readonly ApplicationDbContext _db;

        public FrequencyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetFrequencyForDropDown()
        {
            return _db.Frequcncy.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() });
        }

        public void Update(Frequency frequency)
        {
            var target = _db.Frequcncy.FirstOrDefault(x => x.Id == frequency.Id);

            try
            {
                target.Name = frequency.Name;
                target.FrequencyCount = frequency.FrequencyCount;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
