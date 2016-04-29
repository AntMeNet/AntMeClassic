using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Online.Client
{
    public interface IAchievementManager
    {
        IEnumerable<Achievement> GetAchievements();
    }
}
