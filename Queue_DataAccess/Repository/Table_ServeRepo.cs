using Queue.DataAccess.Repository.IRepository;
using Queue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.DataAccess.Repository
{
    public class Table_ServeRepo : Repository<Table_Serve>, ITable_ServeRepo
    {
        public Table_ServeRepo(AppDbContext db) : base(db)
        {
        }

    }
}
