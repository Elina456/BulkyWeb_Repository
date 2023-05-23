﻿using Bulky.DataAccess.Data;
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
        public IProductRepository product { get; private set; }
        public ICompanyRepository company { get; private set; }
        public IShoppingCartRepository shoppingCart { get; private set; }
        public IApplicationUserRepository applicationUser { get; private set; }
        public IOrderHeaderRepository orderHeader { get; private set; }
        public IOrderDetailRepository orderDetail { get; private set; }
        public UnitOfWork(ApplicationDbContext db) 
        { 
            _db = db;
            category = new CategoryRepository(_db);
            product = new ProductRepository(_db);
            company = new CompanyRepository(_db);
            shoppingCart = new ShoppingCartRepository(_db);
            applicationUser = new ApplicationUserRepository(_db);
            orderHeader = new OrderHeaderRepository(_db);
            orderDetail = new OrderDetailRepository(_db);

        }

        public void save()
        {
            _db.SaveChanges();
        }
    }
}
