using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MvcCore.Helpers
{
    public class CypherService
    {


        public static String EncriptarTextoBasico(String contenido)
        {
            byte[] entrada;
            byte[] salida;

            //conversor
            UnicodeEncoding encoding = new UnicodeEncoding();

            //cifrador
            SHA1Managed sha = new SHA1Managed();
            entrada = encoding.GetBytes(contenido);
            salida = sha.ComputeHash(entrada);
            String res = encoding.GetString(salida);
            return res;
        }

        public static String GenerateSalt()
        {
            Random random = new Random();
            String salt = "";
            for (int i = 0; i < 50; i++)
            {
                int aleat = random.Next(0, 255);
                char letra = Convert.ToChar(aleat);
                salt += letra;
            }
            return salt;
        }

        public static byte[] CifrarContenido(String password, String salt)
        {

            String contenidosalt = password + salt;
            SHA256Managed sha = new SHA256Managed();
            byte[] salida;
            salida = Encoding.UTF8.GetBytes(contenidosalt);
            for (int i = 0; i < 70; i++)
            {
                salida = sha.ComputeHash(salida);
            }
            sha.Clear();
            return salida;

        }
    }
}
