using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Usuarios;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SGE.Infraestructura.Seguridad
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IConfiguration _configuration;

        // Inyectamos IConfiguration para poder leer el archivo appsettings.json
        public JwtProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerarToken(Usuario usuario)
        {
            // 1. Obtenemos los valores de configuración desde el appsettings.json
            var secretKey = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            // 2. Transformamos la clave secreta a bytes
            var keyBytes = Encoding.UTF8.GetBytes(secretKey!);
            var securityKey = new SymmetricSecurityKey(keyBytes);

            // 3. Definimos las credenciales de firma usando el algoritmo HMAC SHA256
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // 4. Creamos los "Claims" (la información que viajará encriptada dentro del token)
            // Aquí es donde guardamos el ID del usuario cumpliendo con la Regla de Oro
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.CorreoElectronico),
                new Claim("es_admin", usuario.EsAdministrador.ToString())
            };

            // 5. Configuramos los detalles del token (vencimiento, firma, etc.)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2), // El token expira en 2 horas
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = credentials
            };

            // 6. Generamos y escribimos el token final
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}