using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Queue.DataAccess.Repository;
using Queue.Models;
using Queue_System.Hubs;
using System.Security.Claims;

namespace Queue_Interview.Controllers
{
    public class HRController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _context;
        private readonly IHubContext<QueueHub> _hubContext;

        public HRController(IUnitOfWork unitOfWork, IHttpContextAccessor context, IHubContext<QueueHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _hubContext = hubContext;
        }


        public IActionResult Serving()
        {
            try
            {
                var userLogin = _context.HttpContext.Session.GetString("User");
                var currentUser = JsonConvert.DeserializeObject<Table>(userLogin);

                return View(currentUser);
            }
            catch (Exception)
            {
                return RedirectToAction("Role", "Account");
            }
        }

        [HttpGet]   
        public async Task<IActionResult> NextQueue()
        {
            var userLogin = _context.HttpContext.Session.GetString("User");
            var currentUser = JsonConvert.DeserializeObject<Table>(userLogin);

            var TableId = currentUser?.TableId;
            var StageId = currentUser?.StageId;


            object? queueItem = null;

            switch (StageId)
            {
                case 1:
                    queueItem = _unitOfWork.Queue_Stage_1.GetAll(q => q.StatusId == 1).OrderBy(q => q.ApplicantId).FirstOrDefault();
                    break;
                case 2:
                    queueItem = _unitOfWork.Queue_Stage_2.GetAll(q => q.StatusId == 1).OrderBy(q => q.ApplicantId).FirstOrDefault();
                    break;
                case 3:
                    queueItem = _unitOfWork.Queue_Stage_3.GetAll(q => q.StatusId == 1).OrderBy(q => q.ApplicantId).FirstOrDefault();
                    break;
            }
            var ServeData = _unitOfWork.Serving.Get(q => q.TableId == TableId && q.StageId == StageId);
            if (ServeData != null)
            {
                var applicant_Status = _unitOfWork.Applicant_Status.Get(ap => ap.ApplicantId == ServeData.ApplicantId);

                if (applicant_Status != null)
                {
                    if (StageId == 1 && (applicant_Status.Stage_1 == null))
                    {
                        return Json(new { error = "Please Evaluate the applicant!" });
                    }
                    else if (StageId == 2 && (applicant_Status.Stage_2 == null))
                    {
                        return Json(new { error = "Please Evaluate the applicant!" });
                    }
                    else if (StageId == 3 && (applicant_Status.Stage_3 == null))
                    {
                        return Json(new { error = "Please Evaluate the applicant!" });
                    }
                }
                else
                {
                    return Json(new { error = "Please Evaluate the applicant!" });
                }
            }

            if (queueItem != null)
            {
                //Serving
                var servingData = _unitOfWork.Serving
                    .GetAll(u => u.TableId == TableId)
                    .OrderByDescending(q => q.Served_At)
                    .FirstOrDefault();

                if (servingData != null)
                {
                    _unitOfWork.Serving.Remove(servingData);
                    _unitOfWork.Save();

                    var newServing = new Serving
                    {
                        TableId = (int)TableId,
                        ApplicantId = GetApplicantId(queueItem),
                        StageId = (int)StageId,
                        Served_At = DateTime.Now
                    };
                    _unitOfWork.Serving.Add(newServing);
                    _unitOfWork.Save();

                    // Serve for Display Monitor
                    await _hubContext.Clients.All.SendAsync("MonitorDisplayQueue", newServing);
                }
                else
                {
                    var serving = new Serving
                    {
                        TableId = (int)TableId,
                        ApplicantId = GetApplicantId(queueItem),
                        StageId = (int)StageId,
                        Served_At = DateTime.Now
                    };
                    _unitOfWork.Serving.Add(serving);
                    _unitOfWork.Save();
                    // Serve for Display Monitor
                    await _hubContext.Clients.All.SendAsync("MonitorDisplayQueue", serving);
                }

                //Staging
                if (StageId == 1)
                {
                    _unitOfWork.Queue_Stage_1.Remove((Queue_Stage_1)queueItem);
                }
                else if (StageId == 2)
                {
                    _unitOfWork.Queue_Stage_2.Remove((Queue_Stage_2)queueItem);
                }
                else if (StageId == 3)
                {
                    _unitOfWork.Queue_Stage_3.Remove((Queue_Stage_3)queueItem);
                }

                _unitOfWork.Save();
            }
            // Update the Display of that User
            await _hubContext.Clients.All.SendAsync("DisplayQueue");
            await _hubContext.Clients.All.SendAsync("UpdateApplicantQueue");

            return Json(queueItem);
        }

