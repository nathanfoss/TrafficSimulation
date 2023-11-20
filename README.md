# TrafficSimulation
This console application attempts to simulate basic highway traffic. Upon startup, the end user inputs road conditions (number of lanes and speed limit) and the application dynamically generates traffic patterns. Cars begin interacting with each other by attempting to move forward at their current speed, pass, and adjust speed if necessary.

## Business Requirements
Generate a traffic pattern based on vehicle types, driver personalities, and available road conditions. Cars should never crash and should attempt to pass or adjust speed to avoid collisions.

## Architecture
- Console Application leveraging the Domain-Driven Design (DDD) Pattern.
- CQRS Powered by MediatR
- Automated testing provided by XUnit
- Quality gateways provided by husky commit hooks

## Roadmap (pun intended)
- Better error checking for user input
- Dynamic road conditions (add/remove lanes, construction, traffic signals)
- Dynamic car conditions (enter/exit configuration, attitude adjustments)
