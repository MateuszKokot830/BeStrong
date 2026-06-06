namespace Application.Dto.Photo
{
    public record PhotoDto(
        int Id,
        string? PublicId,
        string? Url,
        bool IsProfilePhoto
    );
}