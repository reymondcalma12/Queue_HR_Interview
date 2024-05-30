using Queue.DataAccess;
using Queue.DataAccess.Repository.IRepository;
using Queue.Models;


namespace Queue.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        public IApplicant_FormRepo Applicant { get; private set; }
        public IApplicant_StatusRepo Applicant_Status { get; private set; }
        public IServingRepo Serving { get; private set; }
        public IQueue_Stage_1_Repo Queue_Stage_1 { get; private set; }
        public IQueue_Stage_2_Repo Queue_Stage_2 { get; private set; }
        public IQueue_Stage_3_Repo Queue_Stage_3 { get; private set; }
        public IQueueStatusRepo QueueStatus { get; private set; }
        public IStagesRepo Stages { get; private set; }
        public ITableRepo Tables { get; private set; }
        public ITable_ServeRepo Table_ServeRepo { get; }

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            Applicant = new Applicant_FormRepo(_db);
            Applicant_Status = new Applicant_StatusRepo(_db);
            Serving = new ServingRepo(_db);
            Queue_Stage_1 = new Queue_Stage_1_Repo(_db);
            Queue_Stage_2 = new Queue_Stage_2_Repo(_db);
            Queue_Stage_3 = new Queue_Stage_3_Repo(_db);
            QueueStatus = new QueueStatusRepo(_db);
            Stages = new StageRepo(_db);
            Tables = new TableRepo(_db);
            Table_ServeRepo = new Table_ServeRepo(_db);

        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}