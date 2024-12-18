using WebHM.Models;
using WebHM.Models.Momo;

namespace WebHM.Services
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentMomo(OrderInfoModel request);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    } 
}
