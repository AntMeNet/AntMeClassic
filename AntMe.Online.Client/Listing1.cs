using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Online.Client
{
    public sealed class Listing1
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool StaticColonies { get; set; }

        public Guid SettingsId { get; set; }
    }
}
