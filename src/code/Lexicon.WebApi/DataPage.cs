namespace Lexicon.WebApi
{
    using System.Collections.Generic;

    /// <summary>
    /// Represent segment of data.
    /// </summary>
    /// <typeparam name="T"> data type </typeparam>
    public record DataPage<T>
    {
        /// <summary>
        /// Order number of a page.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Count of data items per one page.
        /// </summary>
        public int ItemsPerPage { get; set; }

        /// <summary>
        /// Data items.
        /// </summary>
        public IList<T>? Items { get; set; }

        /// <summary> Default setup </summary>
        //public static DataPage<T> Default => new DataPage<T>()
        //{
        //};
    }
}
