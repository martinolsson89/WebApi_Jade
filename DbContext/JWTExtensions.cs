using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;

using Configuration;

namespace DbContext;

public static class JWTExtentions
{
    public static void AddJwtTokenService(this IServiceCollection Services, IConfiguration configuration)
    {
        // Här är extension metoden för att definera och registrera en jwt token i servicen.

        //Here we tell ASP.NET Core that we are using JWT Authentication
        Services.AddAuthentication(options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // We add an scheme that our token must follow, and add it into the services
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })

            //Here we tell ASP.NET Core that it will be JWT Bearer token based
            .AddJwtBearer(options => { // Här börjar vi skapa JWT tokenet
                
                var jwtOptions = configuration.GetSection(JwtOptions.Position).Get<JwtOptions>(); // Here we dependecy inject the information from secrets that we will fill in to our token
                
                options.RequireHttpsMetadata = false; // Denna säger om vi behöver https för att validera tokenet. eller att tokenet skickas över med https
                options.SaveToken = true; // Ngn middleware sparar tokenet

                //Here we tell ASP.NET Core how to validate the JWT in the HTTP request pipeline
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters() // please explain this
                { // and now i wonder, since we added the default authentication scheme, does this downhere need to look a certain way?
                    ValidateIssuerSigningKey = jwtOptions.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtOptions.IssuerSigningKey)), // Om du vill skapa en jwt token i framtiden kolla bara definitionen, men sålänge du vet att du måste definera den på detta sätt
                    ValidateIssuer = jwtOptions.ValidateIssuer, // and here we fill in the relevant informations from the secrets in the injected class.
                    ValidIssuer = jwtOptions.ValidIssuer,
                    ValidateAudience = jwtOptions.ValidateAudience,
                    ValidAudience = jwtOptions.ValidAudience,
                    RequireExpirationTime = jwtOptions.RequireExpirationTime,
                    ValidateLifetime = jwtOptions.RequireExpirationTime,
                    ClockSkew = TimeSpan.FromDays(1),
                };
        });
        
        Services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Position)); // and here we do something with the dependecy injection again?
        Services.AddTransient<JWTService>(); // Vi lägger till servicen som en transient, vilket betyder att det skapas en ny instans varje gång den kallas.
    }
}
