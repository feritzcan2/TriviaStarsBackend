namespace Api.Service.GameHub.Data.Room
{
    public class GameRoundInfo
    {
        public DateTime? RoundStartDate { get; set; }
        public int Round { get; set; }

        public Dictionary<string, PlayerRoundPlayData> RoundPlayData { get; set; } = new Dictionary<string, PlayerRoundPlayData>();

        public bool CheckTurnsEnded()
        {
            foreach (var value in RoundPlayData.Values)
            {
                if (value.TurnEnded == false) return false;
            }

            return true;
        }
        public void SetNewRound()
        {
            foreach (var keyData in RoundPlayData)
            {
                keyData.Value.Reset();
            }
            Round++;
            RoundStartDate = DateTime.UtcNow;
        }
    }

    public class PlayerRoundPlayData
    {
        public bool TurnEnded { get; set; }
        public int PlayedCardCount { get; set; }

        public void Reset()
        {
            PlayedCardCount = 0;
            TurnEnded = false;
        }


    }
}
