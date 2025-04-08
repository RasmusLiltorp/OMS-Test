using System.Threading.Tasks;
using OMS_Test.Services;
using DTOs;

namespace OMS_Test.Services
{
    public interface IEmailService
    {
        Task SendOrderReceiptAsync(OrderDTO order);    
    }
}
