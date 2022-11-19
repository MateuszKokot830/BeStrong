namespace Domain.Entities
{
    public class Logs
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double DurationInMs { get; set; }
        public string User { get; set; }
        public string Table { get; set; }
        public string QueryParameters { get; set; }
        public string QueryType { get; set; }
        public string QueryText { get; set; }
    }
}