using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class BlogCommentRepository(DataContext context, IMapper mapper) : IBlogCommentRepository
    {
    }
}
