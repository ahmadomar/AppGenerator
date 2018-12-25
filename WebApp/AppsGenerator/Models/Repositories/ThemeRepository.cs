using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsGenerator.Models.Repositories
{
    public class ThemeRepository : GenericRepository<Theme>
    {
        public override Theme GetById(int id)
        {
            return base.GetById(id);
        }
    }
}