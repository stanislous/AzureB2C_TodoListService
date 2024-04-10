### TodoListService

TodoListService is a .NET 8 Web API Application that utilizes MSSQL Server as its underlying database.

#### Setup Instructions
To configure TodoListService, follow these steps:

Install the required NuGet packages:

- Microsoft.Identity.Web;
- System.IdentityModel.Tokens.Jwt;
- Microsoft.AspNetCore.Authentication.JwtBearer;

#### Startup.cs

```
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(options =>
    {
        Configuration.Bind("AzureAdB2C", options);
    },
        options => { Configuration.Bind("AzureAdB2C", options); });
```

The code establishes token-based authentication using Azure AD B2C for a Web API.
It disables automatic claim mapping to give the developer more control over claim handling.
It configures authentication middleware and binds necessary Azure AD B2C settings from configuration files.

#### TodoListController.cs

This code defines a Web API controller in ASP.NET Core for managing a simple to-do list. 
It uses an in-memory dictionary for storage and requires authentication with a specific scope for access.
You can replace it with your database.

- Authentication:
   Requires authenticated users with the "tasks.read" scope for access.

- Authorization:
   Only shows to-do items owned by the authenticated user.

- Data Storage:
   Uses an in-memory dictionary TodoStore to store to-do items.

- Endpoints:
   - GET api/TodoList: Retrieves all to-do items owned by the current user.
   - GET api/TodoList/{id}: Retrieves a specific to-do item by its ID.
   - POST api/TodoList: Creates a new to-do item.
   - DELETE api/TodoList/{id}: Deletes a to-do item by its ID.
   - PATCH api/TodoList/{id}: Updates an existing to-do item.

- Additional Notes:
   - Pre-populates the dictionary with sample data if it's empty.
   - Uses IHttpContextAccessor to access the current user's claims for authorization.
