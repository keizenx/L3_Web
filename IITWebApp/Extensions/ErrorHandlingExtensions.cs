using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IITWebApp.Extensions
{
    public static class ErrorHandlingExtensions
    {
        public static IActionResult HandleDbError(this Controller controller, Exception ex, ILogger logger, string action = "Index")
        {
            logger.LogError(ex, "Erreur de base de données dans {Controller}", controller.GetType().Name);
            
            // En développement, afficher l'erreur complète
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                controller.TempData["ErrorMessage"] = $"Erreur de base de données: {ex.Message}";
                if (ex.InnerException != null)
                {
                    controller.TempData["ErrorDetails"] = $"Détail: {ex.InnerException.Message}";
                }
            }
            else
            {
                controller.TempData["ErrorMessage"] = "Une erreur s'est produite lors de l'opération sur la base de données.";
            }
            
            return controller.RedirectToAction(action);
        }

        public static IActionResult HandleValidationError(this Controller controller, string message)
        {
            controller.TempData["ValidationError"] = message;
            return controller.View();
        }
    }
}