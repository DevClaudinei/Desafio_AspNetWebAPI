using System;
using System.ComponentModel;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjetoWarren.WebAPI.Validators;
using SmartSchool.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddControllers()

.AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<CustomerValidator>());

builder.Services.AddScoped<ICustomerService, CustomerService>();

TypeDescriptor.AddAttributes(typeof(DateTime), new TypeConverterAttribute(typeof(DateTimeConverter)));

// builder.Services.AddScoped<IAlunoService, >

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
