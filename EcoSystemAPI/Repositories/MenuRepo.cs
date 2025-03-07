using DataAccess.IRepositories;
using esust.Models;

namespace esust.Repositories
{
    public class MenuRepo : BaseRepository<MenuTbl>
    {
        public MenuRepo(ESutContextDB dbContext) : base(dbContext)
        {
        }
    }
}
