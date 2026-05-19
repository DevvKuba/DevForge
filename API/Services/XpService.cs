using API.DTO_s;
using API.Interfaces;

namespace API.Services
{
    public class XpService : IXpService
    {
        public int BaseValue { get; set; } = 100;

        public double Exponent { get; set; } = 1.5;

        public XpAwardDto AwardXp(int awardXp, int totalXp, int level)
        {

        }

        public int GetRemainingXpForNextLevel(int totalXp, int level)
        {
            var expThreshold = GetXpThresholdForLevel(level);

            return expThreshold - totalXp;
        }

        public bool HasLevelUpOccured(int currentXp, int level)
        {
            var expThreshold = GetXpThresholdForLevel(level);

            if (currentXp > expThreshold) return true;

            return false;
        }


        public int GetXpThresholdForLevel(int level)
        {
            return (int)Math.Round(BaseValue * Math.Pow(level, Exponent), 0);
        }
    }
}
