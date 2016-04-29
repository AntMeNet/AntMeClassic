using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
