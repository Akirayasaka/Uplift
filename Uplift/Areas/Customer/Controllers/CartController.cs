using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Extensions;
using Uplift.Models;
using Uplift.Models.ViewModels;
using Uplift.Utility;

namespace Uplift.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public CartViewModel CartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            CartVM = new CartViewModel()
            {
                OrderHeader = new OrderHeader(),
                ServiceList = new List<Service>()
            };
        }

        #region 購物車頁面
        public IActionResult Index()
        {
            //導向到購物車頁面前, 先判斷session
            if (HttpContext.Session.GetObject<List<int>>(SD.SessionCart) != null)
            {
                List<int> sessionList = new();
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                foreach (var serviceId in sessionList)
                {
                    CartVM.ServiceList.Add(_unitOfWork.Service.GetFirstOrDefault(u => u.Id == serviceId, includeProperties: "Frequency,Category"));
                }
            }
            return View(CartVM);
        }
        #endregion

        #region Summary Page
        public IActionResult Summary()
        {
            // 先判斷session
            if (HttpContext.Session.GetObject<List<int>>(SD.SessionCart) != null)
            {
                List<int> sessionList = new List<int>();
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                foreach (var serviceId in sessionList)
                {
                    CartVM.ServiceList.Add(_unitOfWork.Service.GetFirstOrDefault(u => u.Id == serviceId, includeProperties: "Frequency,Category"));
                }
            }
            return View(CartVM);
        }
        #endregion

        #region 儲存訂單資訊
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            // 判斷session
            if (HttpContext.Session.GetObject<List<int>>(SD.SessionCart) != null)
            {
                List<int> sessionList = new();
                sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
                CartVM.ServiceList = new List<Service>();
                foreach (var serviceId in sessionList)
                {
                    // 只需要儲存Service相關
                    CartVM.ServiceList.Add(_unitOfWork.Service.Get(serviceId));
                }
            }

            // 判斷前端傳來的Model
            if (!ModelState.IsValid)
            {
                return View(CartVM);
            }
            else
            {
                // 訂單表頭(OrderHeader)
                CartVM.OrderHeader.OrderDate = DateTime.Now;
                CartVM.OrderHeader.Status = SD.StatusSubmitted;
                CartVM.OrderHeader.ServiceCount = CartVM.ServiceList.Count;
                _unitOfWork.OrderHeader.Add(CartVM.OrderHeader);
                _unitOfWork.Save();

                // 訂單表身(OrderDetails), 可能有多筆
                foreach(var service in CartVM.ServiceList)
                {
                    OrderDetails orderDetails = new()
                    {
                        ServiceId = service.Id,
                        OrderHeaderId = CartVM.OrderHeader.Id,
                        ServiceName = service.Name,
                        Price = service.Price,
                    };

                    _unitOfWork.OrderDetails.Add(orderDetails);
                }
                _unitOfWork.Save();

                // 儲存訂單後, 清空購物車 Session
                HttpContext.Session.SetObject(SD.SessionCart, new List<int>());
                return RedirectToAction("OrderConfirmation", "Cart", new { id = CartVM.OrderHeader.Id });
            }
        }
        #endregion

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }

        #region 移除單筆紀錄(From Session)
        public IActionResult Remove(int serviceId)
        {
            List<int> sessionList = new();
            sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
            sessionList.Remove(serviceId);
            HttpContext.Session.SetObject(SD.SessionCart, sessionList);

            return RedirectToAction("Index");
        }
        #endregion
    }
}
