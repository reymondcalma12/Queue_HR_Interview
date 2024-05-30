using Queue.DataAccess.Repository.IRepository;
using Queue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.DataAccess.Repository
{
    public class QueueStatusRepo : Repository<Queue_Status>, IQueueStatusRepo
    {
        public QueueStatusRepo(AppDbContext db) : base(db)
        {
        }

    }
}
