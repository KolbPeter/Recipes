namespace RecipesPWA.Common.Components
{
    /// <summary>
    /// Class to store values in ordered form.
    /// </summary>
    /// <typeparam name="T">The type of the stored values.</typeparam>
    public class OrderedGeneric<T>
    {
        /// <summary>
        /// Gets or sets the value to store.
        /// </summary>
        public T Value;

        /// <summary>
        /// Gets or sets the position of the value in the collection.
        /// </summary>
        public int Order;

        public OrderedGeneric(T value, int order)
        {
            Value = value;
            Order = order;
        }
    }
}
