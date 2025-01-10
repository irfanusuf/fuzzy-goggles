using System;

namespace WebApplication1.Interfaces;

public interface IMailService
{
Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
}
