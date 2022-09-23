﻿using DomainModels.Entities;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;

namespace DomainServices.Services;

public class PortfolioProductService : IPortfolioProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryFactory _repositoryFactory;

    public PortfolioProductService(IUnitOfWork<ApplicationDbContext> unitOfWork, IRepositoryFactory<ApplicationDbContext> repositoryFactory)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
    }

    public long Create(PortfolioProduct portfolioProduct)
    {
        var repository = _unitOfWork.Repository<PortfolioProduct>();
        portfolioProduct.ConvertedAt = DateTime.Now;

        repository.Add(portfolioProduct);
        _unitOfWork.SaveChanges();

        return portfolioProduct.Id;
    }

    public IEnumerable<PortfolioProduct> GetAll()
    {
        var repository = _repositoryFactory.Repository<PortfolioProduct>();
        var query = repository.MultipleResultQuery();

        return repository.Search(query);
    }

    public PortfolioProduct GetById(long id)
    {
        var repository = _repositoryFactory.Repository<PortfolioProduct>();
        var customerFound = repository.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id));

        return repository.SingleOrDefault(customerFound);
    }
}