namespace NARA20240909.Endpoints
{
    public static class CategoriaProductoEndpoint
    {
        // Lista:
        static List<object> Lista_Categorias = new List<object>();


        // Metodo Para Endpoints:
        public static void AddCategoriaProductoEndpoints(this WebApplication app)
        {

            // Endpoint GET: Retorna los registros, * ACCESO PUBLICO *
            app.MapGet("/CategoriaProducto/ObtenerTodos", () =>
            {
                return Lista_Categorias;
            }).AllowAnonymous();


            // Endpoint POST: Guarda un Registro en la lista, * ACCESO PRIVADO *
            app.MapPost("/CategoriaProducto/Registrar", (string Nombre, double Precio) =>
            {
                Lista_Categorias.Add(new { Nombre, Precio });

                return Results.Ok();
            }).RequireAuthorization();

        }

    }
}
