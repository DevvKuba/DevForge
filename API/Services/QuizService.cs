using API.DTO_s;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

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

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var quizQuestions = JsonSerializer.Deserialize<RootResponseDto>(json, options);

            if (quizQuestions == null || quizQuestions.Results.Count == 0) return [];

            // Filter special characters from questions and answers
            foreach (var question in quizQuestions.Results)
            {
                question.Question = RemoveSpecialCharacters(question.Question);
                question.CorrectAnswer = RemoveSpecialCharacters(question.CorrectAnswer);
                question.IncorrectAnswers = question.IncorrectAnswers
                    .Select(answer => RemoveSpecialCharacters(answer))
                    .ToList();
            }

            return quizQuestions.Results;
        }

        public string RemoveSpecialCharacters(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Replace HTML entities with their decoded values
            var decodedText = System.Net.WebUtility.HtmlDecode(input);
            // Remove all special characters except letters, numbers, spaces, and common punctuation
            var cleaned = Regex.Replace(decodedText, @"[^a-zA-Z0-9\s\-&'().,?!]", string.Empty);
            // Normalize whitespace
            cleaned = Regex.Replace(cleaned, @"\s+", " ").Trim();

            return cleaned;
        }
    }
}
