namespace RecipesPWA.Common;

/// <summary>
/// Interface for sending requests.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IRequestService<T> where T : class
{
    /// <summary>
    /// Send a request with the given <paramref name="headers"/>.
    /// </summary>
    /// <param name="headers">The headers for the request.</param>
    /// <returns>Returns a task that evaluates to a collection of <typeparam name="T">.</typeparam></returns>
    Task<IEnumerable<T>> GetAll(IEnumerable<RequestHeader>? headers = null);

    /// <summary>
    /// Send a request with the given <paramref name="headers"/>.
    /// </summary>
    /// <param name="id">The id of the object to retrieve.</param>
    /// <param name="headers">The headers for the request.</param>
    /// <returns>Returns a task that evaluates to an object of <typeparam name="T">.</typeparam></returns>
    Task<T?> GetById(
        string id,
        IEnumerable<RequestHeader>? headers = null);

    /// <summary>
    /// Send a request with the given <paramref name="headers"/>.
    /// </summary>
    /// <param name="name">The name of the object to retrieve.</param>
    /// <param name="headers">The headers for the request.</param>
    /// <returns>Returns a task that evaluates to a collection of <typeparam name="T">.</typeparam></returns>
    Task<IEnumerable<T>> GetByName(
        string name,
        IEnumerable<RequestHeader>? headers = null);

    /// <summary>
    /// Send a request with the given <paramref name="headers"/>.
    /// </summary>
    /// <param name="newObject">The object of type <typeparamref name="T"/> to add.</param>
    /// <param name="headers">The headers for the request.</param>
    /// <returns>Returns a task that evaluates to an object of <typeparam name="T">.</typeparam></returns>
    Task<T> Add(
        T newObject,
        IEnumerable<RequestHeader>? headers = null);

    /// <summary>
    /// Send a request with the given <paramref name="headers"/>.
    /// </summary>
    /// <param name="objectToUpdate">The object of type <typeparamref name="T"/> to update.</param>
    /// <param name="headers">The headers for the request.</param>
    /// <returns>Returns a task that evaluates to an object of <typeparam name="T">.</typeparam></returns>
    Task<T> Update(
        T objectToUpdate,
        IEnumerable<RequestHeader>? headers = null);
}