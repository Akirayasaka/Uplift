using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository.IRepository
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        private readonly ApplicationDbContext _db;
        public ServiceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Service service)
        {
            var objFormDb = _db.Service.FirstOrDefault(s => s.Id == service.Id);

            objFormDb.Name = service.Name;
            objFormDb.LongDesc = service.LongDesc;
            objFormDb.Price = service.Price;
            objFormDb.ImageUrl = service.ImageUrl;
            objFormDb.CategoryId = service.CategoryId;
            objFormDb.FrequencyId = service.FrequencyId;

            _db.SaveChanges();
        }
    }
}
