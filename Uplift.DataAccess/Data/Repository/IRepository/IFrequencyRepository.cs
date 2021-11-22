using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository.IRepository
{
    public interface IFrequencyRepository: IRepository<Frequency>
    {
        /// <summary>
        /// 取得DropDown List
        /// </summary>
        /// <returns></returns>
        IEnumerable<SelectListItem> GetFrequencyListForDropDown();

        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="frequency"></param>
        void Update(Frequency frequency);
    }
}
