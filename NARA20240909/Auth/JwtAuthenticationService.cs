using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NARA20240909.Auth
{
    public class JwtAuthenticationService : IJwtAuthenticationService
    {
        // ATRIBUTOS:
        private readonly string _Key;

        // CONSTRUCTOR:
        public JwtAuthenticationService(string key)
        {
            _Key = key;
        }

        // Implementacion De La Interfaz: Generará un JWT:
        public string Authenticate(string userName)
        {

            // Crea un manejador de tokens JWT:
            var tokenHandler = new JwtSecurityTokenHandler();

            // Clave a Bytes en codificacion ASCII:
            var tokenKey = Encoding.ASCII.GetBytes(_Key);


            // Configuracion De La Informacion Del Token:
            var tokenDescriptor = new SecurityTokenDescriptor
            {

                // Identidad Del Token con el Nombre del usuario:
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName)
                }),


                // Fecha De Vencimiento (8 horas):
                Expires = DateTime.UtcNow.AddHours(8),


                // Configuracion de la Clave de firma y el algoritmo de firma:
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)

            };


            // Creacion del Token JWT:
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Escribe el Token como una cadena y la devuelve:
            return tokenHandler.WriteToken(token);
        }


    }
}
