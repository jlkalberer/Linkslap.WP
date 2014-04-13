namespace Linkslap.WP.Communication.Util
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// The collection extensions.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <param name="condition">
        /// The condition.
        /// </param>
        /// <typeparam name="T">
        /// The model.
        /// </typeparam>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int Remove<T>(this ObservableCollection<T> collection, Func<T, bool> condition)
        {
            var itemsToRemove = collection.Where(condition).ToList();

            foreach (var itemToRemove in itemsToRemove)
            {
                collection.Remove(itemToRemove);
            }

            return itemsToRemove.Count;
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the ObservableCollection(Of T). 
        /// </summary>
        /// <param name="observable">
        /// The observable.
        /// </param>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <typeparam name="T">
        /// The collection model.
        /// </typeparam>
        public static void AddRange<T>(this ObservableCollection<T> observable, IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            foreach (var i in collection)
            {
                observable.Add(i);
            }
        }

        /// <summary>
        /// Removes the first occurence of each item in the specified collection from ObservableCollection(Of T). 
        /// </summary>
        /// <typeparam name="T">
        /// The collection model.
        /// </typeparam>
        /// <param name="observable">
        /// The observable.
        /// </param>
        /// <param name="collection">
        /// The collection.
        /// </param>
        public static void RemoveRange<T>(this ObservableCollection<T> observable, IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            foreach (var i in collection)
            {
                observable.Remove(i);
            }
        }
    }
}
