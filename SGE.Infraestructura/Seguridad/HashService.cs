using System.Security.Cryptography;
using System.Text;
using SGE.Aplicacion.Autorizacion;

namespace SGE.Infraestructura.Seguridad
{
    public class HashService: IHashService
    {

        // hash SHA-256 a partir de una cadena de texto
        public string HashearPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("La contraseña no puede estar vacia", nameof(password));

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}