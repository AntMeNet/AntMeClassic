using System;

namespace AntMe.Online.Client
{
    internal sealed class UpdateRequestModel
    {
        public Guid ClientId { get; set; }

        public Guid UserId { get; set; }

        public string AntMeVersion { get; set; }

        public string OsVersion { get; set; }
    }
}
