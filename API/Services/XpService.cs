using API.Interfaces;

namespace API.Services
{
    public class XpService : IXpService
    {
        public int BaseValue { get; set; } = 100;

        public double Exponent { get; set; } = 1.5;

        public int GetOverflowXpAfterLevelUp(int totalXp, int pastLevel)
        {
            throw new NotImplementedException();
        }

        public int GetRemainingXpForNextLevel(int totalXp, int level)
        {
            throw new NotImplementedException();
        }

        public int AwardXp(int totalXp, int level)
        {
            throw new NotImplementedException();
        }

        public bool hasLevelUpOccured(int totalXp, int level)
        {
            throw new NotImplementedException();
        }

        public int GetXpThresholdForLevel(int level)
        {
            return (int)Math.Round(BaseValue * Math.Pow(level, Exponent), 0);
        }
    }
}
