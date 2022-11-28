using System.ComponentModel.DataAnnotations;

namespace RecipesPWA.Common.Recipes
{
    /// <summary>
    /// Record to store a recipe ingredient.
    /// </summary>
    public record Ingredient
    {
        /// <summary>
        /// Gets or sets the name of the ingredient.
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string Name { get; set; }


        /// <summary>
        /// Gets or sets the unit of measurement of the ingredient.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the ingredient.
        /// </summary>
        [Range(0.01, 100000, ErrorMessage = "A mennyiségnek 0.01 és 100000 között kell lennie.")]
        public decimal Quantity { get; set; }
    }
}
