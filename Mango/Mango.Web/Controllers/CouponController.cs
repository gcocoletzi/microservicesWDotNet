﻿using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            var couponsList = new List<CouponDto?>();
            var response = await _couponService.GetAllCouponsAsync();

            if (response != null && response.IsSuccess)
            {
                couponsList = JsonSerializer.Deserialize<List<CouponDto?>>(
                        response.Result?.ToString(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(couponsList);
        }

        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _couponService.CreateCouponAsync(model);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Coupon created successfully!";
                    return RedirectToAction(nameof(CouponIndex));
                }
				else
				{
					TempData["error"] = response?.Message;
				}
			}
            return View(model);
        }
        
        public async Task<IActionResult> CouponDelete(int couponId)
        {
            var response = await _couponService.GetCouponByIdAsync(couponId);

            if (response != null && response.IsSuccess)
            {
                var model = JsonSerializer.Deserialize<CouponDto>(
                        response.Result?.ToString(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
		public async Task<IActionResult> CouponDelete(CouponDto coupon)
		{
			var response = await _couponService.DeleteCouponAsync(coupon.CouponId);
			if (response != null && response.IsSuccess)
			{
                TempData["success"] = "Coupon deleted successfully!";
                return RedirectToAction(nameof(CouponIndex));
			}
			else
			{
				TempData["error"] = response?.Message;
			}

			return View(coupon);
		}
	}
}
