using API.DTO_s;
using API.Entities;

namespace API.Interfaces
{
    public interface IXpService
    {
        int GetXpThresholdForLevel(int level); // IMP total exp for that level

        int GetRemainingOrLeftoverXpForNextLevel(int totalXp, int level); // until next level

        void AwardXp(AppUser user, int awardXp, int currentXp, int level);

        bool HasLevelUpOccured(int currentXp, int level);

    }
}
