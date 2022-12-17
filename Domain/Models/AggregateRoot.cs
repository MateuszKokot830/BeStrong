namespace Domain.Models
{
    public abstract class AggregateRoot<TId> : Entity<TId>
        where TId : notnull
    {
    }
}