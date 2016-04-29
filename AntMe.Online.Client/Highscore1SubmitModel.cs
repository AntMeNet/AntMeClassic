using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Online.Client
{
    internal sealed class Highscore1SubmitModel
    {
        public byte[] File { get; set; }

        public string ClassName { get; set; }

        public int Seed { get; set; }
    }
}
