﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.RepositoryServices;
using Infrastructure.RepositoryServices.Exceptions;
using Domain.UseCase.UserServices;
using Domain.ViewModel;
using Domain.UseCase;
using Infrastructure.PdfServices;
using API;

namespace api.Controllers
{
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly EntityService _entityService;
        private readonly ScheduleService _scheduleService;
        private readonly ILogger<SchedulesController> _logger;

        public SchedulesController(ILogger<SchedulesController> logger)
        {
            _logger = logger;
            _entityService = new EntityService(new EntityRepository());
            _scheduleService = new ScheduleService(new EntityRepository());
        }

        [HttpPost]
        [Route("/agendamento/simulacao")]
        [Authorize(Roles = "User, Operator")]
        public async Task<IActionResult> Simulation([FromBody] VehicleScheduleSimulationInput schedule)
        {
            try
            {
                return StatusCode(200, await _scheduleService.Simulation(schedule));
            }
            catch (EntityNotFound err)
            {
                return StatusCode(403, new
                {
                    Message = err.Message
                });
            }
            catch (EntityUniq err)
            {
                return StatusCode(401, new
                {
                    Message = err.Message
                });
            }
        }

        [HttpPost]
        [Route("/agendamento/alugar")]
        [Authorize(Roles = "Operator, User")]
        public async Task<IActionResult> BookCar([FromBody] ScheduleInput schedule)
        {
            try
            {
                var path = Startup.ContentRoot;
                var scheduleOut = await _scheduleService.BookCar(schedule, new PdfWriter(), path);
                return StatusCode(201, scheduleOut);
            }
            catch (EntityNotFound err)
            {
                return StatusCode(401, new
                {
                    Message = err.Message
                });
            }
        }

        [HttpGet]
        [Route("/agendamento/consulta/{cpf}")]
        [Authorize(Roles = "Operator, User")]
        public async Task<IActionResult> GetByCPF(string cpf)
        {
            try
            {
                var path = Startup.ContentRoot;
                var scheduleOut = await _scheduleService.GetByCPF(cpf, new PdfWriter(), path);
                return StatusCode(201, scheduleOut);
            }
            catch (EntityNotFound err)
            {
                return StatusCode(401, new
                {
                    Message = err.Message
                });
            }
        }

        [HttpPost]
        [Route("/agendamento/devolucao")]
        [Authorize(Roles = "Operator")]
        public async Task<IActionResult> ReturnPayment([FromBody] Checklist checklist)
        {
            try
            {
                var path = Startup.ContentRoot;
                var schedulePaymentOut = await _scheduleService.ReturnPayment(checklist, new PdfWriter(), path);
                return StatusCode(201, schedulePaymentOut);
            }
            catch (EntityNotFound err)
            {
                return StatusCode(401, new
                {
                    Message = err.Message
                });
            }
        }
    }
}