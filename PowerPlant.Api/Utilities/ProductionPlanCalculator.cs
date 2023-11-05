using PowerPlant.Api.Service.ProductionPlanCalculate.Models;

namespace PowerPlant.Api.Utilities;

/// <summary>
/// Production Plan Calculator.
/// </summary>
public class ProductionPlanCalculator : IProductionPlanCalculator
{
    /// <summary>
    /// Calculate the production plan based on the given request.
    /// </summary>
    /// <returns>The calculated production plan as a list of response models.</returns>
    public List<ProductionPlanResponseModel> CalculateProductionPlan(ProductionPlanRequestModel productionPlanRequest)
    {
        var response = new List<ProductionPlanResponseModel>();

        // The load represents the amount of energy that needs to be generated during one hour, measured in MWh (MegaWatt-hours).
        double totalLoadToMeet = productionPlanRequest.Load;

        // Sort powerPlants by cost-effectiveness (ascending order of cost per MWh).
        var powerPlants =
            SortPowerPlantsByCostEffectiveness(productionPlanRequest.PowerPlants, productionPlanRequest.Fuels);

        /*
         * The powerPlants array describes the power plants at disposal and their characteristics:
         * - name: The name of the power plant.
         * - type: The type of the power plant, which can be "gasfired" "turbojet" or "windturbine".
         * - efficiency: The efficiency of the power plant in converting fuel into electricity.
         * - pmax: The maximum power capacity of the power plant.
         * - pmin: The minimum power capacity the power plant generates when switched on.
         */

        foreach (var powerPlant in powerPlants)
        {
            if (totalLoadToMeet <= 0)
            {
                // No more load to meet, stop producing power.
                break;
            }

            // Calculate the maximum power this powerPlant can produce while considering the remaining load.
            var maxPower = CalculateMaxPowerToProduce(powerPlant, totalLoadToMeet);

            if (maxPower >= powerPlant.Pmin)
            {
                // Calculate the actual power produced by the powerPlant while considering cost.
                var producedPower = CalculateProducedPower(powerPlant, maxPower, productionPlanRequest.Fuels);

                // Add the response for this powerPlant.
                response.Add(new ProductionPlanResponseModel
                {
                    Name = powerPlant.Name,
                    P = producedPower
                });

                // Update the total load to meet.
                totalLoadToMeet -= producedPower;
            }
        }

        if (totalLoadToMeet > 0)
        {
            // Handle any remaining load by distributing it among powerPlants.
            HandleRemainingLoad(response, totalLoadToMeet, productionPlanRequest.Fuels);
        }

        return response;
    }

    /// <summary>
    /// Sort the power plants in ascending order of cost-effectiveness.
    /// This private method sorts the power plants in ascending order of cost-effectiveness.
    /// It calculates the cost per MWh for each power plant based on its Pmax and type and forms the merit order.
    /// </summary>
    private List<PowerPlantModel> SortPowerPlantsByCostEffectiveness(List<PowerPlantModel> powerPlants, FuelModel fuels)
    {
        return powerPlants
            .OrderBy(x => CalculateCost(fuels, x.Pmax, x.Type) / x.Pmax)
            .ToList();
    }

    /// <summary>
    /// This method calculates the maximum power that a power plant can produce while considering the remaining load.
    /// It ensures that the power produced does not exceed the power plant's capacity.
    /// </summary>
    private double CalculateMaxPowerToProduce(PowerPlantModel powerPlant, double remainingLoad)
    {
        return Math.Min(powerPlant.Pmax, remainingLoad);
    }

    /// <summary>
    /// This method calculates the actual power produced by a power plant while considering the cost.
    /// It subtracts the cost from the maximum power.
    /// </summary>
    private double CalculateProducedPower(PowerPlantModel powerPlant, double maxPower, FuelModel fuels)
    {
        var cost = CalculateCost(fuels, maxPower, powerPlant.Type);
        return maxPower - cost;
    }

    /// <summary>
    /// This method calculates the cost for a specific power plant to produce power.
    /// It takes into account the cost of fuels (gas, kerosene) and, in the case of gas-fired power plants, the cost of CO2 emissions allowances.
    /// </summary>
    private double CalculateCost(FuelModel fuels, double producedPower, string type)
    {
        /*
         * The fuels object contains information about the cost of various fuels and the percentage of wind energy:
         * - gas(euro/MWh): The cost of gas per MWh.
         * - kerosine(euro/MWh): The cost of kerosine per MWh.
         * - co2(euro/ton): The cost of CO2 emissions allowances per ton.
         * - wind(%): The percentage of wind energy available, which influences the output of wind turbines.
         */

        double cost = 0;

        switch (type)
        {
            case "gasfired":
                cost = fuels.Gas * producedPower;
                
                // Calculate CO2 emissions cost based on 0.3 tons of CO2 per MWh.
                // Assuming 1 MWh = 1000 kWh
                cost += fuels.Co2 * (producedPower / 1000.0 * 0.3); 
                break;
            case "turbojet":
                cost = fuels.Kerosine * producedPower;
                break;
            case "windturbine":
                cost = 0; // Wind turbines have zero fuel cost.
                break;
        }

        return cost;
    }

    /// <summary>
    /// This method handles any remaining load by distributing it among power plants.
    /// If there are gas-fired power plants, it distributes the load based on their capacity.
    /// If there are no gas-fired power plants, it distributes the load equally among all power plants.
    /// </summary>
    private void HandleRemainingLoad(List<ProductionPlanResponseModel> response, double remainingLoad, FuelModel fuels)
    {
        var gasPowerPlants = response.Where(p => p.Name.StartsWith("gasfired")).ToList();

        if (gasPowerPlants.Any())
        {
            var totalPmax = gasPowerPlants.Sum(x => x.P);
            foreach (var gasPowerPlant in gasPowerPlants)
            {
                var loadShare = (gasPowerPlant.P / totalPmax) * remainingLoad;
                gasPowerPlant.P += loadShare;
            }
        }
        else
        {
            var loadShare = remainingLoad / response.Count;
            foreach (var powerPlant in response)
            {
                var cost = CalculateCost(fuels, loadShare, powerPlant.Name);
                powerPlant.P += loadShare - cost;
            }
        }
    }
}