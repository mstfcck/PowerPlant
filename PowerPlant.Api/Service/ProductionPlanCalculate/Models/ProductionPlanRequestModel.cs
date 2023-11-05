using System.Text.Json.Serialization;

namespace PowerPlant.Api.Service.ProductionPlanCalculate.Models;

public class ProductionPlanRequestModel
{
    [JsonPropertyName("load")]
    public double Load { get; set; }

    [JsonPropertyName("fuels")]
    public FuelModel Fuels { get; set; }

    [JsonPropertyName("powerplants")]
    public List<PowerPlantModel> PowerPlants { get; set; }
}

public class FuelModel
{
    [JsonPropertyName("gas(euro/MWh)")]
    public double Gas { get; set; }

    [JsonPropertyName("kerosine(euro/MWh)")]
    public double Kerosine { get; set; }

    [JsonPropertyName("co2(euro/ton)")]
    public int Co2 { get; set; }

    [JsonPropertyName("wind(%)")]
    public int Wind { get; set; }
}

public class PowerPlantModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("efficiency")]
    public double Efficiency { get; set; }

    [JsonPropertyName("pmin")]
    public int Pmin { get; set; }

    [JsonPropertyName("pmax")]
    public int Pmax { get; set; }
}