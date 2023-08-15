namespace TerritorialHQ.Models.ViewModels
{
    public class LeaderboardEntry
    {
        public string? Id { get; set; }
        public bool IsPublished { get; set; }
        public int Rank { get; set; }
        public string? Tag { get; set; }
        public string? LogoFile { get; set; }
        public decimal Points { get; set; }
        public decimal RankThreshold { get; set; }
        public string PointsDiplay 
        { 
            get
            {
                if (Points == 0)
                {
                    return $"<{string.Format("{0:0.##}",RankThreshold)}";
                }
                else
                {
                    return string.Format("{0:0.##}", Points);
                }
            }
        }

        public string OrdinalRank {
            get 
            {
                int TempRank = Rank;

                if (TempRank <= 0) return "<250th";

                switch (TempRank % 100)
                {
                    case 11:
                    case 12:
                    case 13:
                        return TempRank + "th";
                }

                switch (TempRank % 10)
                {
                    case 1:
                        return TempRank + "st";
                    case 2:
                        return TempRank + "nd";
                    case 3:
                        return TempRank + "rd";
                    default:
                        return TempRank + "th";
                }
            }
        }
    }
}
