using Application.Models.Product.Request;
using Application.Models.Product.Response;
using AutoMapper;
using DomainModels.Entities;

namespace AppServices.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductResult>();
        CreateMap<ProductResult, Product>();
        CreateMap<UpdateProductRequest, Product>();
        CreateMap<CreateProductRequest, Product>();
    }
}
