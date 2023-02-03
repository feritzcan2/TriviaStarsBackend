using Api.Service.GameHub.Contracts;
using Api.Service.GameHub.Data.Deck;
using GameEngine.Entities.Data.Questions;

namespace Api.Service.GameHub.DeckManagement
{
    public class QuestionManager
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionManager(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }
        public async Task<List<QuestionDetail>> GenerateStartingQuestions()
        {
            var questions = await _questionRepository.GetQuestions();
            var randEasy = questions.Where(x => x.Difficulty == "easy").OrderBy(arg => Guid.NewGuid()).Take(2).ToList();
            var randHard = questions.Where(x => x.Difficulty == "hard").OrderBy(arg => Guid.NewGuid()).Take(1).ToList();
            var randMediom = questions.Where(x => x.Difficulty == "medium").OrderBy(arg => Guid.NewGuid()).Take(2).ToList();
            return randMediom.Concat(randEasy).Concat(randHard).ToList();
        }

        public async Task<List<QuestionDetail>> GetRandomCards(int count)
        {
            var quest = new List<QuestionDetail>();
            var questions = await _questionRepository.GetQuestions();
            while (quest.Count < count)
            {
                var index = Random.Shared.Next(0, questions.Count);
                var question = questions[index];
                if (!quest.Contains(question))
                {
                    quest.Add(question);
                }
            }

            return quest;

        }
        public async Task<List<QuestionDetail>> GetQuestions()
        {
            return await _questionRepository.GetQuestions();
        }


        public async Task<QuestionDto> GetQuestionDto(string id)
        {
            var questions = await _questionRepository.GetQuestions();
            var question = questions.FirstOrDefault(x => x.Id == id);
            return new QuestionDto
            {
                Energy = question.Energy,
                CorrectAnswer = question.CorrectAnswer,
                IncorrectAnswers = question.IncorrectAnswers,
                Question = question.Question
            };
        }

        public async Task<QuestionDetail> GetQuestion(string id)
        {
            var questions = await _questionRepository.GetQuestions();
            var question = questions.FirstOrDefault(x => x.Id == id);
            return question;
        }

    }
}