        //Get the applicantId
        private int GetApplicantId(object queueItem)
        {
            if (queueItem is Queue_Stage_1 queueStage1)
                return queueStage1.ApplicantId;
            if (queueItem is Queue_Stage_2 queueStage2)
                return queueStage2.ApplicantId;
            if (queueItem is Queue_Stage_3 queueStage3)
                return queueStage3.ApplicantId;

            return 0;
        }

       //Passed
        public async Task<IActionResult> PassedQueue()
        {
            try
            {
                var userLogin = _context.HttpContext.Session.GetString("User");
                var currentUser = JsonConvert.DeserializeObject<Table>(userLogin);

                var tableId = currentUser?.TableId;
                var stageId = currentUser?.StageId;

                var servingData = _unitOfWork.Serving.Get(q => q.TableId == tableId && q.StageId == stageId);

                if (servingData != null)
                {
                    var applicantStatus = _unitOfWork.Applicant_Status.Get(ap => ap.ApplicantId == servingData.ApplicantId);

                    //Update the Applicant Status
                    if (servingData.StageId == 1)
                    {
                        var applicant = new Applicant_Status
                        {
                            ApplicantId = servingData.ApplicantId,
                            Stage_1 = "Passed",
                            Stage1_Table = currentUser?.Username
                        };
                        _unitOfWork.Applicant_Status.Add(applicant);
                    }
                    else if (servingData.StageId == 2)
                    {
                        applicantStatus.Stage_2 = "Passed";
                        applicantStatus.Stage2_Table = currentUser?.Username;
                        _unitOfWork.Applicant_Status.Update(applicantStatus);
                    }
                    else if (servingData.StageId == 3)
                    {
                        applicantStatus.Stage_3 = "Passed";
                        applicantStatus.Stage3_Table = currentUser?.Username;
                        _unitOfWork.Applicant_Status.Update(applicantStatus);
                    }
                    //Pass to Queue Stage
                    if (servingData.StageId == 1)
                    {
                        var stage2 = new Queue_Stage_2
                        {
                            ApplicantId = servingData.ApplicantId,
                            StatusId = 1,
                            StageId = 2
                        };
                        _unitOfWork.Queue_Stage_2.Add(stage2);
                        await _hubContext.Clients.All.SendAsync("UpdateApplicantQueue", stage2);
                    }
                    else if (servingData.StageId == 2)
                    {
                        var stage3 = new Queue_Stage_3
                        {
                            ApplicantId = servingData.ApplicantId,
                            StatusId = 1,
                            StageId = 3
                        };
                        _unitOfWork.Queue_Stage_3.Add(stage3);
                        await _hubContext.Clients.All.SendAsync("UpdateApplicantQueue", stage3);
                    }

                    // Table Serve Functional
                    var totalServe = _unitOfWork.Table_ServeRepo.Get(t => t.TableId == tableId && t.Served_At.Date == DateTime.Today);
                    if (totalServe != null)
                    {
                        totalServe.TotalPassed++;
                        _unitOfWork.Table_ServeRepo.Update(totalServe);
                    }
                    else
                    {
                        var tableServe = new Table_Serve
                        {
                            TableId = (int)tableId,
                            TotalPassed = 1,
                            TotalPooled = 0,
                            TotalFailed = 0,
                            Served_At = DateTime.Now,
                        };
                        _unitOfWork.Table_ServeRepo.Add(tableServe);
                    }

                    // Serve for Display Monitor
                    await _hubContext.Clients.All.SendAsync("RemoveQueueMonitor", tableId);

                    // Update the Display of that User
                    await _hubContext.Clients.All.SendAsync("DisplayQueue");
                    _unitOfWork.Serving.Remove(servingData);
                    _unitOfWork.Save();

                    return Json(Ok());
                }
                else
                {
                    return Json(null);
                }
            }
            catch (Exception)
            {
                return Json(null);
            }
        }

