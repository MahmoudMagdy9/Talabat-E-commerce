
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Repositories.Contract;
using Talabat.Infrastructure.Data;
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

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //allow DI for any class implement IGenericRepository
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            #endregion

            var app = builder.Build();


            //Update DB Explicitly
            // create scope and dispose it
            using var scope = app.Services.CreateScope();

            //get service provider
            var service = scope.ServiceProvider;

            //retrieve context to put it in DI container (GetRequiredService)
            var _dbconetext = service.GetRequiredService<StoreContext>();
            //to log the error
            var loggerFactory = service.GetRequiredService<ILoggerFactory>();

            try
            {

                //apply any pending migration
                await _dbconetext.Database.MigrateAsync();
                //data seeding
                await StoreContextSeed.SeedAsync(_dbconetext);
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

            app.UseAuthorization();


            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
