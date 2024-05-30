using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Queue.DataAccess.Repository;
using Queue.Models;

namespace Queue_Interview.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _context;

        public AccountController(IUnitOfWork unitOfWork, IHttpContextAccessor contx)
        {
            _unitOfWork = unitOfWork;
            _context = contx;
        }

        public IActionResult Role()
        {
            try
            {
                var userLogin = _context.HttpContext.Session.GetString("User");
                var currentUser = JsonConvert.DeserializeObject<Table>(userLogin);

                if(currentUser != null)
                {
                    return RedirectToAction("Serving", "HR");

                }
                else
                {
                    return View();
                }

            }
            catch (Exception)
            {
                return View();
            }


        }


        [HttpPost]
        public IActionResult Role(Table model)
        {
            if (ModelState.IsValid)
            {
                var userDb = _unitOfWork.Tables.Get(u => u.TableId == model.TableId);

                // Store the selected table ID in the session
                string user = JsonConvert.SerializeObject(userDb);
                _context.HttpContext.Session.SetString("User", user);

                var userLogin = _context.HttpContext.Session.GetString("User");

                if (!string.IsNullOrEmpty(userLogin))
                {
                    return RedirectToAction("Serving", "HR");
                }

            }

            return View();


        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Role", "Account");
        }






        // Fetching data from DB
        public IActionResult GetStages()
        {
            var stage = _unitOfWork.Stages.GetAll().ToList().OrderBy(s => s.StageName);
            return Json(stage);
        }

        public IActionResult GetTables(int id)
        {
            var table = _unitOfWork.Tables.GetAll(t => t.StageId == id == null).ToList().OrderBy(s => s.TableId);
            return Json(table);
        }

    }


}
