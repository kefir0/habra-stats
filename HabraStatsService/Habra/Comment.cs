namespace HabraStatsService.Habra
{
    internal class Comment
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public int ScorePlus { get; set; }
        public int ScoreMinus { get; set; }
        public string Text { get; set; }
    }
}