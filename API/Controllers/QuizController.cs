using API.DTO_s;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class QuizController(IUnitOfWork unitOfWork, IMapper mapper, IQuizService quizService) : BaseApiController
    {
        [HttpGet("GetComputerScienceQuestions")]
        public async Task<ActionResult<List<QuizQuestionDto>>> GetComputerScienceQuestionsAsync([FromQuery] QuizInfoDto quizInfo)
        {
            var quizQuestions = await quizService.RetrieveQuestionsAsync(quizInfo.NumberOfQuestions, quizInfo.Difficulty, quizInfo.QuestionType);

            if (quizQuestions == null || quizQuestions.Count == 0) return NotFound(false);

            return Ok(quizQuestions);
        }
    }
}
