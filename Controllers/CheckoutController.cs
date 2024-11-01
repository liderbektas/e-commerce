using MailKit.Net.Smtp;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using ProductManagament_MVC.Models;

namespace ProductManagament_MVC.Controllers;

public class CheckoutController : Controller
{
    private readonly PM_Context _context;

    public CheckoutController(PM_Context context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        var user = _context.Users.Find(int.Parse(userId));

        var cart = await _context.Carts
            .Include(x => x.CartItems)
            .ThenInclude(o => o.Products)
            .FirstOrDefaultAsync(x => x.UserId == int.Parse(userId));

        if (cart == null || !cart.CartItems.Any())
        {
            return BadRequest("Sepetiniz Boş");
        }

        var orderViewModel = new OrderViewModel()
        {
            User = user,
            Cart = cart,
        };

        return View(orderViewModel);
    }


    [HttpPost]
    public async Task<IActionResult> Index(string address, string paymentMethod, string cardNumber,
        string cardName, string lastDate, string cvv)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(cardNumber))
                {
                    ModelState.AddModelError("", "Adres ve kart numarası boş olamaz.");
                    return View("Error");
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                var cart = await _context.Carts
                    .Include(x => x.CartItems)
                    .ThenInclude(o => o.Products)
                    .FirstOrDefaultAsync(x => x.UserId == int.Parse(userId));

                if (cart == null || !cart.CartItems.Any())
                {
                    return BadRequest("Sepetiniz Boş");
                }


                foreach (var cartItem in cart.CartItems)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == cartItem.ProductId);
                    if (product == null)
                    {
                        ModelState.AddModelError("", $"Ürün bulunamadı: {cartItem.ProductId}");
                        return View("Error");
                    }

                    if (product.Stock < cartItem.Quantity)
                    {
                        ModelState.AddModelError("", $"Yetersiz stok: {product.Name} (Stok: {product.Stock})");
                        return View("Error");
                    }

                    product.Stock -= cartItem.Quantity;
                }


                var order = new Order
                {
                    UserId = int.Parse(userId),
                    OrderDate = DateTime.Now,
                    address = address,
                    PaymentMethod = paymentMethod,
                    CardNumber = cardNumber,
                    CardName = cardName,
                    LastDate = lastDate,
                    CVV = cvv,
                    OrderItems = cart.CartItems.Select(x => new OrderItem
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        Products = x.Products
                    }).ToList()
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();


                cart.CartItems.Clear();
                await _context.SaveChangesAsync();
                SendEmail(order);
                return RedirectToAction("Index", "Success");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Sipariş Alınamadı: " + e.Message);
            }
        }

        return View("Error");
    }

    public void SendEmail(Order order)
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        
        MimeMessage mimeMessage = new MimeMessage();
        MailboxAddress mailboxAddressFrom = new MailboxAddress("LZ.", "lzider123@gmail.com");
        mimeMessage.From.Add(mailboxAddressFrom);

        MailboxAddress mailboxAddressTo = new MailboxAddress("User", userEmail);
        mimeMessage.To.Add(mailboxAddressTo);

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = $@"
        <html>
            <body style='font-family: Arial, sans-serif;'>
                <h2 style='color: #4CAF50;'>Siparişiniz Başarıyla Alınmıştır!</h2>
                <p>Merhaba, {userEmail}</p>
                <p>Siparişinizdeki ürünlerin detayları aşağıda listelenmiştir:</p>
                
                <table style='width: 100%; border-collapse: collapse;'>
                    <thead>
                        <tr>
                            <th style='border: 1px solid #dddddd; padding: 8px;'>Ürün</th>
                            <th style='border: 1px solid #dddddd; padding: 8px;'>Adet</th>
                            <th style='border: 1px solid #dddddd; padding: 8px;'>Fiyat</th>
                        </tr>
                    </thead>
                    <tbody>
                        {string.Join("", order.OrderItems.Select(item => $@"
                            <tr>
                                <td style='border: 1px solid #dddddd; padding: 8px;'>{item.Products.Name}</td>
                                <td style='border: 1px solid #dddddd; padding: 8px;'>{item.Quantity}</td>
                                <td style='border: 1px solid #dddddd; padding: 8px;'>{item.Products.Price:C}</td>
                            </tr>"))}
                    </tbody>
                </table>
                
                <p><strong>Toplam Tutar:</strong> {order.OrderItems.Sum(item => item.Products.Price * item.Quantity):C}</p>
                <p>Siparişiniz en kısa sürede işleme alınacaktır. Bizi tercih ettiğiniz için teşekkür ederiz!</p>
                
                <hr />
                <p style='font-size: small; color: gray;'>Bu e-posta otomatik olarak gönderilmiştir, lütfen yanıtlamayın.</p>
            </body>
        </html>";

        mimeMessage.Body = bodyBuilder.ToMessageBody();
        mimeMessage.Subject = "Sipariş Onayı - Ürün Siparişiniz Alınmıştır";

        mimeMessage.Body = bodyBuilder.ToMessageBody();
        mimeMessage.Subject = "Siparişiniz başarıyla alınmıştır";

        SmtpClient smtpClient = new SmtpClient();
        smtpClient.Connect("smtp.gmail.com", 587, false);
        smtpClient.Authenticate("lzider123@gmail.com", "ytiqohfqmrvbvbiv");
        smtpClient.Send(mimeMessage);
        smtpClient.Disconnect(true);
    }
}