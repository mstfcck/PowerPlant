using PowerPlant.Api.Service.ProductionPlanCalculate.Models;
using PowerPlant.Api.Utilities;

namespace PowerPlant.Api.Tests;

[TestFixture]
public class ProductionPlanCalculatorTests
{
    [Test]
    public void CalculateProductionPlan_ZeroLoad_ShouldReturnEmptyList()
    {
        // Arrange
        var calculator = new ProductionPlanCalculator();
        var request = new ProductionPlanRequestModel
        {
            Load = 0,
            PowerPlants = new List<PowerPlantModel>()
        };

        // Act
        var response = calculator.CalculateProductionPlan(request);

        // Assert
        Assert.IsEmpty(response);
    }

    [Test]
    public void CalculateProductionPlan_SingleWindTurbine_ShouldGenerateWindPower()
    {
        // Arrange
        var calculator = new ProductionPlanCalculator();
        var request = new ProductionPlanRequestModel
        {
            Load = 100,
            Fuels = new FuelModel
            {
                Wind = 100, // 100% wind
            },
            PowerPlants = new List<PowerPlantModel>
            {
                new PowerPlantModel
                {
                    Name = "WindTurbine1",
                    Type = "windturbine",
                    Pmax = 150,
                }
            }
        };

        // Act
        var response = calculator.CalculateProductionPlan(request);

        // Assert
        Assert.That(response.Count, Is.EqualTo(1));
        Assert.That(response[0].Name, Is.EqualTo("WindTurbine1"));
        Assert.That(response[0].P, Is.EqualTo(100));
    }

    [Test]
    public void CalculateProductionPlan_NoPowerPlants_ShouldReturnEmptyList()
    {
        // Arrange
        var calculator = new ProductionPlanCalculator();
        var request = new ProductionPlanRequestModel
        {
            Load = 100,
            PowerPlants = new List<PowerPlantModel>()
        };

        // Act
        var response = calculator.CalculateProductionPlan(request);

        // Assert
        Assert.IsEmpty(response);
    }
}