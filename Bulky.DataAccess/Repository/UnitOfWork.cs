using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository category { get;  private set; }
        public UnitOfWork(ApplicationDbContext db) 
        { 
            _db = db;
            category = new CategoryRepository(_db);
        }

        public void save()
        {
            _db.SaveChanges();
        }
    }
}
