using System;
using System.IO;
using System.Text;
namespace LectorTagMP3
{
    // Clase principal del programa
    class Program
    {
        // Función principal que se ejecuta al iniciar el programa
        static void Main()
        {
            Console.WriteLine("Ingrese la ruta del archivo MP3:"); // Solicita al usuario la ruta del archivo MP3
            string ruta = Console.ReadLine(); // Lee la ruta ingresada

            // Verifica si el archivo existe en el sistema
            if (!File.Exists(ruta))
            {
                Console.WriteLine("El archivo no existe."); // Mensaje si no se encuentra el archivo
                return; // Finaliza la ejecución del programa
            }

            // Abrimos el archivo en modo lectura binaria
            using FileStream fs = new FileStream(ruta, FileMode.Open, FileAccess.Read);
            // FileStream permite leer bytes directamente del archivo

            // Verifica que el archivo tenga al menos 128 bytes para contener un tag ID3v1
            if (fs.Length < 128)
            {
                Console.WriteLine("El archivo es demasiado pequeño para contener un tag ID3v1.");
                return;
            }

            // Nos posicionamos exactamente 128 bytes antes del final del archivo
            fs.Seek(-128, SeekOrigin.End);
            // FileStream.Seek(offset, origen):
            // - offset: cuántos bytes mover el puntero
            // - origen: desde dónde (en este caso, desde el final del archivo)

            byte[] tagBytes = new byte[128]; // Creamos un arreglo para guardar los 128 bytes del tag
            fs.Read(tagBytes, 0, 128); // Leemos los 128 bytes desde la posición actual

            // Convertimos los primeros 3 bytes a texto para verificar si comienza con "TAG"
            string cabecera = Encoding.ASCII.GetString(tagBytes, 0, 3);
            if (cabecera != "TAG")
            {
                Console.WriteLine("El archivo no contiene un tag ID3v1 válido.");
                return;
            }

            // Creamos una instancia de la clase Id3v1Tag para almacenar los datos del tag
            var tag = new Id3v1Tag
            {
                // Extraemos cada campo según su offset y longitud
                Titulo = Encoding.ASCII.GetString(tagBytes, 3, 30).TrimEnd('\0'),
                Artista = Encoding.ASCII.GetString(tagBytes, 33, 30).TrimEnd('\0'),
                Album = Encoding.ASCII.GetString(tagBytes, 63, 30).TrimEnd('\0'),
                Anio = Encoding.ASCII.GetString(tagBytes, 93, 4).TrimEnd('\0'),
                Comentario = Encoding.ASCII.GetString(tagBytes, 97, 30).TrimEnd('\0'),
                Genero = tagBytes[127] // El género es un solo byte
            };

            // Mostramos en consola los datos relevantes
            Console.WriteLine("\nInformación del archivo MP3:");
            Console.WriteLine($"Título: {tag.Titulo}");
            Console.WriteLine($"Artista: {tag.Artista}");
            Console.WriteLine($"Álbum: {tag.Album}");
            Console.WriteLine($"Año: {tag.Anio}");
        }
    }
}

//Codigo de prueba con la creacion del mp3 para que se pueda probar
// using System;
// using System.IO;
// using System.Text;

// namespace LectorTagMP3
// {
//     /// <summary>
//     /// Clase principal del programa.
//     /// Genera un archivo MP3 de prueba y luego lee su información ID3v1.
//     /// </summary>
//     class Program
//     {
//         /// <summary>
//         /// Función principal que se ejecuta al iniciar el programa.
//         /// </summary>
//         static void Main()
//         {
//             string ruta = "sample.mp3";

//             // Generamos un archivo MP3 falso con tag ID3v1 (sobrescribe si existe)
//             GeneradorMP3.GenerarArchivo(ruta);

//             // Abrimos el archivo en modo lectura binaria
//             using FileStream fs = new FileStream(ruta, FileMode.Open, FileAccess.Read);

//             // Verificamos que el archivo tenga al menos 128 bytes para contener un tag ID3v1
//             if (fs.Length < 128)
//             {
//                 Console.WriteLine("El archivo es demasiado pequeño para contener un tag ID3v1.");
//                 return;
//             }

