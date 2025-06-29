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
