using PowerPlant.Api.Service.ProductionPlanCalculate.Models;

namespace PowerPlant.Api.Utilities;

public interface IProductionPlanCalculator
{
    List<ProductionPlanResponseModel> CalculateProductionPlan(ProductionPlanRequestModel productionPlanRequest);
}