namespace K.O.A.E._20240911.Auth
{
    
        // Interfaz para un servicio de autenticación JWT
        public interface IJwtAuthenticationService
        {
            // Método para autenticar al usuario y generar un token JWT
            string Authenticate(string userName);
        }
    

}
