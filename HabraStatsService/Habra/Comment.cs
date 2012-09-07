namespace HabraStatsService.Habra
{
    public class Comment
    {
        public string Id { get; set; }
        public int Score { get; set; }
        public int ScorePlus { get; set; }
        public int ScoreMinus { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", Score, Text);
        }
    }
}