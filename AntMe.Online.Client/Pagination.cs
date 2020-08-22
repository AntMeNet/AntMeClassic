using System;
using System.Collections.Generic;

namespace AntMe.Online.Client
{
    public class Pagination
    {
        /// <summary>
        /// Default Value for the Page Size
        /// </summary>
        public const int DefaultPageSize = 50;

        /// <summary>
        /// The maximum value for  Page Size
        /// </summary>
        public const int MaximumPageSize = 100;

        /// <summary>
        /// Number of total Hits
        /// </summary>
        public int TotalHits { get; set; }

        /// <summary>
        /// Total number of pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Current Page Number.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Hits per Page.
        /// </summary>
        public int PageSize { get; set; }

        public Pagination(int page, int pageSize, int totalHits)
        {
            // Limit PageSize
            PageSize = Math.Max(0, Math.Min(pageSize, MaximumPageSize));

            if (PageSize > 0)
            {
                // Normal Pagination
                TotalHits = totalHits;
                if (TotalHits > 0)
                {
                    TotalPages = (int)Math.Ceiling((decimal)TotalHits / PageSize);
                    CurrentPage = Math.Max(0, Math.Min(TotalPages - 1, page));
                }
                else
                {
                    TotalPages = 0;
                    CurrentPage = 0;
                }
            }
            else
            {
                // Request only Item Count
                TotalHits = totalHits;
                TotalPages = 0;
                CurrentPage = 0;
            }
        }
    }

    public class Pagination<T> : Pagination
    {
        public Pagination(int page, int pageSize, int totalHits)
            : base(page, pageSize, totalHits)
        {
            Result = new List<T>(PageSize);
        }

        /// <summary>
        /// Result Set
        /// </summary>
        public List<T> Result { get; set; }
    }
}
