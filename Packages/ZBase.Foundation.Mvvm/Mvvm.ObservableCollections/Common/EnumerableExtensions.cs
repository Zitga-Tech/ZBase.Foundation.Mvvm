using System.Collections;
using System.Collections.Generic;

namespace ZBase.Foundation.Mvvm.ObservableCollections
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Attempts to determine the number of elements in a sequence without forcing an enumeration.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence that contains elements to be counted.</param>
        /// <param name="count">
        /// When this method returns, contains the number of elements in <paramref name="source"/>,
        /// or 0 if the count couldn't be determined without enumeration.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the count of source can be determined without enumeration; otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// The method performs a series of type tests, identifying common subtypes whose count can be determined without enumerating.
        /// This includes <see cref="ICollection{T}"/>, <see cref="IReadOnlyCollection{T}"/>, <see cref="ICollection"/>.
        /// <br/>
        /// The method is typically a constant-time operation,
        /// but ultimately this depends on the complexity characteristics of the underlying collection's implementation.
        /// </remarks>
        public static bool TryGetCountFast<T>(this IEnumerable<T> source, out int count)
        {
            if (source is ICollection<T> collectionT)
            {
                count = collectionT.Count;
                return true;
            }

            if (source is IReadOnlyCollection<T> readonlyCollectionT)
            {
                count = readonlyCollectionT.Count;
                return true;
            }

            if (source is ICollection collection)
            {
                count = collection.Count;
                return true;
            }

            count = 0;
            return false;
        }
    }
}