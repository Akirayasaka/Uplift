using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// 取得DropDown List
        /// </summary>
        /// <returns></returns>
        IEnumerable<SelectListItem> GetCategoryListForDropDown();

        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="category"></param>
        void Update(Category category);
    }
}
