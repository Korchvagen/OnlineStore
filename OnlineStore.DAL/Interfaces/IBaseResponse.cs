using OnlineStore.DAL.Enum;

namespace OnlineStore.DAL.Interfaces
{
    public interface IBaseResponse<T>
    {
        string Description { get; set; }
        StatusCode StatusCode { get; set; }
        T Data { get; set; }
    }
}
