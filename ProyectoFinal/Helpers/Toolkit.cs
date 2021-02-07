using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal.Helpers
{
    public class Toolkit
    {
        public static bool CompararArrayBytes(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
            {
                if (!a[i].Equals(b[i])) return false;

            }
            return true;
        }
        public static String FilenameNormalizer(String filename)
        {
            String ending = '.' + filename.Split('.').Last();
            String cadena = "";
            for (int i = 0; i < filename.LastIndexOf('.'); i++)
            {
                if (Char.IsDigit(filename[i]) || Char.IsLetter(filename[i])) cadena += filename[i];
            }

            cadena += ending;
            return cadena;
        }
    }
}