        //Failed
        public async Task<IActionResult> FailedQueue()
        {
            try
            {
                var userLogin = _context.HttpContext.Session.GetString("User");
                var currentUser = JsonConvert.DeserializeObject<Table>(userLogin);

                var tableId = currentUser?.TableId;
                var stageId = currentUser?.StageId;

                var servingData = _unitOfWork.Serving.Get(q => q.TableId == tableId && q.StageId == stageId);

                if (servingData != null)
                {
                    var applicantStatus = _unitOfWork.Applicant_Status.Get(ap => ap.ApplicantId == servingData.ApplicantId);

                    if (applicantStatus != null)
                    {
                        if (servingData.StageId == 2)
                        {
                            applicantStatus.Stage_2 = "Pooled";
                            applicantStatus.Stage2_Table = currentUser?.Username;
                        }
                        else if (servingData.StageId == 3)
                        {
                            applicantStatus.Stage_3 = "Pooled";
                            applicantStatus.Stage3_Table = currentUser?.Username;
                        }
                        _unitOfWork.Applicant_Status.Update(applicantStatus);
                    }
                    else
                    {
                        if (servingData.StageId == 1)
                        {
                            var applicant = new Applicant_Status
                            {
                                ApplicantId = servingData.ApplicantId,
                                Stage_1 = "Pooled",
                                Stage1_Table = currentUser?.Username,   
                            };
                            _unitOfWork.Applicant_Status.Add(applicant);
                        }
                    }

                    // Table Serve Functional
                    var totalServe = _unitOfWork.Table_ServeRepo.Get(t => t.TableId == tableId && t.Served_At.Date == DateTime.Today);
                    if (totalServe != null)
                    {
                        totalServe.TotalPooled+=1;
                        _unitOfWork.Table_ServeRepo.Update(totalServe);
                    }
                    else
                    {
                        var tableServe = new Table_Serve
                        {
                            TableId = (int)tableId,
                            TotalPooled = 1,
                            TotalPassed = 0,
                            TotalFailed = 0,
                            Served_At = DateTime.Now,
                        };
                        _unitOfWork.Table_ServeRepo.Add(tableServe);
                    }

                    _unitOfWork.Serving.Remove(servingData);
                    _unitOfWork.Save();

                    // Update the Display of that User
                    await _hubContext.Clients.All.SendAsync("DisplayQueue");
                    // Serve for Display Monitor
                    await _hubContext.Clients.All.SendAsync("RemoveQueueMonitor", tableId);

                    return Json(Ok());
                }
                else
                {
                    return Json(null);
                }
            }
            catch (Exception)
            {
                return Json(null);
            }
        }

        //Get All Waiting
        public IActionResult GetAllWaitingApplicants()
        {
            try
            {
                // Get the userId of the current user
                var userLogin = _context.HttpContext.Session.GetString("User");
                var currentUser = JsonConvert.DeserializeObject<Table>(userLogin);
                var stageId = currentUser?.StageId;

                if (currentUser == null)
                {
                    stageId = 1;
                }

                var status = _unitOfWork.QueueStatus.GetAll();
                var applicants = _unitOfWork.Applicant.GetAll();

                List<Queue_Stage_1> waitingStage1 = null;
                List<Queue_Stage_2> waitingStage2 = null;
                List<Queue_Stage_3> waitingStage3 = null;

                if (stageId.HasValue)
                {
                    switch (stageId.Value)
                    {
                        case 1:
                            waitingStage1 = _unitOfWork.Queue_Stage_1.GetAll(t => t.StatusId == 1).OrderBy(t => t.ApplicantId).ToList();
                            return Json(waitingStage1);
                        case 2:
                            waitingStage2 = _unitOfWork.Queue_Stage_2.GetAll(t => t.StatusId == 1).OrderBy(t => t.ApplicantId).ToList();
                            return Json(waitingStage2);
                        case 3:
                            waitingStage3 = _unitOfWork.Queue_Stage_3.GetAll(t => t.StatusId == 1).OrderBy(t => t.ApplicantId).ToList();
                            return Json(waitingStage3);
                    }
                }

                return Json(null);
            }catch (Exception )
            {
                return Json(null);
            }
        }

