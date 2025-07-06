
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace QuestIA.App.Extensions
{
    public static class ControllerExtensions
    {
        public static Guid GetUserId(this ControllerBase controller)
        {
            // tenta ler o claim NameIdentifier
            var id = controller.User.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? throw new UnauthorizedAccessException("Token sem claim de usuário");
            return Guid.Parse(id);
        }
    }
}
