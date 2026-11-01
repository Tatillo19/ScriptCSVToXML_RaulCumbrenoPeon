using System.IO;
using System.Text;
using System.Xml;

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
                                string[] lineas = File.ReadAllLines(rutaEntrada);

                                for (int i = 1; i < lineas.Length; i++) {
                                    string[] campos = new string[6];
                                    int posicion = 0;
                                    string campoActual = "";
                                    bool dentroDeComillas = false;

                                    foreach (char letra in lineas[i]) {
                                        if (letra == '"') {
                                            dentroDeComillas = !dentroDeComillas;
                                        }
                                        else if (letra == ',' && !dentroDeComillas) {
                                            if (posicion < 6) campos[posicion] = campoActual;
                                            posicion++;
                                            campoActual = "";
                                        }
                                        else {
                                            campoActual += letra;
                                        }
                                    }
                                    if (posicion < 6) campos[posicion] = campoActual;

                                    Pelicula pelicula = new Pelicula(
                                        campos[0],
                                        campos[1],
                                        campos[2],
                                        campos[3],
                                        campos[4],
                                        campos[5]
                                    );

                                    peliculas.Add(pelicula);
                                }
                            }
                            else {
                                XmlDocument doc = new XmlDocument();
                                doc.Load(rutaEntrada);

                                foreach (XmlNode nodo in doc.SelectNodes("//Pelicula")) {
                                    Pelicula pelicula = new Pelicula(
                                        nodo["Title"].InnerText,
                                        nodo["Genre"].InnerText,
                                        nodo["Premiere"].InnerText,
                                        nodo["Runtime"].InnerText,
                                        nodo["IMDBScore"].InnerText,
                                        nodo["Language"].InnerText
                                    );

                                    peliculas.Add(pelicula);
                                }
                            }

                            List<Pelicula> peliculasAExportar = new List<Pelicula>();

                            switch (opcion) {
                                case 3:
                                    peliculasAExportar = peliculas;
                                    break;

                                case 4:
                                case 6:
                                    List<string> valoresDistintos = new List<string>();

                                    foreach (Pelicula pelicula in peliculas) {
                                        string valorBusqueda = "";
                                        if (opcion == 4) {
                                            valorBusqueda = pelicula.Genre;
                                        }
                                        else {
                                            valorBusqueda = pelicula.Language;
                                        }

                                        if (!valoresDistintos.Contains(valorBusqueda) && valorBusqueda != "") {
                                            valoresDistintos.Add(valorBusqueda);
                                        }
                                    }

                                    int numeroDeLista = 1;
                                    foreach (string valor in valoresDistintos) {
                                        Console.WriteLine(numeroDeLista + ". " + valor);
                                        numeroDeLista++;
                                    }

                                    Console.Write("Elige un número: ");
                                    int numeroElegido = int.Parse(Console.ReadLine());
                                    string valorElegido = valoresDistintos[numeroElegido - 1];

                                    foreach (Pelicula pelicula in peliculas) {
                                        string valorComparar = "";
                                        if (opcion == 4) {
                                            valorComparar = pelicula.Genre;
                                        }
                                        else {
                                            valorComparar = pelicula.Language;
                                        }

                                        if (valorComparar == valorElegido) {
                                            peliculasAExportar.Add(pelicula);
                                        }
                                    }
                                    break;

                                case 5:
                                    Console.Write("Año (se exporta ese año y los posteriores): ");
                                    int anioMinimo = int.Parse(Console.ReadLine());

                                    foreach (Pelicula pelicula in peliculas) {
                                        string fechaEstreno = pelicula.Premiere;
                                        string[] partesFecha = fechaEstreno.Split(',');
                                        string anioTexto = partesFecha[1].Trim();
                                        int anioDeLaPelicula = int.Parse(anioTexto);

                                        if (anioDeLaPelicula >= anioMinimo) {
                                            peliculasAExportar.Add(pelicula);
                                        }
                                    }
                                    break;
                            }

                            int exportadas = 0;

                            if (opcionPrincipal == 1) {
                                XmlWriter escritor = XmlWriter.Create(rutaSalida);
                                escritor.WriteStartDocument();
                                escritor.WriteStartElement("Peliculas");

                                foreach (Pelicula pelicula in peliculasAExportar) {
                                    escritor.WriteStartElement("Pelicula");

                                    escritor.WriteStartElement("Title");
                                    escritor.WriteString(pelicula.Title);
                                    escritor.WriteEndElement();

                                    escritor.WriteStartElement("Genre");
                                    escritor.WriteString(pelicula.Genre);
                                    escritor.WriteEndElement();

                                    escritor.WriteStartElement("Premiere");
                                    escritor.WriteString(pelicula.Premiere);
                                    escritor.WriteEndElement();

                                    escritor.WriteStartElement("Runtime");
                                    escritor.WriteString(pelicula.Runtime);
                                    escritor.WriteEndElement();

                                    escritor.WriteStartElement("IMDBScore");
                                    escritor.WriteString(pelicula.IMDBScore);
                                    escritor.WriteEndElement();

                                    escritor.WriteStartElement("Language");
                                    escritor.WriteString(pelicula.Language);
                                    escritor.WriteEndElement();

                                    escritor.WriteEndElement();
                                    exportadas++;
                                }

                                escritor.WriteEndElement();
                                escritor.WriteEndDocument();
                                escritor.Close();
                            }
                            else {
                                StringBuilder textoCsv = new StringBuilder();
                                textoCsv.AppendLine("Title,Genre,Premiere,Runtime,IMDB Score,Language");

                                foreach (Pelicula pelicula in peliculasAExportar) {
                                    string[] campos = {
                                        pelicula.Title,
                                        pelicula.Genre,
                                        pelicula.Premiere,
                                        pelicula.Runtime,
                                        pelicula.IMDBScore,
                                        pelicula.Language
                                    };

                                    string linea = "";

                                    for (int i = 0; i < 6; i++) {
                                        if (campos[i].Contains(",")) {
                                            linea += "\"" + campos[i] + "\"";
                                        }
                                        else {
                                            linea += campos[i];
                                        }

                                        if (i < 5) {
                                            linea += ",";
                                        }
                                    }

                                    textoCsv.AppendLine(linea);
                                    exportadas++;
                                }

                                File.WriteAllText(rutaSalida, textoCsv.ToString());
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