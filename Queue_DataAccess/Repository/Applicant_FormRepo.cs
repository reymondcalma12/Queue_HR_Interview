using Queue.DataAccess;
using Queue.DataAccess.Repository.IRepository;
using Queue.Models;


namespace Queue.DataAccess.Repository
{
    public class Applicant_FormRepo: Repository<Applicant_Form>, IApplicant_FormRepo
    {
        public Applicant_FormRepo(AppDbContext db) : base(db)
        {
        }
            
    }
}
