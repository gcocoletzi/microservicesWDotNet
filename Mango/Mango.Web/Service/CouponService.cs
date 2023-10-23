using Mango.Web.Models;
using Mango.Web.Service.IService;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateCouponAsync(CouponDto coupon)
        {
            var request = new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = coupon,
                Url = string.Concat(CouponAPIBase, "/api/coupon")
            };
            var response = await _baseService.SendAsync(request);

            return response;
        }

        public async Task<ResponseDto?> DeleteCouponAsync(int couponId)
        {
            var request = new RequestDto()
            {
                ApiType = ApiType.DELETE,
                Url = string.Concat(CouponAPIBase, "/api/coupon/", couponId.ToString())
            };
            var response = await _baseService.SendAsync(request);

            return response;
        }

        public async Task<ResponseDto?> GetAllCouponsAsync()
        {
            var request = new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = string.Concat(CouponAPIBase, "/api/coupon")
            };
            var response = await _baseService.SendAsync(request);

            return response;
        }

        public async Task<ResponseDto?> GetCouponAsync(string couponcode)
        {
            var request = new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = string.Concat(CouponAPIBase, "/api/coupon/GetByCode/", couponcode)
            };
            var response = await _baseService.SendAsync(request);

            return response;
        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int couponId)
        {
            var request = new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = string.Concat(CouponAPIBase, "/api/coupon/", couponId.ToString())
            };
            var response = await _baseService.SendAsync(request);

            return response;
        }

        public async Task<ResponseDto?> UpdateCouponAsync(CouponDto coupon)
        {
            var request = new RequestDto()
            {
                ApiType = ApiType.PUT,
                Data = coupon,
                Url = string.Concat(CouponAPIBase, "/api/coupon/")
            };
            var response = await _baseService.SendAsync(request);

            return response;
        }
    }
}
