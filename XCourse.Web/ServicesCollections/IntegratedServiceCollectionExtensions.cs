using Microsoft.AspNetCore.Identity.UI.Services;
using XCourse.Services.Implementations.EmailServices;
using XCourse.Services.Implementations.PaymentService;
using XCourse.Services.Interfaces.PaymentService;

namespace XCourse.Web.ServicesCollections
{
    public static class IntegratedServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegratedServices(this IServiceCollection services)
        {
            services.AddScoped<IEmailSender, GmailSender>();
            services.AddScoped<IStripeService,StripeService>();
            services.AddScoped<IWalletService,WalletService>();
            services.AddScoped<ITransactionService, TransactionService>();

            return services;
        }

    }
}
