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
        private readonly IPortfolioProductAppService _portfolioProductService;

        public PortfolioProductController(IPortfolioProductAppService appService, IMapper mapper)
        {
            _portfolioProductService = appService ?? throw new ArgumentNullException(nameof(appService));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var portfolioProductsFound = _portfolioProductService.GetAll();
            return Ok(portfolioProductsFound);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            try
            {
                var portfolioProductFound = _portfolioProductService.GetById(id);
                return Ok(portfolioProductFound);
            }
            catch (NotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}