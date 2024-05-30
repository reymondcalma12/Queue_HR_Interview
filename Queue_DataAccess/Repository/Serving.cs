using Queue.DataAccess;
using Queue.DataAccess.Repository.IRepository;
using Queue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.DataAccess.Repository
{
    public class ServingRepo : Repository<Serving>, IServingRepo
    {
        public ServingRepo(AppDbContext db) : base(db)
        {
        }

    }

}
