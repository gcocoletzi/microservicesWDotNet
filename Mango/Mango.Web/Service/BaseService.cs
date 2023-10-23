using Mango.Web.Models;
using Mango.Web.Service.IService;
using System.Net;
using System.Text;
using System.Text.Json;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ResponseDto> SendAsync(RequestDto requestDto)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("MangoAPI");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                //token
                message.RequestUri= new Uri(requestDto.Url);

                if(requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonSerializer.Serialize(requestDto.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage apiResponse = null;

                message.Method = requestDto.ApiType switch
                {
                    ApiType.POST => HttpMethod.Post,
                    ApiType.DELETE => HttpMethod.Delete,
                    ApiType.PUT => HttpMethod.Put,
                    _ => HttpMethod.Get
                };
            
                apiResponse = await client.SendAsync(message);

             
                return apiResponse.StatusCode switch
                {
                    HttpStatusCode.NotFound => new() { IsSuccess = false, Message = "Not Found" },
                    HttpStatusCode.Forbidden => new() { IsSuccess = false, Message = "Access Denied" },
                    HttpStatusCode.Unauthorized => new() { IsSuccess = false, Message = "Unauthorized" },
                    HttpStatusCode.InternalServerError => new() { IsSuccess = false, Message = "Internal Server Error" },
                    _ => JsonSerializer.Deserialize<ResponseDto>(
                            await apiResponse.Content.ReadAsStringAsync(),
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new ResponseDto()
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }

        }
    }
}
