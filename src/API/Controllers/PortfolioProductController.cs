using Application.Models.PortfolioProduct.Request;
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
            catch (CustomerException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IActionResult Post(InvestmentRequest investmentRequest, long customerBankId)
        {
            try
            {
                var investmentCustomer = _appService.Invest(investmentRequest, customerBankId);
                return Created("~http://localhost:5160/api/PortfolioProducts", investmentCustomer);
            }
            catch (CustomerException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}