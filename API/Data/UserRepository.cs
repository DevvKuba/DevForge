using API.DTO_s;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API;

// where we aquire methods that interact with the database 
public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    public async Task<MemberDto?> GetMemberAsync(string username)
    {
        return await context.Users
            // use normalisedUser == username.ToUpper() if need be
            .Where(x => x.UserName == username)
            .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
    {
        var query = context.Users.AsQueryable();

        query = query.Where(x => x.Id != userParams.CurrentUserId);

        var minExperience = userParams.MinExperience;
        var maxExperience = userParams.MaxExperience;

        query = query.Where(x => x.YearsOfExperience >= minExperience && x.YearsOfExperience <= maxExperience);

        query = userParams.OrderBy switch
        {
            "created" => query.OrderByDescending(x => x.Created),
            _ => query.OrderByDescending(x => x.LastActive),
        };

        return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(mapper.ConfigurationProvider),
            userParams.PageNumber,
            userParams.PageSize);
    }

    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        var user = await context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        return user;
    }

    // method created in order to find user via photoId but also include their photos
    public async Task<AppUser?> GetUserByPhotoIdAsync(int photoId)
    {
        return await context.Users
            .Include(x => x.Photos)
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Photos.Any(x => x.Id == photoId));
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await context.Users
            .Include(x => x.Photos)
            .ToListAsync();
    }

    public void UpdateAppExperienceAndLevel(AppUser user, int newPoints, int level)
    {
        user.AppExperiencePoints = newPoints;
        user.Level = level;
    }

    public void Update(AppUser user)
    {
        context.Entry(user).State = EntityState.Modified;
    }

}
