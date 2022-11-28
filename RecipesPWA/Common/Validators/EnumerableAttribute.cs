using System.ComponentModel.DataAnnotations;

namespace RecipesPWA.Common.Validators
{
    /// <summary>
    /// Implementation of <see cref="ValidationAttribute"/> to validate <see cref="IEnumerable{T}"/> attributes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumerableAttribute<T> : ValidationAttribute
        where T : class
    {
        /// <summary>
        /// Get a value indicating that the given object is valid or not.
        /// </summary>
        /// <param name="value">The <see cref="object"/> to validate.</param>
        /// <returns>Returns true if teh given <paramref name="value"/> is an enumerable of <typeparamref name="T"/>,
        /// not null and contains at least 1 element, otherwise false.</returns>
        public override bool IsValid(object value) =>
            value is not null
            &&value is IEnumerable<T>
            && ((IEnumerable<T>)value).Any();
    }
}
