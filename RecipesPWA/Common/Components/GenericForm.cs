using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using RecipesPWA.Common.Components;

namespace RecipesPWA.Common.Recipes;

/// <summary>
/// Abstract class to use as a base for forms that handle <typeparam name="T"></typeparam> as binded value.
/// </summary>
/// <typeparam name="T">The type of the binded value.</typeparam>
public abstract class GenericForm<T> : InputBase<OrderedGeneric<T>>
where T : class
{
    /// <summary>
    /// The <see cref="EditContext"/> to use for the form.
    /// </summary>
    public EditContext _editContext;

    /// <summary>
    /// Gets the value to display on the submit button.
    /// </summary>
    public string ButtonText => SubmitButtonText(isNew: IsNew, mustBeRemoved: MustBeRemoved());

    /// <summary>
    /// Holds a value to reset the form when submitted invalid value.
    /// </summary>
    public T? BackupValue { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the form contains a new value.
    /// </summary>
    [Parameter]
    public bool IsNew { get; set; }

    /// <summary>
    /// Gets or sets a value to display on the form.
    /// </summary>
    [Parameter]
    public T? TValue { get; set; }

    /// <summary>
    /// Gets or sets a value indicating the order of the form value in the collection.
    /// </summary>
    [Parameter]
    public int Order { get; set; }

    /// <summary>
    /// Abstract method to decide if the given <param name="value"> must be removed or not.</param>
    /// </summary>
    /// <param name="value">The value to check./></param>
    /// <returns>Returns true if the object need to be removed, otherwise false.</returns>
    public abstract bool MustBeRemoved();

    /// <summary>
    /// Abstract method that retrieves a new object of type <typeparamref name="T"/>
    /// </summary>
    /// <returns></returns>
    public abstract T ResetValue();

    /// <summary>
    /// Event that fired when any field of <see cref="EditContext"/> of the form changed.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">The event arguments.</param>
    protected virtual void OnFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        _editContext.Validate();

        if (_editContext.GetValidationMessages(e.FieldIdentifier).Any())
        {
            InvokeValueChangedCallback(BackupValue);
        }
    }

    /// <summary>
    /// Callback function to notify the parent form of the change of the current value.
    /// </summary>
    /// <param name="value">The changed value of type <typeparamref name="T"/>.</param>
    protected virtual void InvokeValueChangedCallback(T? value)
    {
        ValueChanged.InvokeAsync(new(value: value, order: Order));
    }

    /// <summary>
    /// Method to alter the displayed text on the submit button.
    /// </summary>
    /// <param name="isNew"><inheritdoc cref="IsNew"/></param>
    /// <param name="mustBeRemoved"><inheritdoc cref="MustBeRemoved"/></param>
    /// <returns>Returns a string that will be displayed on the submit button on this form.</returns>
    protected virtual string SubmitButtonText(bool isNew, bool mustBeRemoved)
    {
        return isNew
            ? "Hozzáad"
            : mustBeRemoved
                ? "Töröl"
                : "Módosít";
    }

    /// <inheritdoc />
    protected override bool TryParseValueFromString(string? value, out OrderedGeneric<T> result, out string validationErrorMessage)
    {
        var validationResult = _editContext.Validate();
        result = validationResult
            ? new OrderedGeneric<T>(value: TValue!, order: Order)
            : new OrderedGeneric<T>(value: BackupValue!, order: Order);
        validationErrorMessage = string.Join(Environment.NewLine, _editContext.GetValidationMessages());
        return validationResult;
    }

    /// <inheritdoc />
    protected override Task OnParametersSetAsync()
    {
        BackupValue = ResetValue();
        return base.OnParametersSetAsync();
    }

    /// <inheritdoc />
    protected override Task OnInitializedAsync()
    {
        _editContext = new(TValue);
        if (!IsNew)
        {
            _editContext.OnFieldChanged += OnFieldChanged;
        }
        return base.OnInitializedAsync();
    }
}
