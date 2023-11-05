using MediatR;
using PowerPlant.Api.Service.ProductionPlanCalculate.Models;

namespace PowerPlant.Api.Service.ProductionPlanCalculate;

public class ProductionPlanCalculateRequest : IRequest<List<ProductionPlanResponseModel>>
{
    public ProductionPlanRequestModel Model { get; }

    public ProductionPlanCalculateRequest(ProductionPlanRequestModel model)
    {
        Model = model;
    }
}
