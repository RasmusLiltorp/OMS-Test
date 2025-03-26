using System.Threading.Tasks;
using OMS_Test.Services;

namespace OMS_Test.Services
{
    public interface IEmailService
    {
        Task SendOrderReceiptAsync(OrderLine order);
    }
}
