using System;
using System.Collections.Generic;

class Program {
    // 1. Método
    // Método principal que arranca el log y muestra el menú con while y switch
    static void Main(string[] args) {
        // Nada más empezar preparamos el log en Windows
        Logger.Inicializar();

        // Rutas por defecto de los archivos
        string rutaCsv = "Data/NetflixOriginals.csv";
        string rutaXml = "Data/peliculas.xml";

        bool salir = false;

        // Bucle del menú para que no se cierre hasta que pulsemos salir
        while (!salir) {
            Console.WriteLine("\nCONVERSOR NETFLIX (CSV a XML y XML a CSV)");
            Console.WriteLine("Ruta CSV: " + rutaCsv);
            Console.WriteLine("Ruta XML: " + rutaXml);
            Console.WriteLine("1. Cambiar rutas de origen y destino");
            Console.WriteLine("2. Pasar de CSV a XML");
            Console.WriteLine("3. Pasar de XML a CSV");
            Console.WriteLine("4. Salir");
            Console.Write("Elige una opción: ");

            string opcion = Console.ReadLine();

            switch (opcion) {
                case "1":
                    Console.Write("Escribe la ruta del CSV (ej: Data/NetflixOriginals.csv): ");
                    string nuevaCsv = Console.ReadLine();
                    if (nuevaCsv != "") {
                        rutaCsv = nuevaCsv;
                    }

                    Console.Write("Escribe la ruta del XML (ej: Data/peliculas.xml): ");
                    string nuevaXml = Console.ReadLine();
                    if (nuevaXml != "") {
                        rutaXml = nuevaXml;
                    }

                    Console.WriteLine("Rutas cambiadas.");
                    break;

                case "2":
                    ElegirFiltro(rutaCsv, rutaXml, "CSV a XML");
                    break;

                case "3":
                    ElegirFiltro(rutaXml, rutaCsv, "XML a CSV");
                    break;

                case "4":
                    salir = true;
                    Console.WriteLine("Hasta luego.");
                    break;

                default:
                    Console.WriteLine("Esa opción no vale.");
                    break;
            }
        }
    }

    // 2. Método
    // Método para elegir con switch si queremos exportar todo o filtrar por género, año o idioma
    static void ElegirFiltro(string origen, string destino, string tipoConversion) {
        Console.WriteLine("\nOPCIONES DE EXPORTACIÓN (" + tipoConversion + ")");
        Console.WriteLine("1. Todo el archivo entero");
        Console.WriteLine("2. Filtrar por género");
        Console.WriteLine("3. Filtrar por año (ese año y más nuevas)");
        Console.WriteLine("4. Filtrar por idioma");
        Console.Write("Elige una opción: ");

        string subOpcion = Console.ReadLine();

        switch (subOpcion) {
            case "1":
                GestorArchivo.Exportar(origen, destino, tipoConversion, 1, "");
                break;

            case "2":
                // Leemos el archivo primero para sacar la lista de géneros que hay
                List<Pelicula> listaGeneros;
                if (tipoConversion == "CSV a XML") {
                    listaGeneros = GestorArchivo.LeerCsv(origen);
                }
                else {
                    listaGeneros = GestorArchivo.LeerXml(origen);
                }

                List<string> generos = GestorArchivo.ObtenerGeneros(listaGeneros);

                Console.WriteLine("\nGéneros disponibles:");
                foreach (string g in generos) {
                    Console.WriteLine("- " + g);
                }

                Console.Write("\nEscribe el género que quieres: ");
                string generoElegido = Console.ReadLine();
                GestorArchivo.Exportar(origen, destino, tipoConversion, 2, generoElegido);
                break;

            case "3":
                Console.Write("Escribe el año mínimo (ej: 2020): ");
                string año = Console.ReadLine();

                try {
                    // Probamos si lo que ha escrito es un número de verdad
                    int.Parse(año);
                    GestorArchivo.Exportar(origen, destino, tipoConversion, 3, año);
                }
                catch {
                    Logger.LogError("Has escrito un año que no es un número: " + año);
                }
                break;

            case "4":
                // Leemos el archivo primero para sacar la lista de idiomas que hay
                List<Pelicula> listaIdiomas;
                if (tipoConversion == "CSV a XML") {
                    listaIdiomas = GestorArchivo.LeerCsv(origen);
                }
                else {
                    listaIdiomas = GestorArchivo.LeerXml(origen);
                }

                List<string> idiomas = GestorArchivo.ObtenerIdiomas(listaIdiomas);

                Console.WriteLine("\nIdiomas disponibles:");
                foreach (string id in idiomas) {
                    Console.WriteLine("- " + id);
                }

                Console.Write("\nEscribe el idioma que quieres: ");
                string idiomaElegido = Console.ReadLine();
                GestorArchivo.Exportar(origen, destino, tipoConversion, 4, idiomaElegido);
                break;

            default:
                Console.WriteLine("Opción no válida.");
                break;
        }
    }
}