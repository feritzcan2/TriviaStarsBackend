
namespace Api.Service.Repository.DbEntities
{
    [BsonCollection("QuestionsInReview")]
    public class DbQuestionInReview : BaseEntity
    {
        public ReviewStatus Status { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Difficulty { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public List<string> IncorrectAnswers { get; set; }
    }

    public enum ReviewStatus
    {
        Waiting,
        Selected,
        Unselected,
    }
}