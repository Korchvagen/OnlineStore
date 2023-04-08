using OnlineStore.DAL.Interfaces;
using OnlineStore.DAL.Enum;

namespace OnlineStore.DAL.Response
{
    public class BaseResponse<T> : IBaseResponse<T>
    {
        public string Description { get; set; }
        public StatusCode StatusCode { get; set; }
        public T Data { get; set; }
    }
}
