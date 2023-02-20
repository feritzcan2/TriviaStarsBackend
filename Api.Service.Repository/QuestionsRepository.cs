using Api.Service.GameHub.Contracts;
using Api.Service.GameHub.Data.Deck;
using System.Text.Json;

namespace Api.Service.Repository
{
    public class QuestionsRepository : IQuestionRepository
    {
        private List<OpenTriviaDbQuestion> _questions;

        public enum QuestionCatagory
        {
            Music = 12,
            Film = 11,
            GeneralKnowledge = 9,
            VideoGames = 15,
        }

        public enum QuestionDifficulty
        {
            Easy,
            Medium,
            Hard
        }

        public async Task<List<QuestionDetail>> GetQuestions()
        {
            var questions = await GetOpenDbQuestions();
            return questions.Select(x => new QuestionDetail
            {
                Id = x.Id,
                Category = x.category,
                CorrectAnswer = x.correct_answer,
                Question = x.question,
                Type = x.type,
                Difficulty = x.difficulty,
                IncorrectAnswers = x.incorrect_answers,
                Energy = x.Energy
            }).ToList();

        }

        private async Task<List<OpenTriviaDbQuestion>?> GetOpenDbQuestions()
        {
            var questions = new List<OpenTriviaDbQuestion>();

            if (_questions != null) return _questions;
#if DEBUG

            var questionsFilePath = $"{System.IO.Directory.GetCurrentDirectory()}\\Questions.txt";
            if (File.Exists(questionsFilePath))
            {
                var text = File.ReadAllText(questionsFilePath);
                questions = JsonSerializer.Deserialize<List<OpenTriviaDbQuestion>>(text);
                return questions;
            }
#endif

            var client = new HttpClient();
            foreach (QuestionCatagory catagory in (QuestionCatagory[])Enum.GetValues(typeof(QuestionCatagory)))
            {
                foreach (QuestionDifficulty difficulty in (QuestionDifficulty[])Enum.GetValues(typeof(QuestionDifficulty)))
                {
                    var difficultyStr = Enum.GetName(typeof(QuestionDifficulty), difficulty);
                    var resp = await client.GetAsync($"https://opentdb.com/api.php?amount=50&category={(int)catagory}&difficulty={difficultyStr.ToLower()}&type=multiple");
                    var strContent = await resp.Content.ReadAsStreamAsync();
                    var result = JsonSerializer.Deserialize<OpenTriviaResponse>(strContent);
                    if (result != null)
                    {
                        questions.AddRange(result.results);
                    }
                }
            }

            foreach (var quest in questions)
            {
                quest.Id = Guid.NewGuid().ToString();

                quest.Energy = quest.difficulty == "hard"
                    ? 3
                    : (quest.difficulty == "medium" ? 2 : 1);
            }

#if DEBUG
            File.WriteAllText(questionsFilePath, JsonSerializer.Serialize(questions));

#endif
            _questions = questions;
            return questions;
        }
    }

    public class OpenTriviaDbQuestion
    {
        public string Id { get; set; }
        public string category { get; set; }
        public string type { get; set; }
        public string difficulty { get; set; }
        public string question { get; set; }
        public string correct_answer { get; set; }
        public List<string> incorrect_answers { get; set; }
        public int Energy { get; set; }
    }

    public class OpenTriviaResponse
    {
        public int response_code { get; set; }
        public List<OpenTriviaDbQuestion> results { get; set; }
    }
}