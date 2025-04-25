using System.Net;
using System.Net.Mail;
using System.Text;
using DTOs;

namespace OMS_Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly DataService _dataService;

        public SmtpEmailService(DataService dataService)
        {
            _dataService = dataService;
        }

        public async Task SendOrderReceiptAsync(OrderDTO order)
        {
            /*if (order.CustomerInfo.Email == null)
            {
                Console.WriteLine("No customer email provided.");
                return;
            }*/
            
            var sb = new StringBuilder();
            var total = _dataService.CalculateOrdertotal(order);

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html><head><meta charset='UTF-8'><title>Receipt</title></head><body style='font-family: Arial, sans-serif;'>");

            sb.AppendLine("<div style='max-width: 700px; margin: auto; padding: 20px; border: 1px solid #eee;'>");

            sb.AppendLine("<h2 style='color: #444;'>Arnes Elektronik</h2>");
            sb.AppendLine("<p>Campusvej 55, 5230 Odense<br>arneselektronik@gmail.com<br>+45 12 34 56 78</p>");
            
            sb.AppendLine("<hr style='margin: 20px 0;'>");

            sb.AppendLine("<table style='width: 100%; margin-top: 20px;'>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td style='vertical-align: top;'>");
            sb.AppendLine("<p><strong>Billed To:</strong><br>");
            sb.AppendLine($"{order.CustomerInfo.Name}<br>");
            sb.AppendLine("Unknown Address<br>");
            sb.AppendLine("<a href='mailto:Customer@mail.com'>Customer@mail.com</a><br>");
            sb.AppendLine("+45 87 65 43 21</p>");
            sb.AppendLine("</td>");
            sb.AppendLine("<td style='text-align: right; vertical-align: top;'>");
            sb.AppendLine($"<p><strong>Invoice Date:</strong> {order.Date:dd MMM yyyy}<br>");
            sb.AppendLine($"<strong>Order No:</strong> {order.OrderId}</p>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</table>");

            sb.AppendLine("<h4 style='margin-top: 30px;'>Order Summary</h4>");

            sb.AppendLine("<table style='width: 100%; border-collapse: collapse;'>");
            sb.AppendLine("<thead>");
            sb.AppendLine("<tr style='background-color: #f2f2f2;'>");
            sb.AppendLine("<th style='padding: 8px; border: 1px solid #ddd;'>No.</th>");
            sb.AppendLine("<th style='padding: 8px; border: 1px solid #ddd;'>Item</th>");
            sb.AppendLine("<th style='padding: 8px; border: 1px solid #ddd;'>Price</th>");
            sb.AppendLine("<th style='padding: 8px; border: 1px solid #ddd;'>Quantity</th>");
            sb.AppendLine("<th style='padding: 8px; border: 1px solid #ddd;'>Total</th>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead><tbody>");

            int index = 1;
            foreach (var lineElement in order.LineElements)
            {
                var product = _dataService.Products.FirstOrDefault(p => p.ProductID == lineElement.ProductUuid.ToString());
                
                if (product != null)
                {
                    decimal itemTotal = lineElement.Price * lineElement.Amount;
                    sb.AppendLine("<tr>");
                    sb.AppendLine($"<td style='padding: 8px; border: 1px solid #ddd;'>{index}</td>");
                    sb.AppendLine($"<td style='padding: 8px; border: 1px solid #ddd;'>{product.ProductName}</td>");
                    sb.AppendLine($"<td style='padding: 8px; border: 1px solid #ddd;'>{lineElement.Price.ToString("C")}</td>");
                    sb.AppendLine($"<td style='padding: 8px; border: 1px solid #ddd;'>{lineElement.Amount}</td>");
                    sb.AppendLine($"<td style='padding: 8px; border: 1px solid #ddd;'>{itemTotal.ToString("C")}</td>");
                    sb.AppendLine("</tr>");
                    index++;
                }
            }

            sb.AppendLine("</tbody></table>");

            sb.AppendLine("<div style='text-align: right; margin-top: 20px;'>");
            sb.AppendLine($"<p style='font-size: 16px;'><strong>Grand Total:</strong> {total.ToString("C")}</p>");
            sb.AppendLine("</div>");

            sb.AppendLine("<p style='margin-top: 40px;'>Thank you for your purchase!</p>");

            sb.AppendLine("</div></body></html>");

            var mail = new MailMessage();
            string email = "lucasbarlach@gmail.com";
            mail.From = new MailAddress("ArnesElektronik@gmail.com", "Arnes Elektronik");
            mail.To.Add(email);
            mail.Subject = $"Receipt for Order #{order.OrderId}";
            mail.Body = sb.ToString();
            mail.IsBodyHtml = true;

            using var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("ArnesElektronik@gmail.com", "eclb mljs yant oxzn"),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mail);
        }
    }
}
