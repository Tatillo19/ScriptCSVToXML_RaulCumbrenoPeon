int opcionPrincipal = 0;

while (opcionPrincipal != 3) {
    Console.WriteLine();
    Console.WriteLine("--- MENÚ PRINCIPAL ---");
    Console.WriteLine("1. De CSV a XML");
    Console.WriteLine("2. De XML a CSV");
    Console.WriteLine("3. Cerrar la aplicación");
    Console.Write("Elige una opción: ");

    try {
        opcionPrincipal = int.Parse(Console.ReadLine());
    }
    catch {
        Console.WriteLine("Introduce un número!!");
        opcionPrincipal = 0;
    }

    switch (opcionPrincipal) {
        case 1:
        case 2:
            string rutaEntrada = "";
            string rutaSalida = "";
            int opcion = 0;

            while (opcion != 7) {
                Console.WriteLine();
                if (opcionPrincipal == 1) {
                    Console.WriteLine("--- DE CSV A XML ---");
                }
                else {
                    Console.WriteLine("--- DE XML A CSV ---");
                }

                Console.WriteLine("1. Elegir la ruta del fichero de entrada");
                Console.WriteLine("2. Elegir la ruta del fichero de salida");
                Console.WriteLine("3. Exportar todo el documento");
                Console.WriteLine("4. Exportar por género");
                Console.WriteLine("5. Exportar por año");
                Console.WriteLine("6. Exportar por lenguaje");
                Console.WriteLine("7. Volver al menú principal");
                Console.Write("Elige una opción: ");

                try {
                    opcion = int.Parse(Console.ReadLine());
                }
                catch {
                    Console.WriteLine("Introduce un número!!");
                    opcion = 0;
                }

                switch (opcion) {
                    case 1:
                        Console.Write("Ruta del fichero de entrada: ");
                        rutaEntrada = Console.ReadLine();
                        break;

                    case 2:
                        Console.Write("Ruta del fichero de salida: ");
                        rutaSalida = Console.ReadLine();
                        break;

                    case 3:
                    case 4:
                    case 5:
                    case 6:
                        if (rutaEntrada == "" || rutaSalida == "") {
                            Console.WriteLine("Debes indicar primero la ruta de entrada y de salida.");
                            break;
                        }

                        try {
                            List<Pelicula> peliculas = new List<Pelicula>();

                            if (opcionPrincipal == 1) {
                                peliculas = GestorArchivo.LeerCsv(rutaEntrada);
                            }
                            else {
                                peliculas = GestorArchivo.LeerXml(rutaEntrada);
                            }

                            List<Pelicula> peliculasAExportar = new List<Pelicula>();

                            switch (opcion) {
                                case 3:
                                    peliculasAExportar = peliculas;
                                    break;

                                case 4:
                                case 6:
                                    List<string> valoresDistintos = GestorArchivo.ObtenerValoresUnicos(peliculas, opcion);

                                    int numeroDeLista = 1;
                                    foreach (string valor in valoresDistintos) {
                                        Console.WriteLine(numeroDeLista + ". " + valor);
                                        numeroDeLista++;
                                    }

                                    Console.Write("Elige un número: ");
                                    int numeroElegido = int.Parse(Console.ReadLine());
                                    string valorElegido = valoresDistintos[numeroElegido - 1];

                                    peliculasAExportar = GestorArchivo.FiltrarPorValor(peliculas, opcion, valorElegido);
                                    break;

                                case 5:
                                    Console.Write("Año (se exporta ese año y los posteriores): ");
                                    int anioMinimo = int.Parse(Console.ReadLine());

                                    peliculasAExportar = GestorArchivo.FiltrarPorAnio(peliculas, anioMinimo);
                                    break;
                            }

                            int exportadas = 0;

                            if (opcionPrincipal == 1) {
                                exportadas = GestorArchivo.EscribirXml(rutaSalida, peliculasAExportar);
                            }
                            else {
                                exportadas = GestorArchivo.EscribirCsv(rutaSalida, peliculasAExportar);
                            }

                            Console.WriteLine(exportadas + " películas exportadas a " + rutaSalida);
                        }
                        catch (Exception ex) {
                            Console.WriteLine("Error: " + ex.Message);
                        }

                        Console.WriteLine("Pulsa una tecla para continuar...");
                        Console.ReadKey();
                        break;

                    case 7:
                        break;

                    default:
                        Console.WriteLine("NO VALE ESTA OPCIÓN!");
                        break;
                }
            }
            break;

        case 3:
            Console.WriteLine("Adiós amigo mío.");
            break;

        default:
            Console.WriteLine("NO VALE ESTA OPCIÓN!");
            break;
    }
}