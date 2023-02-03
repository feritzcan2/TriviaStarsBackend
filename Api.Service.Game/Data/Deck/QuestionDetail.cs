namespace Api.Service.GameHub.Data.Deck
{
    public class QuestionDetail
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public int Energy { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public List<string> IncorrectAnswers { get; set; }
        public string Difficulty { get; set; }
    }
}
