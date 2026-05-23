using API.DTO_s;
using API.Interfaces;

namespace API.Services
{
    public class XpService : IXpService
    {
        public int BaseValue { get; set; } = 100;

        public double Exponent { get; set; } = 1.5;

        public UserDto AwardXp(UserDto user, int awardXp, int currentXp, int level)
        {
            var expThreshold = GetXpThresholdForLevel(level);

            var updatedTotalXp = currentXp + awardXp;

           // assuming only one level can be updated via the awardXp
            if(updatedTotalXp > expThreshold)
            {
                level++;

                var leftOverExp = GetRemainingOrLeftoverXpForNextLevel(updatedTotalXp, level);

                UpdateUserXpDetails(user, level, leftOverExp, GetXpThresholdForLevel(level));

            }
            else
            {
                UpdateUserXpDetails(user, level, updatedTotalXp, GetXpThresholdForLevel(level));
            }

            return user;

        }

        public int GetRemainingOrLeftoverXpForNextLevel(int totalXp, int level)
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

        public void UpdateUserXpDetails(UserDto user, int level, int experiencePoints, int levelThreshold)
        {
            user.Level = level;
            user.AppExperiencePoints = experiencePoints;
            user.LevelThreshold = levelThreshold;
        }
    }
}
