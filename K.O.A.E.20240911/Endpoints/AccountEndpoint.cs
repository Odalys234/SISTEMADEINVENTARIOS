// Importa el espacio de nombres "K.O.A.E._20240911.Auth"
// para poder usar sus clases y tipos.
using K.O.A.E._20240911.Auth;

namespace AutenticacionJWTMinimalAPI.Endpoints
{
    public static class AccountEndpoint
    {
        // Define un método público y estático llamado
        // "AddAccountEndpoints" que extiende la clase "WebApplication".
        public static void AddAccountEndpoints(this WebApplication app)
        {
            // Asocia una ruta "/account/login" en el método POST con una función lambda que
            // toma tres parámetros: "login", "password" y "authService".
            // Este es un ejemplo de cómo utilizar el servicio de autenticación (authService) inyectado.
            // Puedes usar authService para realizar operaciones relacionadas con la autenticación.
            // Por ejemplo, autenticar al usuario y generar un token JWT.
            app.MapPost("/account/login", (string login, string password, IJwtAuthenticationService authService) =>
            {
                // Comprueba si las credenciales de inicio de sesión
                // son válidas (en este caso, si el usuario es "admin" y la contraseña es "12345").
                if (login == "admin" && password == "12345")
                {
                    // Si las credenciales son válidas, autentica al usuario
                    // utilizando el servicio de autenticación JWT y obtiene un token.
                    var token = authService.Authenticate(login);

                    // Devuelve una respuesta HTTP OK (200) con el token JWT como resultado.
                    return Results.Ok(token);
                }
                else
                {
                    // Si las credenciales no son válidas, devuelve una respuesta Unauthorized (401).
                    return Results.Unauthorized();
                }
            });
        }

    }
}
