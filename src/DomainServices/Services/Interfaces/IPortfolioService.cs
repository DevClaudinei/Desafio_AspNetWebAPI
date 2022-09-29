using Application.Models.Order;
using DomainModels.Entities;
using System.Collections.Generic;

namespace DomainServices.Services.Interfaces;

public interface IPortfolioService
{
    long Create(Portfolio portfolio);
    IEnumerable<Portfolio> GetAll();
    Portfolio GetById(long id);
    decimal GetTotalBalance(long portfolioId);
    public bool Update(Portfolio portfolio);
    void Delete(long id);
    public void AddProduct(Portfolio portfolio, Product product);
    public void RemoveProduct(Portfolio portfolio, Product product);
    void UpdateWithdraw(Order order, long productId, Portfolio portfolio);
}