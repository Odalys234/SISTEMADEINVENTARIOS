namespace K.O.A.E._20240911.Endpoints
{
    // Declara una clase estática llamada "BodegaEndpoint".
    public static class BodegaEndpoint
    {
        // Crea una lista estática llamada 'bodegas' para almacenar objetos
        // de forma persistente en la aplicación.
        static List<object> bodegas = new List<object>();

        // Define un método público y estático llamado "AddBodegaEndpoints"
        // que extiende la clase "WebApplication".
        public static void AddBodegaEndpoints(this WebApplication app)
        {
            // Mapea una ruta GET "/bodega/{id}" para obtener una bodega específica por ID.
            app.MapGet("/bodega/{id}", (int id) =>
            {
                // Verifica si el ID proporcionado es válido.
                if (id < 0 || id >= bodegas.Count)
                {
                    return Results.NotFound("Bodega no encontrada");
                }

                // Devuelve la bodega específica basada en el ID.
                return Results.Ok(bodegas[id]);
            }).RequireAuthorization(); // Requiere que el usuario esté autenticado para acceder a esta ruta.

            // Mapea una ruta POST "/bodega" que acepta dos parámetros: "nombre" y "ubicacion".
            app.MapPost("/bodega", (int id, string nombre, string ubicacion) =>
            {
                bodegas.Add(new { id,nombre, ubicacion });

                // Devuelve una respuesta HTTP exitosa (200 OK) para indicar que
                // la operación se ha completado con éxito.
                return Results.Ok("Bodega creada");
            }).RequireAuthorization(); // Requiere que el usuario esté autenticado para acceder a esta ruta.

            // Mapea una ruta PUT "/bodega/{id}" para modificar una bodega existente.
            app.MapPut("/bodega/{id}", (int id, string nombre, string ubicacion) =>
            {
                // Verifica si el ID proporcionado es válido.
                if (id < 0 || id >= bodegas.Count)
                {
                    return Results.NotFound("Bodega no encontrada");
                }

                // Modifica la bodega existente en la lista 'bodegas'.
                bodegas[id] = new { nombre, ubicacion };

                // Devuelve una respuesta HTTP exitosa (200 OK) para indicar que la bodega fue modificada.
                return Results.Ok("Bodega modificada");
            }).RequireAuthorization(); // Requiere que el usuario esté autenticado para acceder a esta ruta.
        }
    }
}
