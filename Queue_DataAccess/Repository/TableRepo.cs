using Queue.DataAccess.Repository.IRepository;
using Queue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.DataAccess.Repository
{
    public class TableRepo : Repository<Table>, ITableRepo
    {
        public TableRepo(AppDbContext db) : base(db)
        {
        }

    }
}
