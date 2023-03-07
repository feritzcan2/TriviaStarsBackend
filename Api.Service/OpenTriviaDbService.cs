﻿using Api.Service.Data.OpenTriviaDb;
using Api.Service.Repository;
using Api.Service.Repository.DbEntities;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Net.Http;
using static System.Net.WebRequestMethods;

namespace Api.Service
{
    public class OpenTriviaDbService
    {
        private IMemoryCache _cache;
        private IHttpClientFactory _httpClient;
        private QuestionsInReviewRepository _questionsInReviewRepository;

        public OpenTriviaDbService(IHttpClientFactory httpClient,IMemoryCache cache, QuestionsInReviewRepository questionsInReviewRepository)
        {
            _cache = cache;
            _httpClient = httpClient;
            _questionsInReviewRepository = questionsInReviewRepository;
        }

        public async Task SyncQuestions()
        {
            var random = new Random();
            var questions = new List<OpenTriviaDbQuestionResp>();

            var catagories = await GetCatagories();
            var client = _httpClient.CreateClient();
            foreach (var catagory in catagories)
            {
                foreach (QuestionDifficulty difficulty in (QuestionDifficulty[])Enum.GetValues(typeof(QuestionDifficulty)))
                {
                    var difficultyStr = Enum.GetName(typeof(QuestionDifficulty), difficulty);
                    var resp = await client.GetAsync($"https://opentdb.com/api.php?amount=50&category={(int)catagory.Id}&difficulty={difficultyStr.ToLower()}&type=multiple");
                    var strContent = await resp.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<OpenTriviaQuestionsResponse>(strContent);
                    if (result != null)
                    {
                        questions.AddRange(result.results);
                    }
                    await Task.Delay(random.Next(5000, 40000));
                }
            }
            var questionsInReview = await _questionsInReviewRepository.GetAllCachedEntries();
            questions = questions.Where(x => !questionsInReview.Any(y => y.Question == x.question && y.Category == x.category)).ToList();
            if(questions.Count > 0)
            {
                var db = questions.Select(x => new DbQuestionInReview
                {
                    Category = x.category,
                    CorrectAnswer = x.correct_answer,
                    Difficulty = x.difficulty,
                    IncorrectAnswers = x.incorrect_answers,
                    Status = ReviewStatus.Waiting,
                    Question = x.question,
                    Type = x.type,
                }).ToList();
                await _questionsInReviewRepository.InsertManyAsync(db);
            }
            
        }

        public async Task<TriviaCategory[]> GetCatagories()
        {
            var cached = _cache.Get<TriviaCategory[]>("TriviaCategories");
            if (cached != null) return cached;

            var url = "https://opentdb.com/api_category.php";
            var resp = await _httpClient.CreateClient().GetAsync(url);
            var strContent = await resp.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CatagoriesResponse>(strContent);
            if (result != null && result.TriviaCategories!=null)
            {
                _cache.Set<TriviaCategory[]>("TriviaCategories", result.TriviaCategories,TimeSpan.FromMinutes(180));
                return result.TriviaCategories;
            }
            throw new Exception("Catagories cannot be found ");
        }
        public async Task<IReadOnlyList<DbQuestionInReview>> QueryByPage(int page, int pageSize)
        {
            return await _questionsInReviewRepository.QueryByPage(page, pageSize);
        }
    }

    public enum QuestionDifficulty
    {
        Easy,
        Medium,
        Hard
    }
}