        public IActionResult GetWaitingApplicantStage1()
        {
            var applicants = _unitOfWork.Applicant.GetAll();
            var waitingStage1 = _unitOfWork.Queue_Stage_1.GetAll(t => t.StatusId == 1).OrderBy(t => t.ApplicantId).ToList();
            return Json(waitingStage1);
        }

        //Get the QueueServe
        public async Task<IActionResult> QueueServeDisplay()
        {
            try
            {
                var userLogin = _context.HttpContext.Session.GetString("User");
                var currentUser = JsonConvert.DeserializeObject<Table>(userLogin);

                var TableId = currentUser?.TableId;
                var StageId = currentUser?.StageId;

                var Queue = _unitOfWork.Serving.Get(q => q.TableId == TableId && q.StageId == StageId);

                return Json(Queue);
            }
            catch (Exception e)
            {
                return Json(null, e);
            }
            
        }

        //Call the QueueServe
        public async Task<IActionResult> QueueCallDisplay()
        {
            try
            {
                var userLogin = _context.HttpContext.Session.GetString("User");
                var currentUser = JsonConvert.DeserializeObject<Table>(userLogin);

                var TableId = currentUser?.TableId;
                var StageId = currentUser?.StageId;

                var stage = _unitOfWork.Stages.Get(s => s.StageId == StageId);
                var tables = _unitOfWork.Tables.GetAll();
                var Queue = _unitOfWork.Serving.Get(q => q.TableId == TableId && q.StageId == StageId);

                if(Queue != null)
                {
                    // Call and Blink Queue Applicant for Display Monitor
                    await _hubContext.Clients.All.SendAsync("CallQueueApplicant", Queue, currentUser?.Username, stage.StageName);

                    return Json(new { queue = Queue, tableName = currentUser?.Username, stageName = stage.StageName });
                }
                else
                {
                    return Json(null);
                }
               
            }
            catch (Exception e)
            {
                return Json(null, e);
            }
        }


        //Temporary Reject
        public async Task<IActionResult> TempRejectQueue()
        {
            try
            {
                var userLogin = _context.HttpContext.Session.GetString("User");
                var currentUser = JsonConvert.DeserializeObject<Table>(userLogin);

                var tableId = currentUser?.TableId;
                var stageId = currentUser?.StageId;

                var servingData = _unitOfWork.Serving.Get(q => q.TableId == tableId && q.StageId == stageId);

                if (servingData != null)
                {
                    //Pass to Queue Stage
                    if (servingData.StageId == 1)
                    {
                        var stage = new Queue_Stage_1
                        {
                            ApplicantId = servingData.ApplicantId,
                            StatusId = 2,
                            StageId = 1,
                            TemporaryRejected_At = DateTime.Now,
                        };
                        _unitOfWork.Queue_Stage_1.Add(stage);
                    }
                    else if (servingData.StageId == 2)
                    {
                        var stage = new Queue_Stage_2
                        {
                            ApplicantId = servingData.ApplicantId,
                            StatusId = 2,
                            StageId = 1,
                            TemporaryRejected_At = DateTime.Now,
                        };
                        _unitOfWork.Queue_Stage_2.Add(stage);
                    }
                    else if (servingData.StageId == 3)
                    {
                        var stage = new Queue_Stage_3
                        {
                            ApplicantId = servingData.ApplicantId,
                            StatusId = 2,
                            StageId = 1,
                            TemporaryRejected_At = DateTime.Now,
                        };
                        _unitOfWork.Queue_Stage_3.Add(stage);
                    }



                    _unitOfWork.Serving.Remove(servingData);
                    _unitOfWork.Save();
                    //Load All Temporary Reject Applicant
                    await _hubContext.Clients.All.SendAsync("UpdateApplicantTempReject");
                    // Serve for Display Monitor
                    await _hubContext.Clients.All.SendAsync("RemoveQueueMonitor", tableId);
                    // Update the Display of that User
                    await _hubContext.Clients.All.SendAsync("DisplayQueue");

                    return Json(Ok());
                }
                else
                {
                    return Json(null);
                }
            }
            catch (Exception)
            {
                return Json(null);
            }
        }

