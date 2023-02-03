using Api.Service.GameHub.Data.Deck;
using Api.Service.GameHub.Data.Player;
using GameEngine.Entities.Data.Deck;

namespace Api.Service.GameHub.DeckManagement
{
    public class DeckManager
    {
        private readonly QuestionManager _questionManager;

        public DeckManager(QuestionManager questionManager)
        {
            _questionManager = questionManager;
        }
        public async Task<Deck> GenerateStartingDeck()
        {
            var questions = await _questionManager.GenerateStartingQuestions();

            var gameDeck = new Deck
            {
                Cards = questions.Select(x => new PlayerCard
                {
                    Id = Guid.NewGuid().ToString(),
                    QuestionId = x.Id
                }).ToList()
            };



            return gameDeck;
        }

        public async Task<DeckDto> ToDeckDto(Deck deck)
        {
            var questions = await _questionManager.GetQuestions();

            var deckDto = new DeckDto
            {
                Cards = deck.Cards.Select(x => new CardDto
                {
                    Id = x.Id,
                    Energy = questions.FirstOrDefault(y => y.Id == x.QuestionId).Energy,
                    Catagory = questions.FirstOrDefault(y => y.Id == x.QuestionId).Category
                }).ToList()
            };
            return deckDto;
        }


        public async Task<IList<CardDto>> FillCards(GamePlayer player)
        {
            var dto = new List<CardDto>();
            var questions = await _questionManager.GetRandomCards(5 - player.Deck.Cards.Count);
            foreach (var question in questions)
            {
                var card = new PlayerCard { Id = Guid.NewGuid().ToString(), QuestionId = question.Id };
                player.Deck.Cards.Add(card);
                dto.Add(new CardDto { Energy = question.Energy, Catagory = question.Category, Id = card.Id });

            }
            return dto;
        }
    }
}
