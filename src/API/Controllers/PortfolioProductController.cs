using AppServices.Services.Interfaces;
using AutoMapper;
using DomainServices.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioProductController : Controller
    {
        private readonly IPortfolioProductAppService _appService;

        public PortfolioProductController(IPortfolioProductAppService appService, IMapper mapper)
        {
            _appService = appService ?? throw new ArgumentNullException(nameof(appService));
        }

        [HttpGet]
        public IActionResult Get()
        {
            var customersFound = _appService.GetAllPortfolioProduct();
            return Ok(customersFound);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            try
            {
                var customerFoundId = _appService.GetPortfolioProductById(id);
                return Ok(customerFoundId);
            }
            catch (NotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}