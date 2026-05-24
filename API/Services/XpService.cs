using API.DTO_s;
using API.Entities;
using API.Interfaces;

namespace API.Services
{
    public class XpService(IUnitOfWork unitOfWork) : IXpService
    {
        public int BaseValue { get; set; } = 100;

        public double Exponent { get; set; } = 1.5;

        public void AwardXp(AppUser user, int awardXp, int currentXp, int level)
        {
            var expThreshold = GetXpThresholdForLevel(level);

            var updatedTotalXp = currentXp + awardXp;

           // assuming only one level can be updated via the awardXp
            if(updatedTotalXp > expThreshold)
            {
                level++;

                var leftOverExp = updatedTotalXp- expThreshold;

                unitOfWork.UserRepository.UpdateAppExperienceAndLevel(user, leftOverExp, level);

            }
            else
            {
                unitOfWork.UserRepository.UpdateAppExperienceAndLevel(user, updatedTotalXp, level);
            }

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
