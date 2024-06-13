using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using Talabat.API.Extensions;
using Talabat.API.Helpers;
using Talabat.Application;
using Talabat.Application.AuthService;
using Talabat.Core;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Infrastructure;
using Talabat.Infrastructure.Data;
using Talabat.Infrastructure.Identity;
using Talabat.Infrastructure.Repository;

namespace Talabat.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services
            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            } );
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                //option.Password.RequiredUniqueChars = 2;
                //option.Password.RequireDigit = true;
                //option.Password.RequireLowercase = true;
                //option.Password.RequireUppercase = true;
                //option.Password.RequiredLength = 6;

            }).AddEntityFrameworkStores<ApplicationIdentityDbContext>();

            
            builder.Services.AddAuthServices(builder.Configuration);//JWT Authentication

            builder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });



            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(builder.Configuration["FrontBaseUrl"]);
                });
            });




            builder.Services.AddScoped(typeof(IPaymentService), typeof(PaymentService));

            builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));

            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            builder.Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            builder.Services.AddScoped(typeof(IOrderService), typeof(OrderService));

            builder.Services.AddAutoMapper(typeof(MappingProfiles));

            #endregion

            var app = builder.Build();


            //Update DB Explicitly
            // create scope and dispose it
            using var scope = app.Services.CreateScope();

            //get service provider
            var service = scope.ServiceProvider;

            //retrieve context to put it in DI container (GetRequiredService)
            var dbconetext = service.GetRequiredService<StoreContext>();
            var identityDbContext = service.GetRequiredService<ApplicationIdentityDbContext>();
            //for data seeding
            var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
            await ApplicationIdentityDataSeed.SeedUserAsync(userManager);
            //to log the error
            var loggerFactory = service.GetRequiredService<ILoggerFactory>();

            try
            {

                //apply any pending migration (Update DB)
                await dbconetext.Database.MigrateAsync();
                //data seeding
                await StoreContextSeed.SeedAsync(dbconetext);

                await identityDbContext.Database.MigrateAsync();
            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(e,"an error has been occurred during appealing migration");
            }
                

            #region Configure Kestrel Middlewares

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("MyPolicy");

            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseMiddleware<ProfilingMiddleware>();
            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
