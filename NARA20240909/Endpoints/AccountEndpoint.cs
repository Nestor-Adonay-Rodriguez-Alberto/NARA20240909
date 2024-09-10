using NARA20240909.Auth;

namespace NARA20240909.Endpoints
{
    public static class AccountEndpoint
    {

        // Metodo:
        public static void AddAccountEndpoints(this WebApplication app)
        {
            app.MapPost("/account/login", (string login, string password, IJwtAuthenticationService authService) =>
            {

                // Valida el inicio de seccion:
                if (login == "admin" && password == "12345")
                {
                    // Autentifica al usuario y genera un token:
                    var token = authService.Authenticate(login);

                    // Respuesta http 200 con el token JWT como resultado:
                    return Results.Ok(token);
                }
                else
                {
                    // Credenciales invalidas:
                    return Results.Unauthorized();
                }

            });
        }


    }
}
