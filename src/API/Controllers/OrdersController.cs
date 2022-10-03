﻿using AppServices.Services.Interfaces;
using AutoMapper;
using DomainServices.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderAppService _orderAppService;

        public OrdersController(IOrderAppService appService, IMapper mapper)
        {
            _orderAppService = appService ?? throw new ArgumentNullException(nameof(appService));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var order = _orderAppService.GetAll();
            return Ok(order);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            try
            {
                var order = _orderAppService.GetOrderById(id);
                return Ok(order);
            }
            catch (NotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}