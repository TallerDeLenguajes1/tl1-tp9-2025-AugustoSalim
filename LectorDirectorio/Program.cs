// Namespace general del proyecto
namespace LectorDirectorioApp
{
    // Clase principal del programa
    class Program
    {
        static void Main()
        {
            string ruta;

            // Bucle que se repite hasta que el usuario ingrese una ruta de directorio válida
            do
            {
                Console.WriteLine("Ingrese la ruta del directorio a analizar:");
                ruta = Console.ReadLine();

                // Si la ruta no existe, se notifica
                if (!Directory.Exists(ruta))
                {
                    Console.WriteLine("El directorio ingresado no existe. Intente nuevamente.\n");
                }
                else
                {
                    break; // Si es válido, salimos del bucle
                }

            } while (true);

            // Mostrar las carpetas dentro del directorio
            Console.WriteLine("\nCarpetas encontradas:");
            string[] carpetas = Directory.GetDirectories(ruta);
            foreach (string carpeta in carpetas)
            {
                Console.WriteLine("- " + Path.GetFileName(carpeta)); // Solo el nombre
            }

            // Mostrar los archivos dentro del directorio
            Console.WriteLine("\nArchivos encontrados:");
            string[] archivos = Directory.GetFiles(ruta);
            List<string> lineasCSV = new();
            lineasCSV.Add("Nombre del Archivo,Tamaño (KB),Fecha de Ultima Modificacion"); // Encabezado del CSV

            foreach (string archivo in archivos)
            {
                FileInfo info = new(archivo);
                double tamanioKB = Math.Round(info.Length / 1024.0, 2); // Tamaño redondeado a dos decimales
                string fecha = info.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");

                // Mostrar en consola
                Console.WriteLine("- " + info.Name + " (" + tamanioKB + " KB)");

                // Agregar línea al CSV
                lineasCSV.Add($"{info.Name},{tamanioKB},{fecha}");
            }

            // Ruta donde se guardará el archivo CSV (en el mismo directorio analizado)
            string rutaCSV = Path.Combine(ruta, "reporte_archivos.csv");
            File.WriteAllLines(rutaCSV, lineasCSV);

            Console.WriteLine($"\nReporte CSV generado correctamente en: {rutaCSV}");
        }
    }
}
