using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimchaFund.Data;
using SimchaFund.Web.Models;

namespace SimchaFund.Web.Controllers
{
    public class SimchasController : Controller
    {
        private string _connectionString =
            @"Data Source=.\sqlexpress;Initial Catalog=SimchaFund;Integrated Security=true;";
        public IActionResult Index()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }
            var mgr = new SimchaFundManager(_connectionString);
            var viewModel = new SimchaIndexViewModel();
            viewModel.TotalContributors = mgr.GetContributorCount();
            viewModel.Simchas = mgr.GetAllSimchas();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult New(Simcha simcha)
        {
            var mgr = new SimchaFundManager(_connectionString);
            mgr.AddSimcha(simcha);
            TempData["Message"] = $"New Simcha Created! Id: {simcha.Id}";
            return RedirectToAction("index");
        }

        public IActionResult Contributions(int simchaId)
        {
            var mgr = new SimchaFundManager(_connectionString);
            Simcha simcha = mgr.GetSimchaById(simchaId);
            IEnumerable<SimchaContributor> contributors = mgr.GetSimchaContributorsOneQuery(simchaId);

            var viewModel = new ContributionsViewModel
            {
                Contributors = contributors,
                Simcha = simcha
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UpdateContributions(List<ContributionInclusion> contributors, int simchaId)
        {
            var mgr = new SimchaFundManager(_connectionString);
            mgr.UpdateSimchaContributions(simchaId, contributors);
            TempData["Message"] = "Simcha updated successfully";
            return RedirectToAction("Index");
        }
    }
}
