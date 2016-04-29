using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Online.Client
{
    internal sealed class Highscore1Manager : IHighscore1Manager
    {
        private Connection connection;

        public Guid DefaultStaticListing { get { return Guid.Parse("B944206E-06EE-457F-B730-8BFA4621FA6F"); } }

        public Guid DefaultNonstaticListing { get { return Guid.Parse("FFD2F0AA-B629-48EE-8B60-92EF2331A191"); } }

        internal Highscore1Manager(Connection connection)
        {
            this.connection = connection;
        }

        public IEnumerable<Listing1> GetListings()
        {
            return connection.Get<IEnumerable<Listing1>>("Highscore1");
        }

        public Listing1 GetListing(Guid id)
        {
            return connection.Get<Listing1>(string.Format("Highscore1/{0}", id));
        }

        public Row1Values GetRowValues(Guid id)
        {
            return connection.Get<Row1Values>(string.Format("Highscore1/My/{0}", id));
        }

        public void SubmitRow(Guid listingId, byte[] file, string className, int seed)
        {
            Highscore1SubmitModel model = new Highscore1SubmitModel()
            {
                File = file,
                ClassName = className,
                Seed = seed
            };

            connection.Post<Highscore1SubmitModel>(string.Format("Highscore1/My/{0}/Submit", listingId), model);
        }


        public Pagination<Row1> GetRows(Guid listingId, ListingOrder order, int page, int pageSize)
        {
            return connection.Get<Pagination<Row1>>(string.Format("Highscore1/{0}/{1}?page={2}&pageSize={3}", listingId, order, page, pageSize));
        }
    }
}
