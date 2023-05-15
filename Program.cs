using FlightBooking.Database;
using FlightBooking.Utils.JsonConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
    o.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<FlightDbContext>(o => o.UseInMemoryDatabase(databaseName: "FlightBooking"));
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<FlightRepository>();
builder.Services.AddScoped<BookingRepository>();

builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "FlightBookingSwaggerAnnotation", Version = "V1" });
    o.IncludeXmlComments(Path.Combine(System.AppContext.BaseDirectory, "FlightBooking.xml"));
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter token 'bearer' [space] <token>"
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin",
         policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireUser",
         policy => policy.RequireRole("User"));
});

//Adding Bearer Configuration
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    var Key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JWT:Issuer"],//Validate the server that generates the token
        ValidAudience = builder.Configuration["JWT:Audience"],//Validate the recipient of the tpken is authorized to recive
        IssuerSigningKey = new SymmetricSecurityKey(Key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetService<FlightDbContext>();

DBInitializer.InIt(dbContext);

app.Run();

