using System;

namespace AntMe.Online.Client
{
    public class Achievement
    {
        public Guid Id { get; set; }

        public bool Hidden { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Value { get; set; }

        public Guid PictureId { get; set; }

        public DateTime? Achieved { get; set; }
    }
}
