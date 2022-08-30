using Application.Models.PortfolioProduct.Request;
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
        public IActionResult GetById(long id)
        {
            var customerFoundId = _appService.GetPortfolioProductById(id);
            return customerFoundId is null
                ? NotFound($"Customer para o id: {id} não foi encontrado.")
                : Ok(customerFoundId);
        }

        [HttpPost]
        public IActionResult Post(InvestmentRequest investmentRequest, long customerBankId)
        {
            var investmentCustomer = _appService.Invest(investmentRequest, customerBankId);
            return investmentCustomer.isValid
                ? Created("~http://localhost:5160/api/PortfolioProducts", investmentCustomer.message)
                : BadRequest(investmentCustomer.message);
        }
    }
}
