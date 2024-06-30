﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMDT.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        CategoryRepository Category { get; }
        IProductRepository Product { get; }
        void Save();
        
    }
}
