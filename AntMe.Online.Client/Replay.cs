using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Online.Client
{
    public sealed class Replay
    {
        public Guid Id { get; set; }

        public Guid Owner { get; set; }

        public DateTime CreateDate { get; set; }

        public int Size { get; set; }
    }
}
