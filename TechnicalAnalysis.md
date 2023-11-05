# PowerPlant

## Technical Analysis

### Example Request:

[This JSON payload](https://github.com/gem-spaas/powerplant-coding-challenge/blob/master/example_payloads/payload2.json) includes information about the load, fuels, and powerplants.

#### Request Detail:

- Load: The load is 480 MWh, which is the amount of energy that needs to be generated during one hour.
- Fuels: This section provides the cost and other data related to fuels and wind energy.
    - Gas: The cost of gas is 13.4 euro per MWh.
    - Kerosine: The cost of kerosine is 50.8 euro per MWh.
    - CO2: The cost of CO2 emission allowances is 20 euro per ton.
    - Wind: The percentage of wind energy is 0%, indicating that there is    no wind energy available.
- PowerPlants: This section describes the available power plants.
    - "gasfiredbig1" and "gasfiredbig2" are gas-fired powerplants with 53% efficiency, a minimum output of 100 MW (Pmin), and a maximum output of 460 MW (Pmax).
    - "gasfiredsomewhatsmaller" is a gas-fired powerplant with 37% efficiency, a minimum output of 40 MW (Pmin), and a maximum output of 210 MW (Pmax).
    - "tj1" is a turbojet powerplant with 30% efficiency, no minimum output (Pmin is 0), and a maximum output of 16 MW (Pmax).
    - "windpark1" and "windpark2" are wind turbines with 100% efficiency, no minimum output (Pmin is 0), and maximum outputs of 150 MW and 36 MW, respectively.

### Example Response:

[This JSON response](https://github.com/gem-spaas/powerplant-coding-challenge/blob/master/example_payloads/response3.json) is an array with each PowerPlant's name and the amount of power (in MW) they should produce.

#### Response Detail:

- "windpark1" should produce 90.0 MW.
- "windpark2" should produce 21.6 MW.
- "gasfiredbig1" should produce 460.0 MW.
- "gasfiredbig2" should produce 338.4 MW.
- "gasfiredsomewhatsmaller" and "tj1" should not produce any power (0.0 MW).

The response ensures that the total power produced by all powerplants matches the specified load of 910 MWh, while considering the cost of fuels and the availability of wind energy.

## Acceptance Criteria

For a submission to be reviewed as part of an application for a position in the team, the project needs to:

- contain a README.md explaining how to build and launch the API
- expose the API on port 8888

