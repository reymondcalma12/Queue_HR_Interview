using Queue.DataAccess.Repository.IRepository;
using System;

namespace Queue.DataAccess.Repository
{
    public interface IUnitOfWork
    {
        IApplicant_FormRepo Applicant { get; }
        IApplicant_StatusRepo Applicant_Status { get; }
        IServingRepo Serving { get; }
        IQueue_Stage_1_Repo Queue_Stage_1 { get; }
        IQueue_Stage_2_Repo Queue_Stage_2 { get; }
        IQueue_Stage_3_Repo Queue_Stage_3 { get; }
        IQueueStatusRepo QueueStatus { get; }
        IStagesRepo Stages { get; }
        ITableRepo Tables { get; }
        ITable_ServeRepo Table_ServeRepo { get; }
        void Save();

    }
}