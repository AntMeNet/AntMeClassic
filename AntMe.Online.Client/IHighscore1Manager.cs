using System;
using System.Collections.Generic;

namespace AntMe.Online.Client
{
    /// <summary>
    /// Zugriffsmanager für alles rund um die Highscores.
    /// </summary>
    public interface IHighscore1Manager
    {
        Guid DefaultStaticListing { get; }

        Guid DefaultNonstaticListing { get; }

        IEnumerable<Listing1> GetListings();

        Listing1 GetListing(Guid id);

        Row1Values GetRowValues(Guid id);

        void SubmitRow(Guid listingId, byte[] file, string className, int seed);

        Pagination<Row1> GetRows(Guid listingId, ListingOrder order, int page, int pageSize);
    }
}
