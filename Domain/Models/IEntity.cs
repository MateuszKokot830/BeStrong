namespace Domain.Models
{
    public interface IEntity<TId>
    {
        TId Id { get; }
    }
}
