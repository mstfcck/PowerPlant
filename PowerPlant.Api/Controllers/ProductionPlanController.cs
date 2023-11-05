using MediatR;
using Microsoft.AspNetCore.Mvc;
using PowerPlant.Api.Service.ProductionPlanCalculate;
using PowerPlant.Api.Service.ProductionPlanCalculate.Models;

namespace PowerPlant.Api.Controllers;

[ApiController]
[Route("productionplan")]
public class ProductionPlanController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductionPlanController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(List<ProductionPlanResponseModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CalculateProductionPlan([FromBody] ProductionPlanRequestModel request)
    {
        var response = await _mediator.Send(new ProductionPlanCalculateRequest(request));
        return Ok(response);
    }
}
