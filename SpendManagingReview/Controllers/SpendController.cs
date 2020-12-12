using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SpendManagingReview.Models;

namespace SpendManagingReview.Controllers
{
    public class SpendController : Controller
    {
        SpendDAL objSpend = new SpendDAL();
        public IActionResult Index(string searchString)
        {
            List<SpendReport> lstEmployee = new List<SpendReport>();
            lstEmployee = objSpend.GetAllSpend().ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                lstEmployee = objSpend.GetSearchResult(searchString).ToList();
            }
            return View(lstEmployee);
        }

        public ActionResult AddEditSpend(int itemId)
        {
            SpendReport model = new SpendReport();
            if (itemId > 0)
            {
                model = objSpend.GetSpendData(itemId);
            }
            return PartialView("_spendForm", model);
        }

        [HttpPost]
        public ActionResult Create(SpendReport newSpend)
        {
            if (ModelState.IsValid)
            {
                if (newSpend.ItemId > 0)
                {
                    objSpend.UpdateSpend(newSpend);
                }
                else
                {
                    objSpend.AddSpend(newSpend);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            objSpend.DeleteSpend(id);
            return RedirectToAction("Index");
        }

        public ActionResult SpendSummary()
        {
            return PartialView("_spendReport");
        }

        public JsonResult GetMonthlySpend()
        {
            Dictionary<string, decimal> monthlySpend = objSpend.CalculateMonthlySpend();
            return new JsonResult(monthlySpend);
        }

        public JsonResult GetWeeklySpend()
        {
            Dictionary<string, decimal> weeklySpend = objSpend.CalculateWeeklySpend();
            return new JsonResult(weeklySpend);
        }
    }
}
