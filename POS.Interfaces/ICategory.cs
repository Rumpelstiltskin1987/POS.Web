using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Entities;

namespace POS.Interfaces
{
    public interface ICategory
    {
        void Add(Category category);
        void Delete(int id);
        IEnumerable<Category> GetAllActive();
        IEnumerable<Category> GetAllInactive();
        Category GetById(int id);         
        void Inactivate(Category category);
        void Update(Category category);
    }
}
