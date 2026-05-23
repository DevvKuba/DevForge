using API.DTO_s;

namespace API.Interfaces
{
    public interface IXpService
    {
        int GetXpThresholdForLevel(int level); // IMP total exp for that level

        int GetRemainingOrLeftoverXpForNextLevel(int totalXp, int level); // until next level

        UserDto AwardXp(UserDto user, int awardXp, int totalXp, int level);

        bool HasLevelUpOccured(int currentXp, int level);

        void UpdateUserXpDetails(UserDto user, int level, int experiencePoints, int levelThreshold);

    }
}
