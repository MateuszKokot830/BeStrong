namespace Application.Dto
{
    public class PhotoDto
    {
        public int Id { get; set; }
        public string PublicId { get; set; }
        public string Url { get; set; } 
        public bool IsProfilePhoto { get; set; }
    }
}