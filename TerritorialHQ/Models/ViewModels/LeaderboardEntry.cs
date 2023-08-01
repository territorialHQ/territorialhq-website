namespace TerritorialHQ.ViewModels
{
    public class LeaderboardEntry
    {
        public string? Id { get; set; }
        public bool IsPublished { get; set; }
        public int Rank { get; set; }
        public string? Tag { get; set; }
        public string? LogoFile { get; set; }
        public decimal Points { get; set; }
    }
}
