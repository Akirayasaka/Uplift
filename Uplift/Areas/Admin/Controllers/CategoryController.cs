using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.IRepository;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region API Calls
        /// <summary>
        /// 取得Category全部清單
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Category.GetAll() });
        }

        /// <summary>
        /// 刪除單筆Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            // 取得單筆資料物件
            var objFromDb = _unitOfWork.Category.Get(id);

            // 物件為空, 則回傳錯誤訊息
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }

            // 移除資料
            _unitOfWork.Category.Remove(objFromDb);
            // 儲存變更
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}
