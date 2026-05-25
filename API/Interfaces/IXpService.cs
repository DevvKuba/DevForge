using API.DTO_s;
using API.Entities;

namespace API.Interfaces
{
    public interface IXpService
    {
        int GetXpThresholdForLevel(int level); // IMP total exp for that level

        void AwardXp(AppUser user, int awardXp);

        void LoseXp(AppUser user, int loseXp);

        bool HasLevelUpOccured(int currentXp, int level);

    }
}
