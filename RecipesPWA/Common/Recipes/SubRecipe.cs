using RecipesPWA.Common.Validators;
using System.ComponentModel.DataAnnotations;

namespace RecipesPWA.Common.Recipes
{
    /// <summary>
    /// Record to store information of a sub recipe of a recipe.
    /// </summary>
    public record SubRecipe
    {
        /// <summary>
        /// Gets or sets the name of the sub recipe.
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets instructions for the sub recipe.
        /// </summary>
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string Preparation { get; set; }

        /// <summary>
        /// Gets or sets a collection of <see cref="Ingredient"/> for the sub recipe.
        /// </summary>
        [Required]
        [Enumerable<Ingredient>]
        public IEnumerable<Ingredient> Ingredients { get; set; }
    }
}