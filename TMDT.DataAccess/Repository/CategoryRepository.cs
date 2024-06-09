﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TMDT.DataAccess.Data;
using TMDT.DataAccess.Repository.IRepository;
using TMDT.Models;

namespace TMDT.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db; 
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Updata(Category obj)
        {
            _db.categories.Update(obj);
        }
    }
}