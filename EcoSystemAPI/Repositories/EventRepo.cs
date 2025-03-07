using DataAccess.IRepositories;
using esust.Models;

namespace esust.Repositories
{
    public class EventRepo : BaseRepository<EventTbl>
    {
        public EventRepo(ESutContextDB dbContext) : base(dbContext)
        {
        }
    }
}
