using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Queue.DataAccess.Repository;
using Queue.Models;
using Queue_System.Hubs;

namespace Queue_Interview.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<QueueHub> _hubContext;

        public HomeController(IUnitOfWork unitOfWork, IHubContext<QueueHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ApplicationForm(int id)
        {
            var applicant = _unitOfWork.Applicant.Get(a=>a.ApplicantId == id);
            if (applicant == null)
            {
                return NotFound();
            }

            return View(applicant);
        }

        [HttpPost]
        public async Task<IActionResult> AddApplicant(Applicant_Form model)
        {
            if (ModelState.IsValid)
            {
                // Add in the Application Form
                _unitOfWork.Applicant.Add(model);
                // Add Applicant in the Stage 1 
                _unitOfWork.Save();
                var stage1 = new Queue_Stage_1
                {
                    ApplicantId = (int)model.ApplicantId,
                    StatusId = 1,
                    StageId = 1
                };
                _unitOfWork.Queue_Stage_1.Add(stage1);
                _unitOfWork.Save();

                await _hubContext.Clients.All.SendAsync("UpdateWaitingStage1");

                // Redirect to the ApplicationForm page with the ID parameter
                return RedirectToAction("ApplicationForm", "Home", new { id = model.ApplicantId});
            }

            return RedirectToAction("Index", "Home");
        }



    }
}
