using System.ComponentModel.DataAnnotations;
using RecipesPWA.Common.Validators;

namespace RecipesPWA.Common.Recipes
{
    /// <summary>
    /// Record to store information of a recipe.
    /// </summary>
    public record Recipe : IEntity
    {
        /// <summary>
        /// Gets or sets the id of the recipe.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the recipe.
        /// </summary>
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a collection of instructions for the recipe.
        /// </summary>
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Enumerable<string>]
        public IEnumerable<string> Instructions { get; set; }

        /// <summary>
        /// Gets or sets a collection of <see cref="SubRecipe"/> for the recipe.
        /// </summary>
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        [Enumerable<SubRecipe>]
        public IEnumerable<SubRecipe> SubRecipes { get; set; }
    }
}
