using System.Collections.Generic;

namespace AntMe.Online.Client
{
    public interface IAchievementManager
    {
        IEnumerable<Achievement> GetAchievements();
    }
}
