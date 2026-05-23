using API.DTO_s;

namespace API.Interfaces
{
    public interface IXpService
    {
        int GetXpThresholdForLevel(int level); // IMP total exp for that level

        int GetRemainingOrLeftoverXpForNextLevel(int totalXp, int level); // until next level

        UserXpDetailDto AwardXp(int awardXp, int totalXp, int level);

        bool HasLevelUpOccured(int currentXp, int level);

    }
}
