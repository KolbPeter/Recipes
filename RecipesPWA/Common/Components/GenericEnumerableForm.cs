using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace RecipesPWA.Common.Components;

/// <summary>
/// Abstract class to use as a base for forms that handle collections as binded value.
/// </summary>
/// <typeparam name="T">The type of the binded values.</typeparam>
public abstract class GenericEnumerableForm<T> : InputBase<IEnumerable<T>>
    where T : class
{
    /// <summary>
    /// A <see cref="OrderedGeneric{T}"/> to store the edited value.
    /// </summary>
    public OrderedGeneric<T> NewTValue
    {
        get => _newTValue;
        set
        {
            _newTValue = value;
            NewTValueChanged(value.Value, value.Order);
        }
    }

    private OrderedGeneric<T> _newTValue;

    /// <summary>
    /// Collection of <typeparam name="T"> to display on the form.</typeparam>
    /// </summary>
    [Parameter]
    public IEnumerable<T> Values { get; set; } = Enumerable.Empty<T>();

    /// <summary>
    /// Abstract method to provide a new <see cref="OrderedGeneric{T}"/> object.
    /// </summary>
    /// <returns>Returns a new <see cref="OrderedGeneric{T}"/> object.</returns>
    public abstract OrderedGeneric<T> ResetNewTValue();

    /// <summary>
    /// Abstract method to decide if the given <param name="value"> must be removed or not.</param>
    /// </summary>
    /// <param name="value">The value to check./></param>
    /// <returns>Returns true if the object need to be removed, otherwise false.</returns>
    public abstract bool MustBeRemoved(T value);

    private void NewTValueChanged(T value, int order)
    {
        Values = MustBeRemoved(value)
            ? Values.Where((x, i) => i != order)
            : order < Values.Count()
                ? Values.Select((x, i) => order != i ? x : value)
                : Values.Append(value);

        _newTValue = ResetNewTValue();

        ValueChanged.InvokeAsync(Values);
    }

    protected override Task OnInitializedAsync()
    {
        _newTValue = ResetNewTValue();
        return base.OnInitializedAsync();
    }

    protected override bool TryParseValueFromString(string? value, out IEnumerable<T> result,
        out string? validationErrorMessage)
    {
        result = Values;
        validationErrorMessage = null;
        return true;
    }
}