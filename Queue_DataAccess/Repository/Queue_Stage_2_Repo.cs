using Queue.DataAccess.Repository.IRepository;
using Queue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.DataAccess.Repository
{
    public class Queue_Stage_2_Repo : Repository<Queue_Stage_2>, IQueue_Stage_2_Repo
    {
        public Queue_Stage_2_Repo(AppDbContext db) : base(db)
        {
        }

    }

}