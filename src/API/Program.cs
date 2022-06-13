using System;
using API.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddServicesConfiguration();
builder.Services.AddAutoMapperConfiguration();
builder.Services.AddMvcConfiguration();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API - Backend para criação de cliente",
        Description = "API REST criada com o ASP.NET Core para manipulações de informações referentes a cliente",
        Contact = new OpenApiContact()
        {
            Name = "Claudinei José Santos",
            Email = "claudinei.santos@warren.com.br",
            Url = new Uri("httts://github.com/santosclaudinei-warren/Desafio_AspNetWebAPI")
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(ui =>
    {
        ui.SwaggerEndpoint("./v1/swagger.json", "API - Backend para cadastro de cliente");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();