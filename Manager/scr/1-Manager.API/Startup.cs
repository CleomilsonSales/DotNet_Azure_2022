using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Manager.Services.Interfaces;
using Manager.Services.Services;
using Manager.Infra.Interfaces;
using Manager.Infra.Repositories;
using AutoMapper;
using Manager.API.ViewModels;
using Manager.Services.DTO;
using Manager.Domain.Entities;
using Manager.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text; //para o encoding
using Manager.API.Token;

namespace Manager.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            
            #region Jwt

            var secretKey = Configuration["Jwt:Key"];

            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters{
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            #endregion
            
            
            #region DI_AutoMapper
            //#region é so pra organização, onde vc pode minizar o trecho do codigo

            var autoMapperConfig = new MapperConfiguration(cfg =>{
                cfg.CreateMap<User, UserDTO>().ReverseMap();
                cfg.CreateMap<CreateUserViewModel, UserDTO>().ReverseMap();
                cfg.CreateMap<UpdateUserViewModel, UserDTO>().ReverseMap();
            });

            services.AddSingleton(autoMapperConfig.CreateMapper());
            
            services.AddSingleton(d => Configuration);
            services.AddDbContext<ManagerContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:USER_MANAGER"]), ServiceLifetime.Transient);
            //AddScoped - uma instancia unica por requisição
            //AddTransient - uma instancia por dependencia de construtores
            //AddSingleton - uma instancia por todo o ciclo da aplicação
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();

            #endregion

            #region Swagger

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo{
                    Title = "Manager API",
                    Version = "v1",
                    Description = "API constuindo no curso de API Rest com .Net 5, Azure 2022 e SQL Server 2019",
                    Contact = new OpenApiContact{
                        Name = "Cleomilson Sales",
                        Email = "cleomilsonsales@hotmail.com",
                        Url = new Uri("https://github.com/cleomilsonsales")
                    },
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme{
                    In = ParameterLocation.Header,
                    Description = "Utilize o Bearer <Token>",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {{
                    new OpenApiSecurityScheme{
                        Reference = new OpenApiReference{
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
                });
            });

            #endregion
        


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Manager.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
