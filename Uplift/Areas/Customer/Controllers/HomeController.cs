using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Extensions;
using Uplift.Models;
using Uplift.Models.ViewModels;
using Uplift.Utility;

namespace Uplift.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private HomeViewModel HomeVM;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// 客戶端首頁
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            HomeVM = new HomeViewModel()
            {
                CategoryList = _unitOfWork.Category.GetAll(),
                ServicesList = _unitOfWork.Service.GetAll(includeProperties: "Frequency")
            };

            return View(HomeVM);
        }

        /// <summary>
        /// 詳細資料(客戶端)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Details(int id)
        {
            var serviceFromDb = _unitOfWork.Service.GetFirstOrDefault(includeProperties:"Category,Frequency", filter: c => c.Id == id);
            return View(serviceFromDb);
        }

        /// <summary>
        /// 商品加入購物車(存放於session)
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public IActionResult AddToCart(int serviceId)
        {
            // 陣列紀錄serviceId
            List<int> sessionList = new();

            // 從session沒有取到"Cart"
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SD.SessionCart)))
            {
                // 新增到session
                sessionList.Add(serviceId);
                HttpContext.Session.SetObject(SD.SessionCart, sessionList);
            }
            else
            {
                // 有取得"Cart"則取出做檢查
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);

                // 清單內沒有包含這筆id, 就新增進session
                if (!sessionList.Contains(serviceId))
                {
                    sessionList.Add(serviceId);
                    HttpContext.Session.SetObject(SD.SessionCart, sessionList);
                }
            }
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
