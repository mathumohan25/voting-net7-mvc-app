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
using ElectoralSystem.Models.RequestDtos;
using ElectoralSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;

namespace ElectoralSystem.Areas.ECs.Controllers
{
    [Area("ECs")]
    [Authorize(Roles = "EC_Admin")]
    public class StatesController : Controller
    {
        private readonly IAsyncRepository<State> _stateRepository;
        private readonly IMapper _mapper;

        public StatesController( IAsyncRepository<State> repository, IMapper mapper)
        {
            _stateRepository = repository;
            _mapper = mapper;
        }

        public async Task<IActionResult> List()
        {
            var states = await _stateRepository.GetAllAsync();
            return View(states);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
                return NotFound();

            var state = await _stateRepository.GetByIdAsync(Id.Value);
            if (state == null)
                return NotFound();
            return View(state);
        }
        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit2(int?Id, [Bind("Id,Name,NumberOfSeats")] State stateReq)
        {
            {
                try
                {
                    if (stateReq != null)
                    {
                        var stateObj = await _stateRepository.GetByIdAsync(stateReq.Id);
                        stateObj.NumberOfSeats = stateReq.NumberOfSeats;
                        await _stateRepository.UpdateAsync(stateObj);
                    }
                    TempData["success"] = "State Updated successfully";

                }
                catch (Exception e)
                {
                    TempData["error"] = "Not able to Update the state details";
                }

            }
            return RedirectToAction("List");

        }      
    }
}
