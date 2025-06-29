using System;
using System.IO;
using System.Text;

/// <summary>
/// Esta clase genera un archivo MP3 ficticio con un tag ID3v1 válido al final
/// para poder probar el lector.
/// </summary>
class GeneradorMP3
{
    static void Main()
    {
        string nombreArchivo = "sample.mp3";

        using (FileStream fs = new FileStream(nombreArchivo, FileMode.Create))
        {
            // Escribimos 5000 bytes vacíos simulando contenido MP3
            byte[] datosFalsos = new byte[5000];
            fs.Write(datosFalsos, 0, datosFalsos.Length);

            // Creamos el bloque de 128 bytes del Tag ID3v1
            byte[] tag = new byte[128];
            Encoding.ASCII.GetBytes("TAG").CopyTo(tag, 0);            // Header
            Encoding.ASCII.GetBytes("Canción de prueba").CopyTo(tag, 3); // Título (30)
            Encoding.ASCII.GetBytes("Artista Genérico").CopyTo(tag, 33); // Artista (30)
            Encoding.ASCII.GetBytes("Álbum Demo").CopyTo(tag, 63);       // Álbum (30)
            Encoding.ASCII.GetBytes("2025").CopyTo(tag, 93);             // Año (4)
            Encoding.ASCII.GetBytes("Solo un comentario").CopyTo(tag, 97); // Comentario (30)
            tag[127] = 13; // Género: 13 = Pop

            fs.Write(tag, 0, 128); // Escribimos el tag al final
        }

        Console.WriteLine("Archivo sample.mp3 generado con éxito.");
    }
}
