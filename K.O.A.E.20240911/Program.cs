// Importar el espacio de nombres donde se encuentra la interfaz IJwtAuthenticationService
using K.O.A.E._20240911;

// Importar el espacio de nombres que contiene la definición de los endpoints de la aplicación
using AutenticacionJWTMinimalAPI.Endpoints;
using K.O.A.E._20240911.Auth;
using K.O.A.E._20240911.Endpoints;



// Importar el espacio de nombres necesario para configurar la autenticación basada en tokens JWT
using Microsoft.AspNetCore.Authentication.JwtBearer;

// Importar el espacio de nombres para trabajar con tokens de seguridad
using Microsoft.IdentityModel.Tokens;

// Importar el espacio de nombres para definir la documentación de la API con Swagger
using Microsoft.OpenApi.Models;



// Importar el espacio de nombres para trabajar con codificación de texto y bytes
using System.Text;

// Crear un objeto "builder" para configurar la aplicación
var builder = WebApplication.CreateBuilder(args);

// Agregar un servicio para permitir la exploración de API Endpoints
builder.Services.AddEndpointsApiExplorer();

// Configurar Swagger para documentar la API
builder.Services.AddSwaggerGen(c =>
{
    // Definir información básica de la API en Swagger
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT API", Version = "v1" });

    // Configurar un esquema de seguridad para JWT en Swagger
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Ingresar tu token de JWT Authentication",

        // Hacer referencia al esquema de seguridad JWT definido anteriormente
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    // Agregar el esquema de seguridad JWT a la documentación
    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    // Agregar un requisito de seguridad para JWT en Swagger
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

// Configurar políticas de autorización
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("LoggedInPolicy", policy =>
    {
        // Requerir que el usuario esté autenticado para acceder a recursos protegidos
        policy.RequireAuthenticatedUser();
    });
});

// Definir la clave secreta utilizada para firmar y verificar tokens JWT
// Esta clave se puede modificar. Lo ideal sería una clave diferente para cada proyecto
var key = "Key.JWTAPIMinimal2023.API";

// Configurar la autenticación con JWT
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Configurar la autenticación JWT utilizando el esquema JwtBearer
.AddJwtBearer(x =>
{
    // Indicar si se requiere metadata HTTPS al validar el token.
    // En un entorno de desarrollo, generalmente se establece en false.
    x.RequireHttpsMetadata = false;

    // Indicar que se debe guardar el token JWT recibido del cliente.
    x.SaveToken = true;


    // Configurar los parámetros de validación del token JWT
    x.TokenValidationParameters = new TokenValidationParameters
    {
        // Establecer la clave secreta utilizada para firmar y verificar el token.
        // En este caso, la clave es una cadena codificada en ASCII.
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),

        // Indicar si se debe validar el "audience" (audiencia) del token.
        // En este caso, se establece en false, lo que significa que no se valida la audiencia.
        ValidateAudience = false,

        // Indicar si se debe validar la firma del token utilizando la clave especificada.
        // En este caso, se establece en true para validar la firma.
        ValidateIssuerSigningKey = true,

        // Indicar si se debe validar el "issuer" (emisor) del token.
        // En este caso, se establece en false, lo que significa que no se valida el emisor.
        ValidateIssuer = false
    };
});

// Agregar una instancia única del servicio de autenticación JWT
// al inyector para poder obtenerla al momento de generar el token
builder.Services.AddSingleton<IJwtAuthenticationService>(new JwtAuthenticationService(key));

// Construir la aplicación
var app = builder.Build();

// Configurar Swagger en el entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Agregar los endpoints de la aplicación
app.AddAccountEndpoints();
app.AddBodegaEndpoints();
app.AddCategoriaProductoEndpoints();

// Configurar redirección HTTPS
app.UseHttpsRedirection();

// Habilitar la autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Ejecutar la aplicación
app.Run();
