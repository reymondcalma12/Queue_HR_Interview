using Queue.DataAccess;
using Queue.DataAccess.Repository.IRepository;
using Queue.Models;


namespace Queue.DataAccess.Repository
{
    public class Applicant_StatusRepo : Repository<Applicant_Status>, IApplicant_StatusRepo
    {

        public Applicant_StatusRepo(AppDbContext db) : base(db)
        {
        }

    }
}
