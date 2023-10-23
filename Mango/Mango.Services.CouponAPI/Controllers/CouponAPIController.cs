using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private ResponseDto _response;

        public CouponAPIController(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> objList = _appDbContext.Coupons.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(objList);
            }
            catch (Exception ex) 
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                var singleObj = _appDbContext.Coupons.Find(id);
                _response.Result = _mapper.Map<CouponDto>(singleObj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto Get(string code)
        {
            try
            {
                var singleObj = _appDbContext.Coupons.SingleOrDefault(x => x.CouponCode.ToLower() == code.ToLower());
                _response.Result = _mapper.Map<CouponDto>(singleObj); ;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                _appDbContext.Coupons.Add(_mapper.Map<Coupon>(couponDto));
                _appDbContext.SaveChanges();

                _response.Result = couponDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPut]
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
                _appDbContext.Coupons.Update(_mapper.Map<Coupon>(couponDto));
                _appDbContext.SaveChanges();

                _response.Result = couponDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ResponseDto Delete(int id)
        {
            try
            {
                var singleObj = _appDbContext.Coupons.Find(id);
                if (singleObj != null)
                {
                    _appDbContext.Remove(singleObj);
                    _appDbContext.SaveChanges();
                    _response.IsSuccess = true;
                    _response.Message = "Coupon was deleted.";
                    return _response;
                }
                _response.IsSuccess = false;
                _response.Message = "Coupon was not found.";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