//             // Posicionamos el puntero 128 bytes antes del final
//             fs.Seek(-128, SeekOrigin.End);

//             // Leemos los 128 bytes correspondientes al tag
//             byte[] tagBytes = new byte[128];
//             fs.Read(tagBytes, 0, 128);

//             // Verificamos si comienza con "TAG"
//             string cabecera = Encoding.ASCII.GetString(tagBytes, 0, 3);
//             if (cabecera != "TAG")
//             {
//                 Console.WriteLine("El archivo no contiene un tag ID3v1 válido.");
//                 return;
//             }

//             // Creamos un objeto para almacenar la información del tag
//             var tag = new Id3v1Tag
//             {
//                 Titulo = Encoding.ASCII.GetString(tagBytes, 3, 30).TrimEnd('\0'),
//                 Artista = Encoding.ASCII.GetString(tagBytes, 33, 30).TrimEnd('\0'),
//                 Album = Encoding.ASCII.GetString(tagBytes, 63, 30).TrimEnd('\0'),
//                 Anio = Encoding.ASCII.GetString(tagBytes, 93, 4).TrimEnd('\0'),
//                 Comentario = Encoding.ASCII.GetString(tagBytes, 97, 30).TrimEnd('\0'),
//                 Genero = tagBytes[127]
//             };

//             // Mostramos en consola los datos del tag ID3v1
//             Console.WriteLine("\nInformación del archivo MP3:");
//             Console.WriteLine($"Título: {tag.Titulo}");
//             Console.WriteLine($"Artista: {tag.Artista}");
//             Console.WriteLine($"Álbum: {tag.Album}");
//             Console.WriteLine($"Año: {tag.Anio}");
//             Console.WriteLine($"Comentario: {tag.Comentario}");
//             Console.WriteLine($"Género (código): {tag.Genero}");
//         }
//     }
// }


//Generar MP3 FALSO codigo
// using System;
// using System.IO;
// using System.Text;

// /// <summary>
// /// Esta clase permite generar un archivo MP3 ficticio con un tag ID3v1 válido al final,
// /// para poder probar el lector de metadatos ID3v1.
// /// </summary>
// public class GeneradorMP3
// {
//     /// <summary>
//     /// Genera un archivo MP3 con contenido simulado y un tag ID3v1 al final.
//     /// </summary>
//     /// <param name="nombreArchivo">Nombre del archivo a generar (por ejemplo: "sample.mp3")</param>
//     public static void GenerarArchivo(string nombreArchivo)
//     {
//         using (FileStream fs = new FileStream(nombreArchivo, FileMode.Create))
//         {
//             // Escribimos 5000 bytes vacíos simulando contenido MP3
//             byte[] datosFalsos = new byte[5000];
//             fs.Write(datosFalsos, 0, datosFalsos.Length);

//             // Creamos el bloque de 128 bytes del Tag ID3v1
//             byte[] tag = new byte[128];

//             // Escribimos los campos del tag ID3v1 (ver offset + longitud en tabla)
//             Encoding.ASCII.GetBytes("TAG").CopyTo(tag, 0);                    // Header: 3 bytes
//             Encoding.ASCII.GetBytes("Canción de prueba").CopyTo(tag, 3);     // Título: 30 bytes
//             Encoding.ASCII.GetBytes("Artista Genérico").CopyTo(tag, 33);     // Artista: 30 bytes
//             Encoding.ASCII.GetBytes("Álbum Demo").CopyTo(tag, 63);           // Álbum: 30 bytes
//             Encoding.ASCII.GetBytes("2025").CopyTo(tag, 93);                 // Año: 4 bytes
//             Encoding.ASCII.GetBytes("Solo un comentario").CopyTo(tag, 97);   // Comentario: 30 bytes
//             tag[127] = 13; // Género: 1 byte (13 = Pop)

//             // Escribimos el tag completo al final del archivo
//             fs.Write(tag, 0, 128);
//         }

//         Console.WriteLine($"Archivo {nombreArchivo} generado con éxito.");
//     }
// }
