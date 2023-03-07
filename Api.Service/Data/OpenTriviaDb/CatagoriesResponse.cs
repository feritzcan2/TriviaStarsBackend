using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Service.Data.OpenTriviaDb
{

    public  class CatagoriesResponse
    {
        [JsonProperty("trivia_categories")]
        public TriviaCategory[] TriviaCategories { get; set; }
    }

    public  class TriviaCategory
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
