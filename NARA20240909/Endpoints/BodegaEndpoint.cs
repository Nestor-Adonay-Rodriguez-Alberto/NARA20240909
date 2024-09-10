using Microsoft.AspNetCore.Mvc;
using NARA20240909.Models;

namespace NARA20240909.Endpoints
{
    public static class BodegaEndpoint
    {
        // Lista:
        static List<Bodega> Lista_Bodegas = new List<Bodega>();


        // Metodo Para Endpoints:
        public static void AddBodegaEndpoints(this WebApplication app)
        {

            // Endpoint GET: Retorna un registro encontrado, * ACCESO PUBLICO *
            app.MapGet("/Bodega/ObtenerPorId", (int id) =>
            {
                Bodega Objeto_Obtenido = Lista_Bodegas.FirstOrDefault(x => x.IdBodega == id);

                return Objeto_Obtenido;
            }).RequireAuthorization();


            // Endpoint POST: Guarda un Registro en la lista, * ACCESO PRIVADO *
            app.MapPost("/Bodega/Crear", ([FromBody] Bodega bodega) =>
            {
                Lista_Bodegas.Add(bodega);

                return Results.Ok();
            }).RequireAuthorization();


            // Endpoint GET: Retorna un registro encontrado, * ACCESO PUBLICO *
            app.MapPut("/Bodega/Modificar", (int id, [FromBody] Bodega bodega) =>
            {
                Bodega Objeto_Obtenido = Lista_Bodegas.FirstOrDefault(x => x.IdBodega == id);

                if(Objeto_Obtenido!=null)
                {
                    Objeto_Obtenido.IdBodega = bodega.IdBodega;
                    Objeto_Obtenido.Nombre = bodega.Nombre;
                    Objeto_Obtenido.Tipos_Productos = bodega.Tipos_Productos;

                    return Results.Ok();
                }
                else
                {
                    return Results.NotFound();
                }
            }).RequireAuthorization();

        }

    }
}
