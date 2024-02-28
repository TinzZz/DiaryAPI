using Diary.Application.Mapping;
using Diary.Application.Services;
using Diary.Application.Validations;
using Diary.Application.Validations.FluentValidations.Report;
using Diary.Domain.Dto.Report;
using Diary.Domain.Interfaces.Services;
using Diary.Domain.Interfaces.Validations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ReportMapping));
            InitServices(services);
        }

        private static void InitServices(this IServiceCollection services)
        {
            services.AddScoped<IReportValidator, ReportValidator>();
            services.AddScoped<IValidator<CreateReportDTO>, CreateReportValidator>();
            services.AddScoped<IValidator<UpdateReportDTO>, UpdateReportValidator>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}