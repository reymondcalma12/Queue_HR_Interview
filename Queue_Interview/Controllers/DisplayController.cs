using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Queue.DataAccess.Repository;
using Queue.Models;
using Queue.Models.ViewModel;
using Queue_System.Hubs;

namespace Queue_Interview.Controllers
{
    public class DisplayController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<QueueHub> _hubContext;

        public DisplayController(IUnitOfWork unitOfWork, IHubContext<QueueHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        public IActionResult Pre_Screening()
        {
            var servingVM = new ServingVM
            {
                Tables = _unitOfWork.Tables.GetAll(t => t.StageId == 1),
                Servings = _unitOfWork.Serving.GetAll().ToList(),
            };

            return View(servingVM);
        }

        public IActionResult Initial_Interview()
        {
            var servingVM = new ServingVM
            {
                Tables = _unitOfWork.Tables.GetAll(t => t.StageId == 2),
                Servings = _unitOfWork.Serving.GetAll().ToList(),
            };

            return View(servingVM);
        }

        public IActionResult Final_Interview()
        {
            var servingVM = new ServingVM
            {
                Tables = _unitOfWork.Tables.GetAll(t => t.StageId == 3),
                Servings = _unitOfWork.Serving.GetAll().ToList(),
            };

            return View(servingVM);
        }




    }
}
