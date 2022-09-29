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
        private readonly IOrderAppService _orderAppService;

        public PortfolioProductController(IOrderAppService appService, IMapper mapper)
        {
            _orderAppService = appService ?? throw new ArgumentNullException(nameof(appService));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var portfolioProductsFound = _orderAppService.GetAllOrder();
            return Ok(portfolioProductsFound);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            try
            {
                var portfolioProductFound = _orderAppService.GetOrderById(id);
                return Ok(portfolioProductFound);
            }
            catch (NotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}