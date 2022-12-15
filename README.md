# Recipes

## Am of teh project:
To create a Blazor progressive web assembly to manipulate recipes, stored in an Azure Mongo database. The recipes can be created, retrieved, updated and deleted through an Azure Function App, using HTTP triggers. Every part of the solution is stored and can be reached on Azure. Uses free solutions where it was possibly, current setup can work for some euro cents per month.

## Curent state:
When this readme is created the web app have a full functionality, all CRUD operations work perfectly.  
The function app is working flawlessly too.  
Already added Application Insights for both apps for monitoring and logging purposes.
The database is holding 2 recipes.

## Future plans:
- Add logging capabilities, especially for the web app(RecipesPWA).
- Add authentication and authorization for RecipesPWA, at least for modify and delete recipes.
- Add logging when modify or delete a recipe.
    - Maybe an E-mail notification will be enough, with the unmodified values as json string.  
    - Or stor the unmodified values in the cloud.