using API.DTO_s;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace API.Controllers
{
    [Authorize]
    public class QuizController(IUnitOfWork unitOfWork, IMapper mapper, IQuizService quizService, IXpService xpService) : BaseApiController
    {
        // Get Call To get a history of most recent quizzes, returns with it's quiz Questions 
        // allows for checking of how many xp points were gained, completed questions etc.

        [HttpGet("GetAllCompletedQuizzes")]
        public async Task<ActionResult<List<Quiz>>> GetAllUserCompletedQuizzesAsync([FromQuery] QuizParams quizParams)
        {
            if (quizParams.UserId == null) return NotFound(new List<Quiz> { });

            var user = await unitOfWork.UserRepository.GetUserByIdAsync((int)quizParams.UserId);

            if(user == null) return NotFound(new List<Quiz> { });
            
            var quizzes = await unitOfWork.QuizRepository.GetUserQuizzesAsync(quizParams);

            if (quizzes.Count == 0 || quizzes == null) return NotFound(new List<Quiz> { });

            return Ok(quizzes);
        }

        [HttpGet("GetComputerScienceQuestions")] // historically all questions answered
        public async Task<ActionResult<List<QuizQuestionDto>>> GetComputerScienceQuestionsAsync([FromQuery] QuizInfoDto quizInfo)
        {
            var quizQuestions = await quizService.RetrieveQuestionsAsync(quizInfo.NumberOfQuestions, quizInfo.Difficulty, quizInfo.QuestionType);

            if (quizQuestions == null || quizQuestions.Count == 0) return NotFound(false);

            return Ok(quizQuestions);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> SaveCompletedQuizAsync(QuizDto quizDto)
        {
            if (quizDto == null) return NotFound(new ApiResponse<string> { Success = false, Message = "Quiz info not found"});

            var associatedUser = await unitOfWork.UserRepository.GetUserByIdAsync(quizDto.UserId);

            if (associatedUser == null) return NotFound(new ApiResponse<string> { Success = false, Message = "Associated user not found" });

            var completedQuiz = mapper.Map<Quiz>(quizDto);

            completedQuiz.CompletedAt = DateTime.UtcNow;

            await unitOfWork.QuizRepository.SaveQuizAsync(completedQuiz);

            var gainXp = xpService.CalculateXpGainsForQuizCompletion(completedQuiz.Difficulty, completedQuiz.Questions.Count, completedQuiz.PercentageScore);

            completedQuiz.XpAwardedForCompletion = gainXp;
            xpService.AwardXp(associatedUser, gainXp);

            if (!await unitOfWork.Complete()) return BadRequest(new ApiResponse<string> { Success = false, Message = "Quiz not saved" });

            return Ok(new ApiResponse<string> 
            {
                Success = true,
                Message = "User Quiz saved"
            });
        }
    }

}
