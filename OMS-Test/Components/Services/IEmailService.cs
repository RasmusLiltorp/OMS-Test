using System.Threading.Tasks;
using OMS_Services;
using DTOs;

namespace OMS_Services
{
    public interface IEmailService
    {
        Task SendOrderReceiptAsync(OrderDTO order);    
    }
}
