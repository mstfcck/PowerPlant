using System.Text.Json.Serialization;

namespace PowerPlant.Api.Service.ProductionPlanCalculate.Models;

public class ProductionPlanResponseModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("p")]
    public double P { get; set; }
}