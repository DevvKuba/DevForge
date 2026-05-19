namespace API.Interfaces
{
    public interface IXpService
    {
        int GetXpThresholdForLevel(int level); // IMP total exp for that level

        int GetRemainingXpForNextLevel(int totalXp, int level); // until next level

        int GetOverflowXpAfterLevelUp(int totalXp, int pastLevel); 

        int AwardXp(int totalXp, int level);

        bool hasLevelUpOccured(int totalXp, int level);

    }
}
