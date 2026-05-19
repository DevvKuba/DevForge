using API.DTO_s;

namespace API.Interfaces
{
    public interface IXpService
    {
        int GetXpThresholdForLevel(int level); // IMP total exp for that level

        int GetRemainingXpForNextLevel(int totalXp, int level); // until next level

        XpAwardDto AwardXp(int awardXp, int totalXp, int level);

        bool HasLevelUpOccured(int currentXp, int level);

    }
}
