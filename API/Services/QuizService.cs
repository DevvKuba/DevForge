using API.DTO_s;
using API.Helpers;
using API.Interfaces;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace API.Services
{
    public class QuizService(IUnitOfWork unitOfWork, HttpClient httpClient) : IQuizService
    {
        public string BaseUrl { get; } = "https://opentdb.com/api.php?";

        public int ComputerScienceCategoryId { get; } = 18;

        public async Task<List<QuizQuestionDto>> RetrieveQuestionsAsync(int numberOfQuestions, string difficulty, string questionType)
        {
            var requestUrl = BaseUrl + "amount=" + numberOfQuestions.ToString() + "&difficulty=" +
                difficulty + "&category=" + ComputerScienceCategoryId + "&type=" +questionType;

            var response = await httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode) return [];

            var json = await response.Content.ReadAsStringAsync();

            var quizQuestions = JsonSerializer.Deserialize<RootResponseDto>(json);

            return [];
        }
    }
}
