using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpendManagingReview.Models
{
    public class SpendDAL
    {
        SpendDBContext db = new SpendDBContext();
        public IEnumerable<SpendReport> GetAllSpend()
        {
            try
            {
                return db.SpendReport.ToList();
            }
            catch
            {
                throw;
            }
        }

        // To filter out the records based on the search string 
        public IEnumerable<SpendReport> GetSearchResult(string searchString)
        {
            List<SpendReport> exp = new List<SpendReport>();
            try
            {
                exp = GetAllSpend().ToList();
                return exp.Where(x => x.ItemName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1);
            }
            catch
            {
                throw;
            }
        }

        //To Add new Spend record       
        public void AddSpend(SpendReport Spend)
        {
            try
            {
                db.SpendReport.Add(Spend);
                db.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        //To Update the records of a particluar Spend  
        public int UpdateSpend(SpendReport Spend)
        {
            try
            {
                db.Entry(Spend).State = EntityState.Modified;
                db.SaveChanges();

                return 1;
            }
            catch
            {
                throw;
            }
        }

        //Get the data for a particular Spend  
        public SpendReport GetSpendData(int id)
        {
            try
            {
                SpendReport spend = db.SpendReport.Find(id);
                return spend;
            }
            catch
            {
                throw;
            }
        }

        //To Delete the record of a particular Spend  
        public void DeleteSpend(int id)
        {
            try
            {
                SpendReport emp = db.SpendReport.Find(id);
                db.SpendReport.Remove(emp);
                db.SaveChanges();

            }
            catch
            {
                throw;
            }
        }

        // To calculate last six months Spend
        public Dictionary<string, decimal> CalculateMonthlySpend()
        {
            SpendDAL objSpend = new SpendDAL();
            List<SpendReport> lstEmployee = new List<SpendReport>();

            Dictionary<string, decimal> dictMonthlySum = new Dictionary<string, decimal>();

            decimal foodSum = db.SpendReport.Where
                (cat => cat.Category == "Food" && (cat.SpendDate > DateTime.Now.AddMonths(-7)))
                .Select(cat => cat.Amount)
                .Sum();

            decimal shoppingSum = db.SpendReport.Where
               (cat => cat.Category == "Shopping" && (cat.SpendDate > DateTime.Now.AddMonths(-7)))
               .Select(cat => cat.Amount)
               .Sum();

            decimal travelSum = db.SpendReport.Where
               (cat => cat.Category == "Travel" && (cat.SpendDate > DateTime.Now.AddMonths(-7)))
               .Select(cat => cat.Amount)
               .Sum();

            decimal healthSum = db.SpendReport.Where
               (cat => cat.Category == "Health" && (cat.SpendDate > DateTime.Now.AddMonths(-7)))
               .Select(cat => cat.Amount)
               .Sum();

            dictMonthlySum.Add("Food", foodSum);
            dictMonthlySum.Add("Shopping", shoppingSum);
            dictMonthlySum.Add("Travel", travelSum);
            dictMonthlySum.Add("Health", healthSum);

            return dictMonthlySum;
        }

        // To calculate last four weeks Spend
        public Dictionary<string, decimal> CalculateWeeklySpend()
        {
            SpendDAL objSpend = new SpendDAL();
            List<SpendReport> lstEmployee = new List<SpendReport>();

            Dictionary<string, decimal> dictWeeklySum = new Dictionary<string, decimal>();

            decimal foodSum = db.SpendReport.Where
                (cat => cat.Category == "Food" && (cat.SpendDate > DateTime.Now.AddDays(-7)))
                .Select(cat => cat.Amount)
                .Sum();

            decimal shoppingSum = db.SpendReport.Where
               (cat => cat.Category == "Shopping" && (cat.SpendDate > DateTime.Now.AddDays(-28)))
               .Select(cat => cat.Amount)
               .Sum();

            decimal travelSum = db.SpendReport.Where
               (cat => cat.Category == "Travel" && (cat.SpendDate > DateTime.Now.AddDays(-28)))
               .Select(cat => cat.Amount)
               .Sum();

            decimal healthSum = db.SpendReport.Where
               (cat => cat.Category == "Health" && (cat.SpendDate > DateTime.Now.AddDays(-28)))
               .Select(cat => cat.Amount)
               .Sum();

            dictWeeklySum.Add("Food", foodSum);
            dictWeeklySum.Add("Shopping", shoppingSum);
            dictWeeklySum.Add("Travel", travelSum);
            dictWeeklySum.Add("Health", healthSum);

            return dictWeeklySum;
        }
    }
}
