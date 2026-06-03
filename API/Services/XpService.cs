using API.DTO_s;
using API.Entities;
using API.Interfaces;

namespace API.Services
{
    public class XpService(IUnitOfWork unitOfWork) : IXpService
    {
        public int BaseValue { get; set; } = 100;

        public double Exponent { get; set; } = 1.5;

        public void AwardXp(AppUser user, int awardXp)
        {
            var level = user.Level;
            var currentXp = user.AppExperiencePoints;

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

        public void LoseXp(AppUser user, int loseXp)
        {
            var level = user.Level;
            var currentXp = user.AppExperiencePoints;

            var updatedTotalXp = currentXp - loseXp;

            var pastLevelThreshold = GetXpThresholdForLevel(level - 1);

            // if the updated xp is less than the past level threshold
            if (updatedTotalXp < pastLevelThreshold)
            {
                level--;

                unitOfWork.UserRepository.UpdateAppExperienceAndLevel(user, updatedTotalXp, level);
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

        public int CalculateXpGainsForQuizCompletion(string difficulty, int numberOfQuestions, double percentageScore)
        {
            var difficultyMultiplier = difficulty switch
            {
                "easy" => 1.0,
                "medium" => 1.4,
                "hard" => 1.8,
                _ => 1.0
            };

            var baseXp = difficultyMultiplier * numberOfQuestions;

            var accuracyBonus = (percentageScore / 100) * numberOfQuestions + 1;

            var totalXp = baseXp * accuracyBonus;

            return (int)Math.Round(totalXp);
        }

        public int GetXpThresholdForLevel(int level)
        {
            return (int)Math.Round(BaseValue * Math.Pow(level, Exponent), 0);
        }

    }
}
