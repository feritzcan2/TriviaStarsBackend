using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Service.Data.OpenTriviaDb
{
    public class OpenTriviaDbQuestionResp
    {
        public long catagoryId;
        public string category { get; set; }
        public string type { get; set; }
        public string difficulty { get; set; }
        public string question { get; set; }
        public string correct_answer { get; set; }
        public List<string> incorrect_answers { get; set; }
    }

    public class OpenTriviaQuestionsResponse
    {
        public int response_code { get; set; }
        public List<OpenTriviaDbQuestionResp> results { get; set; }
    }
}
