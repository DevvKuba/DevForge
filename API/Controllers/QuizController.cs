using API.DTO_s;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Runtime.CompilerServices;

namespace API.Controllers
{
    [Authorize]
    public class QuizController(IUnitOfWork unitOfWork, IMapper mapper, IQuizService quizService, IXpService xpService) : BaseApiController
    {
        // Get Call To get a history of most recent quizzes, returns with it's quiz Questions 
        // allows for checking of how many xp points were gained, completed questions etc.

        [HttpGet("GetAllCompletedQuizzes")]
        public async Task<ActionResult<List<QuizDto>>> GetAllUserCompletedQuizzesAsync([FromQuery] QuizParams quizParams)
        {
            if (quizParams.UserId == null) return NotFound(new List<Quiz> { });

            var user = await unitOfWork.UserRepository.GetUserByIdAsync((int)quizParams.UserId);

            if (user == null) return NotFound(new List<Quiz> { });

            var quizzes = await unitOfWork.QuizRepository.GetUserQuizzesAsync(quizParams);

            if (quizzes.Count == 0 || quizzes == null) return Ok(new List<Quiz> { });

            return Ok(quizzes);
        }

        [HttpGet("GetComputerScienceQuestions")] // historically all questions answered
        public async Task<ActionResult<List<QuizQuestionDto>>> GetComputerScienceQuestionsAsync([FromQuery] QuizInfoDto quizInfo)
        {
            var quizQuestions = await quizService.RetrieveQuestionsAsync(quizInfo.NumberOfQuestions, quizInfo.Difficulty, quizInfo.QuestionType);

            if (quizQuestions == null || quizQuestions.Count == 0) return NotFound(false);

            return Ok(quizQuestions);
        }

        [HttpPost("SaveStartedQuiz")]
        public async Task<ActionResult<ApiResponse<QuizDto>>> SaveStartedQuizAsync(QuizDto startedQuiz)
        {
            if (startedQuiz == null) return NotFound(new ApiResponse<string> { Success = false, Message = "Quiz info not found" });

            var associatedUser = await unitOfWork.UserRepository.GetUserByIdAsync(startedQuiz.UserId);

            if (associatedUser == null) return NotFound(new ApiResponse<string> { Success = false, Message = "Associated user not found" });

            var newQuiz = mapper.Map<Quiz>(startedQuiz);

            var areOtherQuizzesActive = await unitOfWork.QuizRepository.DoesUserHaveAnUnfinishedQuizAsync(associatedUser);

            if (areOtherQuizzesActive) return BadRequest(new ApiResponse<string> { Success = false, Message = "Ongoing quiz active, either complete this quiz or delete it to start a new one" });

            await unitOfWork.QuizRepository.SaveQuizAsync(newQuiz);

            if (!await unitOfWork.Complete()) return BadRequest(new ApiResponse<string> { Success = false, Message = "Quiz not saved" });

            return Ok(new ApiResponse<QuizDto> { Data = startedQuiz, Success = true, Message = "Started quiz has been saved" });

        }

        [HttpPut("SaveCompletedQuiz")]
        public async Task<ActionResult<ApiResponse<string>>> SaveCompletedQuizAsync(QuizDto quizDto)
        {
            if (quizDto == null || quizDto.Id == null) return NotFound(new ApiResponse<string> { Success = false, Message = "Quiz info not found" });

            var associatedUser = await unitOfWork.UserRepository.GetUserByIdAsync(quizDto.UserId);

            if (associatedUser == null) return NotFound(new ApiResponse<string> { Success = false, Message = "Associated user not found" });

            var pendingQuiz = await unitOfWork.QuizRepository.GetQuizByIdAsync((int)quizDto.Id);

            if (pendingQuiz == null) return NotFound(new ApiResponse<string> { Success = false, Message = "Pending Quiz not found" });

            pendingQuiz.CompletedAt = DateTime.UtcNow;
            pendingQuiz.PercentageScore = quizDto.PercentageScore;

            var gainXp = xpService.CalculateXpGainsForQuizCompletion(pendingQuiz.Difficulty, pendingQuiz.Questions.Count, pendingQuiz.PercentageScore);

            pendingQuiz.XpAwardedForCompletion = gainXp;
            xpService.AwardXp(associatedUser, gainXp);

            if (!await unitOfWork.Complete()) return BadRequest(new ApiResponse<string> { Success = false, Message = "Quiz not saved" });

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "User Quiz saved",
                XpDetails = new UserXpDetailDto
                {
                    Level = associatedUser.Level,
                    LevelThreshold = xpService.GetXpThresholdForLevel(associatedUser.Level),
                    AppExperiencePoints = associatedUser.AppExperiencePoints
                }
            });
        }

        [HttpDelete("DeleteExistingQuiz")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteQuizAsync(int quizId)
        {
            var quiz = await unitOfWork.QuizRepository.GetQuizByIdAsync(quizId);

            if (quiz == null) return NotFound(false);

            unitOfWork.QuizRepository.DeleteQuiz(quiz);

            if (!await unitOfWork.Complete()) return BadRequest(new ApiResponse<string> { Success = false, Message = "Quiz not saved" });

            return Ok(new ApiResponse<string> { Success = true, Message = "Quiz removed successfully" });
        }

    }

}
