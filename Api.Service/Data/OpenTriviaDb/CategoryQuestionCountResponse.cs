using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Service.Data.OpenTriviaDb
{
    public  class   CategoryQuestionCountResponse

    {
        [JsonProperty("category_id")]
        public long CategoryId { get; set; }

        [JsonProperty("category_question_count")]
        public CategoryQuestionCount CategoryQuestionCount { get; set; }
    }

    public  class CategoryQuestionCount
    {
        [JsonProperty("total_question_count")]
        public long TotalQuestionCount { get; set; }

        [JsonProperty("total_easy_question_count")]
        public long TotalEasyQuestionCount { get; set; }

        [JsonProperty("total_medium_question_count")]
        public long TotalMediumQuestionCount { get; set; }

        [JsonProperty("total_hard_question_count")]
        public long TotalHardQuestionCount { get; set; }
    }
}
