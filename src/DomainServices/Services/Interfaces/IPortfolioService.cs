using DomainModels.Entities;
using System.Collections.Generic;

namespace DomainServices.Services.Interfaces;

public interface IPortfolioService
{
    long Create(Portfolio portfolio);
    IEnumerable<Portfolio> GetAll();
    Portfolio GetById(long id);
    decimal GetTotalBalance(long portfolioId);
    void Update(Portfolio portfolio);
    void Delete(long id);
    IEnumerable<Portfolio> GetAllByCustomerId(long id);
}