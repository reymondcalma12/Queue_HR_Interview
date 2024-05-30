using Queue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.DataAccess.Repository.IRepository
{
    public interface IStagesRepo : IRepository<Stage>
    {
        Task<IEnumerable<Stage>> GetStagesTable();
    }
}
