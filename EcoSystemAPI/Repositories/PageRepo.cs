using DataAccess.IRepositories;
using esust.Models;

namespace esust.Repositories
{
    public class PageRepo : BaseRepository<PageTbl>
    {
        public PageRepo(ESutContextDB dbContext) : base(dbContext)
        {
        }
    }
}
