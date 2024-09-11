 namespace K.O.A.E._20240911.Auth
{
    public static class CategoriaProductoEndpoint
    {
        // Lista de categorías en memoria
        static List<object> categorias = new List<object>();

        public static void AddCategoriaProductoEndpoints(this WebApplication app)
        {
            // Endpoint público para obtener todas las categorías
            app.MapGet("/categoria/obtenertodos", () =>
            {
                return Results.Ok(categorias);
            }).AllowAnonymous(); // Permitir acceso sin autenticación

            // Endpoint privado para registrar una nueva categoría
            app.MapPost("/categoria/registrar", (string nombre, string descripcion) =>
            {
                categorias.Add(new {nombre,descripcion});
                return Results.Ok("Categoría registrada");
            }).RequireAuthorization(); // Solo usuarios autenticados pueden acceder
        }
    }
}

