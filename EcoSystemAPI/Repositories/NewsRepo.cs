using DataAccess.IRepositories;
using esust.Models;

namespace esust.Repositories
{
    public class NewsRepo : BaseRepository<NewsTbl>
    {
        public NewsRepo(ESutContextDB dbContext) : base(dbContext)
        {
        }
    }
}
