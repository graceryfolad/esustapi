using DataAccess.IRepositories;
using esust.Models;

namespace esust.Repositories
{
    public class ImageSliderRepo : BaseRepository<ImageSliderTbl>
    {
        public ImageSliderRepo(ESutContextDB dbContext) : base(dbContext)
        {
        }
    }
}
