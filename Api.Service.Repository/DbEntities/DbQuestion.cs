
namespace Api.Service.Repository.DbEntities
{
    [BsonCollection("Questions")]
    public class DbQuestion : BaseEntity
    {
        public string Category { get; set; }
        public string Type { get; set; }
        public string Difficulty { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public List<string> IncorrectAnswers { get; set; }
        public long CatagoryId { get; set; }
    }
}