using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectoralSystem.Data;
using ElectoralSystem.Models.Entities;
using AutoMapper;
using ElectoralSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ElectoralSystem.Areas.ECs.Controllers
{
    [Area("ECs")]
    [Authorize(Roles = "EC_Admin")]
    public class VotersController : Controller
    {
        private readonly IAsyncRepository<Voter> _voterRepository;
        private readonly IMapper _mapper;

        public VotersController(IAsyncRepository<Voter> repository, IMapper mapper)
        {
            _voterRepository = repository;
            _mapper = mapper;
        }

        public async Task<IActionResult> List()
        {
            var pendingVoters = await _voterRepository.GetAsync(x=> !x.IsEligible,
                null, nameof(Voter.State),false);
            return View(pendingVoters);
        }

        [HttpGet]
        public async Task<IActionResult> Approve(int? id)
        {
            if (id == null)
                return NotFound();

            var voter = await _voterRepository.GetAsync(x=> x.Id == id.Value, null,
                nameof(Voter.State), false);
            if (voter == null || voter.Count <=0)
                return NotFound();
            return View(voter.First());
        }
        [HttpPost]
        [ActionName("Approve")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve2(Voter voterdto)
        {
            //if (ModelState.IsValid)
            {
                try
                {
                    Voter voter = await _voterRepository.GetByIdAsync(voterdto.Id);
                    if (voter != null)
                    {
                        voter.IsEligible = true;
                        await _voterRepository.UpdateAsync(voter);
                    }
                    TempData["success"] = "Voter approved successfully";

                }
                catch (Exception e)
                {
                    TempData["error"] = "Not able to approve the voter";
                }

            }
            return RedirectToAction("List");

        }
    }
}
