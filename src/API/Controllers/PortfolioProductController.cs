using AppServices.Services.Interfaces;
using AutoMapper;
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
        public IActionResult GetById(Guid id)
        {
            var customerFoundId = _appService.GetPortfolioProductById(id);
            return customerFoundId is null
                ? NotFound($"Customer para o id: {id} não foi encontrado.")
                : Ok(customerFoundId);
        }
    }
}
