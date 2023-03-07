using Api.Service.Repository.DbEntities;

namespace Api.Service.Data.Request;

public class UpdateQuestionRequest
{
    public DbQuestionInReview Question { get; set; }
    
    public bool Accepted { get; set; }
    
}