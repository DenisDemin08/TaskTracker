using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using TaskTracker.Domain.Services.Contracts;
using TaskTracker.Storage;
using TaskTracker.Storage.Repository;
using TaskTracker.Domain.Services.Contracts.Repositories;
using TaskTracker.Domain.Services.UseCases;

namespace TaskTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(builder.Configuration["Cors:AllowedOrigins"]?.Split(';')
                           ?? ["http://localhost:7056"])
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials()
                          .WithExposedHeaders("WWW-Authenticate");
                });
            });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TaskTracker API",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Support",
                        Email = "support@tasktracker.com"
                    }
                });
                c.CustomSchemaIds(type => type.FullName?.Replace("+", "."));

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddDbContext<TaskTrackerdbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAdministratorRepository, AdministratorRepository>();
            builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
            builder.Services.AddScoped<ITasksRepository, TasksRepository>();
            builder.Services.AddScoped<ITeamRepository, TeamRepository>();

            builder.Services.AddScoped<IUnitOfWork>(provider =>
            {
                var context = provider.GetRequiredService<TaskTrackerdbContext>();
                return new UnitOfWork(
                    context,
                    provider.GetRequiredService<IUserRepository>(),
                    provider.GetRequiredService<IAdministratorRepository>(),
                    provider.GetRequiredService<IManagerRepository>(),
                    provider.GetRequiredService<IEmployeeRepository>(),
                    provider.GetRequiredService<IProjectRepository>(),
                    provider.GetRequiredService<ITasksRepository>(),
                    provider.GetRequiredService<ITeamRepository>()
                );
            });

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAccessControlService, AccessControlService>();
            builder.Services.AddScoped<ICompletingTaskService, CompletingTaskService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IProjectManageService, ProjectManageService>();
            builder.Services.AddScoped<ITaskCompletionConfirmationService, TaskCompletionConfirmationService>();
            builder.Services.AddScoped<ITeamManageService, TeamManageService>();
            builder.Services.AddScoped<IViewProjectsService, ViewProjectsService>();
            builder.Services.AddScoped<IViewTaskService, ViewTaskService>();
            builder.Services.AddScoped<ICreateTaskService, CreateTaskService>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Convert.FromBase64String(jwtSettings["SecretKey"]!))
                    };
                });

            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("Administrator", policy =>
                    policy.RequireRole("Administrator"))
                .AddPolicy("Manager", policy =>
                    policy.RequireRole("Manager"));

            var app = builder.Build();

            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (UnauthorizedAccessException ex)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsJsonAsync(new { error = "Unauthorized", message = ex.Message });
                }
                catch (ArgumentException ex)
                {
                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsJsonAsync(new { error = "Not Found", message = ex.Message });
                }
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 401)
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        error = "Unauthorized",
                        message = "Authentication required"
                    }));
                }
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskTracker API v1");
                    c.RoutePrefix = "swagger";
                    c.OAuthClientId("swagger-ui");
                    c.OAuthUsePkce();
                });
            }

            app.MapControllers();
            app.MapGet("/", () => Results.Redirect("/swagger"));

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<TaskTrackerdbContext>();
                db.Database.Migrate();
            }

            app.Run();
        }
    }
}