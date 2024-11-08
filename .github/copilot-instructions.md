# GitHub Copilot Custom Instructions

## What would you like Copilot to know about you to provide better suggestions?
- I am a C# developer working primarily on .NET backend applications and web APIs.
- My projects often involve ASP.NET Core, Entity Framework Core, and Postgres AWS RDS.
- I prioritize clean, readable, and well-documented code and follow the SOLID principles in my designs.
- I prefer async/await patterns in asynchronous operations.
- I focus on security and performance, especially for database interactions.

## How would you like Copilot to respond?
- Provide concise, readable code suggestions with meaningful variable names.
- Follow C# naming conventions (e.g., PascalCase for methods and properties, camelCase for local variables).
- Use async/await syntax for asynchronous methods whenever possible.
- Add comments in code suggestions explaining complex logic.
- Suggest testable code patterns and indicate if unit tests are recommended.
- Avoid deprecated methods or APIs and prefer best practices in .NET development.

## Additional Notes
- Avoid suggestions related to frontend frameworks like React or Angular, as they’re outside the scope of my work.
- If using libraries or frameworks, prioritize those commonly used in the .NET ecosystem.
- Use `ILogger` for logging in ASP.NET Core applications instead of `Console.WriteLine`.
