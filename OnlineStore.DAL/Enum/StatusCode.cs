namespace OnlineStore.DAL.Enum
{
    public enum StatusCode
    {
        OK = 200,

        NoAccounts = 204,

        NoProducts = 204,

        NoOrders = 204,

        NoBanners = 204,

        NoSuchProducts = 204,

        LastProduct = 204,

        AccountAlreadyExist = 400,

        OutOfRange = 400,

        WrongPassword = 400,

        PasswordsDoNotMAtch = 400,

        AccountNotActivated = 401,

        AccountNotFound = 404,

        ProductInfoNotFound = 404,

        ProductNotFound = 404,

        CartNotFound = 404,

        OrderNotFound = 404,

        OrdersNotFound = 404,

        BannerNotFound = 404,

        InternalServerError = 500,

        FileConvertationError = 500
    }
}
