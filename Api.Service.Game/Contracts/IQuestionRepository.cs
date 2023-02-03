using Api.Service.GameHub.Data.Deck;

namespace Api.Service.GameHub.Contracts
{
    public interface IQuestionRepository
    {
        Task<List<QuestionDetail>> GetQuestions();
    }
}
