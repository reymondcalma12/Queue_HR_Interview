using Microsoft.EntityFrameworkCore;
using Queue.DataAccess.Repository.IRepository;
using Queue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.DataAccess.Repository
{
    public class StageRepo : Repository<Stage>, IStagesRepo
    {
        private AppDbContext _db;

        public StageRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Stage>> GetStagesTable()
        {
            return await _db.Stage.Include(x => x.tables).ToListAsync();
        }
    }
}
