using MediatR;
using PowerPlant.Api.Service.ProductionPlanCalculate.Models;
using PowerPlant.Api.Utilities;

namespace PowerPlant.Api.Service.ProductionPlanCalculate;

public class ProductionPlanCalculateRequestHandler : IRequestHandler<ProductionPlanCalculateRequest, List<ProductionPlanResponseModel>>
{
    private readonly IProductionPlanCalculator _productionPlanCalculator;

    public ProductionPlanCalculateRequestHandler(IProductionPlanCalculator productionPlanCalculator)
    {
        _productionPlanCalculator = productionPlanCalculator;
    }

    public Task<List<ProductionPlanResponseModel>> Handle(ProductionPlanCalculateRequest request, CancellationToken cancellationToken)
    {
        var response = _productionPlanCalculator.CalculateProductionPlan(request.Model);
        return Task.FromResult(response);
    }
}
