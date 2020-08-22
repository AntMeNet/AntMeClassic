using System.Collections.Generic;

namespace AntMe.Online.Client
{
    internal class AchievementManager : IAchievementManager
    {
        private Connection connection;

        internal AchievementManager(Connection connection)
        {
            this.connection = connection;
        }

        public IEnumerable<Achievement> GetAchievements()
        {
            return connection.Get<IEnumerable<Achievement>>("Achievements");
        }
    }
}
