using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class BlogRepository(DataContext context, IMapper mapper) : IBlogRepository
    {
    }
}
