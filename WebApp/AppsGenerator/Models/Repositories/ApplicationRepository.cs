using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsGenerator.Models.Repositories
{
    public class ApplicationRepository : GenericRepository<Application>
    {
        public override Application GetById(int id)
        {
            return base.GetById(id);
        }
    }
}