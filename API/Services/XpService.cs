using API.Interfaces;

namespace API.Services
{
    public class XpService : IXpService
    {
        public int AwardXp(int totalXp, int level)
        {
            throw new NotImplementedException();
        }

        public int GetOverflowXpAfterLevelUp(int totalXp, int pastLevel)
        {
            throw new NotImplementedException();
        }

        public int GetRemainingXpForNextLevel(int totalXp, int level)
        {
            throw new NotImplementedException();
        }

        public int GetXpThresholdForLevel(int level)
        {
            throw new NotImplementedException();
        }

        public bool hasLevelUpOccured(int totalXp, int level)
        {
            throw new NotImplementedException();
        }
    }
}
