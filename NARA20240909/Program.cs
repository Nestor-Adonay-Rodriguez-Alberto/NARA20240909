using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NARA20240909.Auth;
using NARA20240909.Endpoints;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Explora los Edpoints de la API
builder.Services.AddEndpointsApiExplorer();

// CONFIGURA Swagger: * agregada *
builder.Services.AddSwaggerGen(c =>
{

    //Informacion de la API en Swagger:
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT API", Version = "v1" });

    // Seguridad para JWT en Swagger:
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Ingresar tu token de JWT Authentication",

        // Referencia al esquema de seguridad anterior:
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }

    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    // Requisito de seguridad para JWT Swagger:
    c.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSecurityScheme, Array.Empty<string>() } });

});


// POLITICAS DE AUTORIZACION: * agregada *
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("LoggedInPolicy", policy =>
    {
        // Requiere que el usuari este autorizado para recursos autorizados:
        policy.RequireAuthenticatedUser();
    });

});


// CLAVE PARA FIRMAR Y VERIFICAR TOKENS JWR: * agregada *
var Key = "Key.WED_API_JWT_2024.API";


// AUTHENTICACION CON JWT: * agregada *
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
// CONFIGURA AUTENTICACION JWT ESQUEMA JwtBearer:
.AddJwtBearer(x =>
{
    // Se requiere metadata HTTPS al validar el TOKEN:
    x.RequireHttpsMetadata = false;

    // Guardar el token recibido del cliente:
    x.SaveToken = true;

    // Configura los parametros de validacion del Token JWT
    x.TokenValidationParameters = new TokenValidationParameters
    {
        // Establece la clave codificada en ASCII:
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key)),

        ValidateAudience = false,

        // Validar la firma del token utilizando la clave especificada:
        ValidateIssuerSigningKey = true,

        ValidateIssuer = false
    };

});


// * agregada *
// Agrega una Intancia unica del servicio de authenticacion JWT al Inyector para obtenerla al generar el token
builder.Services.AddSingleton<IJwtAuthenticationService>(new JwtAuthenticationService(Key));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Endpoints De La Aplicacion: * agregada *
app.AddAccountEndpoints();
app.AddCategoriaProductoEndpoints();
app.AddBodegaEndpoints();

app.UseHttpsRedirection();

// Habiilitar autenticacion y autorizacion:
app.UseAuthentication();
app.UseAuthorization();

app.Run();

