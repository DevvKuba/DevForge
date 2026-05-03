using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class BlogLikeRepository(DataContext context, IMapper mapper) : IBlogLikeRepository
    {
    }
}
