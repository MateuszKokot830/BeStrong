namespace Application.Dto.User
{
    public record MeasurementsDto(
        int? Height,
        decimal? Weight,
        decimal? Chest,
        decimal? Shoulders,
        decimal? Arms,
        decimal? Waist,
        decimal? Hips,
        decimal? Thights
    );
}