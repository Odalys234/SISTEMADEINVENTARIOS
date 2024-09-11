// Directiva para trabajar con tokens y su validación en JSON Web Tokens (JWT)
using K.O.A.E._20240911.Auth;
using Microsoft.IdentityModel.Tokens;

// Directiva para manejar la creación y manipulación de tokens JWT
using System.IdentityModel.Tokens.Jwt;

// Directiva para definir y trabajar con reclamaciones de identidad del usuario
using System.Security.Claims;

// Directiva para trabajar con codificación de texto y bytes
using System.Text;

namespace K.O.A.E._20240911.Auth
{
    public class JwtAuthenticationService : IJwtAuthenticationService
    {
        private readonly string _key;

        public JwtAuthenticationService(string key)
        {
            _key = key;
        }

        //Metodo para autenticar al usuario y generar un token JWT
        public string Authenticate(string username)
        {
            //crear un manejador de tokens JWT
            var tokenHandler = new JwtSecurityTokenHandler();

            //convertir la clave en bytes utlizando codificacion ASCII
            var tokenKey = Encoding.ASCII.GetBytes(_key);

            //Configurar la informacion del token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Definir la identidad del token con el nmbre del usuario
                Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, username)
            }),

                // establecer la fecha de vencimiento (8 horas desde ahora)
                Expires = DateTime.UtcNow.AddHours(8),

                //configurar la clave de firma y el algoritmo de firma
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey)
                , SecurityAlgorithms.HmacSha256Signature)
            };

            // Crear el token JWT
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //escribir el token como una cadena y devolverlo
            return tokenHandler.WriteToken(token);
        }

    }

}