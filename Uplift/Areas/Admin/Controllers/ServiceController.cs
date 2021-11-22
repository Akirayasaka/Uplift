using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models.ViewModels;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IWebHostEnvironment _hostEnvironment; // 上傳檔案用

        public ServiceController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }


        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增/修改 頁面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Upsert(int? id)
        {
            ServiceVM servVM = new()
            {
                Service = new Models.Service(),
                CategoryLIst = _unitOfWork.Category.GetCategoryListForDropDown(),
                FrequencyList = _unitOfWork.Frequency.GetFrequencyListForDropDown()
            };
            if(id != null)
            {
                servVM.Service = _unitOfWork.Service.Get(id.GetValueOrDefault());
            }


            return View(servVM);
        }


        #region API Calls
        public IActionResult GetAll()
        {
            // 有加入 includeProperties: "Category,Frequency", 才會帶入關聯的資料
            return Json(new { data = _unitOfWork.Service.GetAll(includeProperties: "Category,Frequency")});
        }
        #endregion
    }
}
