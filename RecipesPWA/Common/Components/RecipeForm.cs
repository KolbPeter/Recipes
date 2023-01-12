using RecipesPWA.Common.Recipes;

namespace RecipesPWA.Common.Components;

/// <summary>
/// Abstract class to use as a base for forms that handle a <see cref="Recipe"/> as binded value.
/// </summary>
public abstract class RecipeForm : GenericForm<Recipe>
{
    /// <summary>
    /// Parameter to convert and store text area value to a string collection.
    /// </summary>
    public string Instruction
    {
        get => TValue is null
            ? string.Empty
            : string.Join(Environment.NewLine, TValue.Instructions);
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                TValue.Instructions = value
                    .Split(Environment.NewLine)
                    .Where(x => !string.IsNullOrEmpty(x));
            }
        }
    }

    ///<inheritdoc />
    public override bool MustBeRemoved()
    {
        return string.IsNullOrEmpty(TValue?.Name ?? null);
    }

    ///<inheritdoc />
    public override Recipe ResetValue()
    {
        return new()
        {
            Name = TValue?.Name ?? string.Empty,
            Instructions = TValue?.Instructions ?? Array.Empty<string>(),
            SubRecipes = TValue?.SubRecipes ?? Array.Empty<SubRecipe>()
        };
    }

    /// <summary>
    /// Method that will execute when submit button is clicked.
    /// </summary>
    protected abstract void OnMouseClick();

    ///<inheritdoc />
    protected override void OnParametersSet()
    {
        TValue ??= new Recipe
        {
            Name = TValue?.Name ?? string.Empty,
            Instructions = TValue?.Instructions ?? Array.Empty<string>(),
            SubRecipes = TValue?.SubRecipes ?? Array.Empty<SubRecipe>(),
        };
        base.OnParametersSet();
    }
}