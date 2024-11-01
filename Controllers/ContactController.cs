using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ProductManagament_MVC.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace ProductManagament_MVC.Controllers;

public class ContactController : Controller
{
    private readonly PM_Context _context;

    public ContactController(PM_Context context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(string username, string email, string message)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var contact = new Contact()
                {
                    UserName = username,
                    Email = email,
                    Message = message,
                    ReceivedDate = DateTime.Now
                };

                await _context.Contacts.AddAsync(contact);
                await _context.SaveChangesAsync();
                SendEmail(contact, message, email, username);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        return View();
    }

    public void SendEmail(Contact contact, string message, string email, string username)
    {
        MimeMessage mimeMessage = new MimeMessage();
        MailboxAddress mailboxAddressFrom = new MailboxAddress("LZ.", "lzider123@gmail.com");
        mimeMessage.From.Add(mailboxAddressFrom);

        MailboxAddress mailboxAddressTo = new MailboxAddress("User", email);
        mimeMessage.To.Add(mailboxAddressTo);

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = $@"
    <html>
    <body style='font-family: Arial, sans-serif; color: #333;'>
        <div style='max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
            <h2 style='color: #4CAF50;'>Bize Ulaştığınız İçin Teşekkürler, {username}!</h2>
            <p>Değerli mesajınızı aldık ve en kısa sürede sizinle iletişime geçeceğiz. Aşağıda mesajınızın bir özeti bulunmaktadır:</p>
            
            <div style='background-color: #f9f9f9; padding: 15px; border-left: 4px solid #4CAF50; margin: 20px 0;'>
                <p><strong>Gönderilen Mesaj:</strong></p>
                <p>{message}</p>
            </div>

            <p>Eğer daha fazla sorunuz varsa, <a href='mailto:lzider123@gmail.com'>buraya tıklayarak</a> bize ulaşabilirsiniz.</p>

            <footer style='margin-top: 20px; text-align: center; color: #aaa; font-size: 12px;'>
                <p>© 2024 LZ Şirketi. Tüm hakları saklıdır.</p>
                <p><a href='https://www.sirketwebsiteniz.com' style='color: #4CAF50;'>sirketwebsiteniz.com</a></p>
            </footer>
        </div>
    </body>
    </html>";

        mimeMessage.Body = bodyBuilder.ToMessageBody();
        mimeMessage.Subject = "Mesajınız var!!";
        
        SmtpClient smtpClient = new SmtpClient();
        smtpClient.Connect("smtp.gmail.com", 587, false);
        smtpClient.Authenticate("lzider123@gmail.com", "ytiqohfqmrvbvbiv");
        smtpClient.Send(mimeMessage);
        smtpClient.Disconnect(true);
    }
}