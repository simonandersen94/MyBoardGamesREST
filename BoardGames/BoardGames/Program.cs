using BoardGames.BusinessLogic.Interfaces;
using BoardGames.BusinessLogic;
using BoardGames.Data;
using BoardGames.Data.Interfaces;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace BoardGames {
    public class Program {
        public static void Main(string[] args) {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddTransient<IGameControl, GameControl>();
            builder.Services.AddTransient<IGameAccess, GameAccess>();

            builder.Services.AddTransient<IVersionControl, VersionControl>();
            builder.Services.AddTransient<IVersionAccess, VersionAccess>();

            builder.Services.AddTransient<ICharacterControl, CharacterControl>();
            builder.Services.AddTransient<ICharacterAccess, CharacterAccess>();

            builder.Services.AddRouting(options => options.LowercaseUrls = true); //Makes the route lower case

            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI(options => {
                    options.DocExpansion(DocExpansion.None);
                    options.EnableFilter();
                    options.DisplayRequestDuration();
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}