using Application.Interfaces.Repositories;
using Domain.Aggregates;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class PostRepository(DataContext context) : BaseRepository<Post>(context), IPostRepository
    {
    }
}