        //GET All TEMPORARY REJECT APPLICANTS
        public IActionResult GetAllTempRejectApplicants()
        {
            try
            {
                // Get the userId of the current user
                var userLogin = _context.HttpContext.Session.GetString("User");
                var currentUser = JsonConvert.DeserializeObject<Table>(userLogin);
                var stageId = currentUser?.StageId;

                var status = _unitOfWork.QueueStatus.GetAll();
                var applicants = _unitOfWork.Applicant.GetAll();

                List<Queue_Stage_1> waitingStage1 = null;
                List<Queue_Stage_2> waitingStage2 = null;
                List<Queue_Stage_3> waitingStage3 = null;

                if (stageId.HasValue)
                {
                    switch (stageId.Value)
                    {
                        case 1:
                            waitingStage1 = _unitOfWork.Queue_Stage_1.GetAll(t => t.StatusId == 2).OrderBy(t => t.ApplicantId).ToList();
                            return Json(waitingStage1);
                        case 2:
                            waitingStage2 = _unitOfWork.Queue_Stage_2.GetAll(t => t.StatusId == 2).OrderBy(t => t.ApplicantId).ToList();
                            return Json(waitingStage2);
                        case 3:
                            waitingStage3 = _unitOfWork.Queue_Stage_3.GetAll(t => t.StatusId == 2).OrderBy(t => t.ApplicantId).ToList();
                            return Json(waitingStage3);
                    }
                }

                return Json(null);
            }
            catch (Exception)
            {
                return Json(null);
            }
        }

        //REJECT/FAILED THE TEMPORARY REJECT
        public async Task<IActionResult> RejectQueue(int id)
        {
            try
            {
                var userLogin = _context.HttpContext.Session.GetString("User");
                var currentUser = JsonConvert.DeserializeObject<Table>(userLogin);

                var tableId = currentUser?.TableId;
                var stageId = currentUser?.StageId;

                if(stageId != null)
                {
                    //Remove Applicant in Stage 1
                    if (stageId == 1)
                    {
                        var temporaryRejectStage = _unitOfWork.Queue_Stage_1.Get(t => t.ApplicantId == id);
                        _unitOfWork.Queue_Stage_1.Remove(temporaryRejectStage);
                    }
                    else if (stageId == 2)
                    {
                        var temporaryRejectStage = _unitOfWork.Queue_Stage_2.Get(t => t.ApplicantId == id);
                        _unitOfWork.Queue_Stage_2.Remove(temporaryRejectStage);   
                    }
                    else if (stageId == 3)
                    {
                        var temporaryRejectStage = _unitOfWork.Queue_Stage_3.Get(t => t.ApplicantId == id);
                        _unitOfWork.Queue_Stage_3.Remove(temporaryRejectStage);
                    }

                    // Table Serve Functional
                    var tableIdExist = _unitOfWork.Table_ServeRepo.Get(t => t.TableId == tableId && t.Served_At.Date == DateTime.Today);
                    if (tableIdExist != null)
                    {
                        tableIdExist.TotalFailed++;
                        _unitOfWork.Table_ServeRepo.Update(tableIdExist);
                    }
                    else
                    {
                        var tableServe = new Table_Serve
                        {
                            TableId = (int)tableId,
                            TotalFailed = 1,
                            TotalPooled = 0,
                            TotalPassed = 0,
                            Served_At = DateTime.Now,
                        };
                        _unitOfWork.Table_ServeRepo.Add(tableServe);
                    }

                    //Update the Applicant Status
                    var applicantStatus = _unitOfWork.Applicant_Status.Get(ap => ap.ApplicantId == id);
                    if (stageId == 1)
                    {
                        var applicant = new Applicant_Status
                        {
                            ApplicantId = id,
                            Stage_1 = "Failed",
                            Stage1_Table = currentUser?.Username
                        };
                        _unitOfWork.Applicant_Status.Add(applicant);
                    }
                    else if (stageId == 2)
                    {
                        applicantStatus.Stage_2 = "Failed";
                        applicantStatus.Stage2_Table = currentUser?.Username;
                        _unitOfWork.Applicant_Status.Update(applicantStatus);
                    }
                    else if (stageId == 3)
                    {
                        applicantStatus.Stage_3 = "Failed";
                        applicantStatus.Stage3_Table = currentUser?.Username;
                        _unitOfWork.Applicant_Status.Update(applicantStatus);
                    }

                    _unitOfWork.Save();

                    //Load All Temporary Reject Applicant
                    await _hubContext.Clients.All.SendAsync("UpdateApplicantTempReject");
                    // Serve for Display Monitor
                    await _hubContext.Clients.All.SendAsync("RemoveQueueMonitor", tableId);
                    // Update the Display of that User
                    await _hubContext.Clients.All.SendAsync("DisplayQueue");

                    return Json(Ok());
                }
                else
                {
                    return Json(null);
                }
            }
            catch (Exception)
            {
                return Json(null);
            }
        }

