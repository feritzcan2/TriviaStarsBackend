using Api.Service;
using Api.Service.Data.OpenTriviaDb;
using Api.Service.Data.Request;
using MagicOnion;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionReviewController : ControllerBase
    {
       
        private OpenTriviaDbService _service;

        public QuestionReviewController( OpenTriviaDbService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("catagories")]
        public async Task<TriviaCategory[]> GetCatagories()
        {
            return await _service.GetCatagories();
        }

        [HttpGet]
        [Route("questions")]
        public async Task<IReadOnlyList<Service.Repository.DbEntities.DbQuestionInReview>> GetCatagories(int page,int pageSize,int catagory)
        {
            return await _service.QueryByPage(page,pageSize,catagory);
        }
        
        [HttpPost]
        [Route("updateQuestion")]
        public async Task<bool> GetCatagories([FromBody] UpdateQuestionRequest request)
        {
             await _service.SaveQuestion(request);
             return true;
        }
    }
}