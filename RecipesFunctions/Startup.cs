using Azure.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipesFunctions.Common.MongoDb.Collections;
using RecipesFunctions.Common.MongoDb.Context;
using RecipesFunctions.Common.MongoDb.Repositories;
using System;

[assembly: FunctionsStartup(typeof(RecipesFunctions.Startup))]

namespace RecipesFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IMongoDbContext, MongoDbContext>();
            builder.Services.AddScoped<IMongoDbRepository<Recipe>, RecipeRepository>();
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            builder.ConfigurationBuilder.AddJsonFile("appsettings.json", false, true);

            var builtConfig = builder.ConfigurationBuilder.Build();
            var vaultUri = new Uri(builtConfig["VaultUri"]);
            builder.ConfigurationBuilder
                .AddAzureKeyVault(
                    vaultUri,
                    new DefaultAzureCredential());

            base.ConfigureAppConfiguration(builder);
        }
    }
}