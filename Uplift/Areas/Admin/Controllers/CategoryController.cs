using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;
using Uplift.Utility;

namespace Uplift.Areas.Admin.Controllers
{
    [Authorize]
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

        public IActionResult Upsert(int? id)
        {
            Category category = new();
            if (id == null)
            {
                return View(category);
            }
            category = _unitOfWork.Category.Get(id.GetValueOrDefault());
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                if(category.Id == 0)
                {
                    _unitOfWork.Category.Add(category);
                }
                else
                {
                    _unitOfWork.Category.Update(category);
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(category);
        }


        #region API Calls
        /// <summary>
        /// 取得Category全部清單
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            //return Json(new { data = _unitOfWork.Category.GetAll() });

            #region Stored Procedure
            return Json(new { data = _unitOfWork.SP_Call.ReturnList<Category>(SD.usp_GetAllCategory, null)});
            #endregion
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