        //SERVED TEMPORARY REJECT APPLICANT
        public async Task<IActionResult> ServeTempRejectQueue(int id)
        {
            try
            {
                var userLogin = _context.HttpContext.Session.GetString("User");
                var currentUser = JsonConvert.DeserializeObject<Table>(userLogin);

                var tableId = currentUser?.TableId;
                var stageId = currentUser?.StageId;

                if (stageId != null)
                {
                    //EVALUATE
                    var ServeData = _unitOfWork.Serving.Get(q => q.TableId == tableId && q.StageId == stageId);
                    if (ServeData != null)
                    {
                        var applicant_Status = _unitOfWork.Applicant_Status.Get(ap => ap.ApplicantId == ServeData.ApplicantId);

                        if (applicant_Status != null)
                        {
                            if (stageId == 1 && (applicant_Status.Stage_1 == null))
                            {
                                return Json(new { error = "Please Evaluate the applicant!" });
                            }
                            else if (stageId == 2 && (applicant_Status.Stage_2 == null))
                            {
                                return Json(new { error = "Please Evaluate the applicant!" });
                            }
                            else if (stageId == 3 && (applicant_Status.Stage_3 == null))
                            {
                                return Json(new { error = "Please Evaluate the applicant!" });
                            }
                        }
                        else
                        {
                            return Json(new { error = "Please Evaluate the applicant!" });
                        }
                    }
                    else
                    {
                        //REMOVE
                        if (stageId == 1)
                        {
                            var temporaryRejectStage = _unitOfWork.Queue_Stage_1.Get(t => t.ApplicantId == id);
                            _unitOfWork.Queue_Stage_1.Remove(temporaryRejectStage);
                        }
                        else if (stageId == 2)
                        {
                            var temporaryRejectStage = _unitOfWork.Queue_Stage_2.Get(t => t.ApplicantId == id);
                            _unitOfWork.Queue_Stage_2.Remove(temporaryRejectStage);
                        }
                        else if (stageId == 3)
                        {
                            var temporaryRejectStage = _unitOfWork.Queue_Stage_3.Get(t => t.ApplicantId == id);
                            _unitOfWork.Queue_Stage_3.Remove(temporaryRejectStage);
                        }
                        //ADD SERVE
                        var newServing = new Serving
                        {
                            TableId = (int)tableId,
                            ApplicantId = id,
                            StageId = (int)stageId,
                            Served_At = DateTime.Now
                        };
                        _unitOfWork.Serving.Add(newServing);
                        // Serve for Display Monitor
                        await _hubContext.Clients.All.SendAsync("MonitorDisplayQueue", newServing);
                    }
                    _unitOfWork.Save();
                    // Update the Display of that User
                    await _hubContext.Clients.All.SendAsync("DisplayQueue");
                    await _hubContext.Clients.All.SendAsync("UpdateApplicantTempReject");
                    return Json(Ok());
                }
                else
                {
                    return Json(null);
                }
            }
            catch (Exception)
            {
                return Json(null);
            }
        }




    }
}