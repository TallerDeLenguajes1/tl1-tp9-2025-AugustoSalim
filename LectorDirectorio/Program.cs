// Namespace general del proyecto
namespace LectorDirectorioApp
{
    // Clase principal del programa
    class Program
    {
        static void Main()
        {
            string ruta; // Variable para almacenar la ruta del directorio ingresada por el usuario

            // Bucle que se repite hasta que el usuario ingrese una ruta de directorio válida
            do
            {
                Console.WriteLine("Ingrese la ruta del directorio a analizar:"); // Solicita al usuario ingresar un path
                ruta = Console.ReadLine(); // Lee el path ingresado por el usuario

                // Verifica si la ruta ingresada existe
                if (!Directory.Exists(ruta))
                {
                    Console.WriteLine("El directorio ingresado no existe. Intente nuevamente.\n"); // Muestra mensaje de error
                }
                else
                {
                    break; // Si la ruta es válida, rompe el bucle
                }

            } while (true);//Podríamos usar un while (!Directory.Exists(ruta)) también, pero así tenemos más control con el break.

            // Mostrar las carpetas dentro del directorio ingresado
            Console.WriteLine("\nCarpetas encontradas:");
            string[] carpetas = Directory.GetDirectories(ruta); // Obtiene todos los subdirectorios dentro de la ruta
            // Directory.GetDirectories(ruta) devuelve las rutas completas de las carpetas.Ejemplo: C:\MiCarpeta\Subcarpeta1. Path.GetFileName(...) extrae solo el nombre del último segmento, o sea, el nombre de la carpeta.Resultado: Subcarpeta1. Lo usamos para que en la consola solo veas Subcarpeta1 y no toda la ruta.
            foreach (string carpeta in carpetas)
            {
                Console.WriteLine("- " + Path.GetFileName(carpeta)); // Muestra solo el nombre de la carpeta
            }

            // Mostrar los archivos dentro del directorio ingresado
            Console.WriteLine("\nArchivos encontrados:");
            string[] archivos = Directory.GetFiles(ruta); // Obtiene todos los archivos dentro del directorio (sin entrar a subcarpetas)

            List<string> lineasCSV = new List<string>(); // Lista para almacenar las líneas del archivo CSV
            // Agregamos encabezado al archivo CSV
            lineasCSV.Add("Nombre del Archivo;Tamaño (KB);Fecha de Ultima Modificacion");

            // Recorremos todos los archivos encontrados
            foreach (string archivo in archivos)
            {
                FileInfo info = new(archivo); // Obtenemos información detallada del archivo
                double tamanioKB = Math.Round(info.Length / 1024.0, 2); // Calculamos el tamaño en KB y redondeamos a 2 decimales
                DateTime fechaModificacion = info.LastWriteTime; // Guardamos la fecha de última modificación como tipo DateTime
                string fecha = fechaModificacion.ToString("yyyy-MM-dd HH:mm:ss"); // Convertimos la fecha a string con formato


                // Mostramos en consola el nombre y tamaño del archivo
                Console.WriteLine("- " + info.Name + " (" + tamanioKB + " KB)");

                // Agregamos una nueva línea con los datos al CSV usando punto y coma como separador
                lineasCSV.Add($"{info.Name};{tamanioKB};{fecha}");
            }

            // Ruta donde se guardará el archivo CSV (en el mismo directorio analizado)
            string rutaCSV = Path.Combine(ruta, "reporte_archivos.csv");
            File.WriteAllLines(rutaCSV, lineasCSV); // Escribimos todas las líneas al archivo CSV
            // Esta línea:Crea el archivo si no existe.Sobrescribe el archivo si ya existe.Escribe todas las líneas del List<string> llamado lineasCSV.

            // Confirmación final en consola
            Console.WriteLine($"\nReporte CSV generado correctamente en: {rutaCSV}");
        }
    }
}
