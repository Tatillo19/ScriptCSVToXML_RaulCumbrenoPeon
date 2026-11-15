using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class GestorArchivo {
    // 1. Método
    // Método para leer el archivo CSV y devolver una lista con todas las películas
    public static List<Pelicula> LeerCsv(string rutaCsv) {
        List<Pelicula> lista = new List<Pelicula>();

        try {
            // Si el archivo no existe mandamos un error al log y salimos
            if (!File.Exists(rutaCsv)) {
                Logger.LogError("No se encuentra el CSV en: " + rutaCsv);
                return lista;
            }

            // Leemos todas las líneas del archivo de golpe
            string[] lineas = File.ReadAllLines(rutaCsv);

            // Empezamos en 1 para saltarnos la línea 0 que son los nombres de las columnas
            for (int i = 1; i < lineas.Length; i++) {
                if (lineas[i] != "") {
                    // Separamos los datos de la línea
                    List<string> campos = TrocearLineaCsv(lineas[i]);

                    // Si tenemos los 6 datos creamos la película y la metemos en la lista
                    if (campos.Count >= 6) {
                        Pelicula p = new Pelicula(campos[0], campos[1], campos[2], campos[3], campos[4], campos[5]);
                        lista.Add(p);
                    }
                }
            }
        }
        catch (Exception ex) {
            Logger.LogError("Fallo al leer el CSV: " + ex.Message);
        }

        return lista;
    }

    // 2. Método
    // Método para leer el archivo XML y devolver una lista con todas las películas
    public static List<Pelicula> LeerXml(string rutaXml) {
        List<Pelicula> lista = new List<Pelicula>();

        try {
            if (!File.Exists(rutaXml)) {
                Logger.LogError("No encuentro el archivo XML en: " + rutaXml);
                return lista;
            }

            // Abrimos el documento XML
            XmlDocument doc = new XmlDocument();
            doc.Load(rutaXml);

            // Buscamos todas las etiquetas Pelicula
            XmlNodeList nodos = doc.GetElementsByTagName("Pelicula");

            // Recorremos cada película del XML y sacamos sus 6 datos
            foreach (XmlNode nodo in nodos) {
                string title = "";
                string genre = "";
                string premiere = "";
                string runtime = "";
                string score = "";
                string language = "";

                if (nodo["Title"] != null) {
                    title = nodo["Title"].InnerText;
                }
                if (nodo["Genre"] != null) {
                    genre = nodo["Genre"].InnerText;
                }
                if (nodo["Premiere"] != null) {
                    premiere = nodo["Premiere"].InnerText;
                }
                if (nodo["Runtime"] != null) {
                    runtime = nodo["Runtime"].InnerText;
                }
                if (nodo["IMDBScore"] != null) {
                    score = nodo["IMDBScore"].InnerText;
                }
                if (nodo["Language"] != null) {
                    language = nodo["Language"].InnerText;
                }

                Pelicula p = new Pelicula(title, genre, premiere, runtime, score, language);
                lista.Add(p);
            }
        }
        catch (Exception ex) {
            Logger.LogError("Fallo al leer el XML: " + ex.Message);
        }

        return lista;
    }

    // 3. Método
    // Método para sacar la lista de todos los géneros sin que se repitan
    public static List<string> ObtenerGeneros(List<Pelicula> peliculas) {
        List<string> generos = new List<string>();

        foreach (Pelicula p in peliculas) {
            // Si este género no está todavía en la lista lo añadimos
            if (!generos.Contains(p.GetGenre())) {
                generos.Add(p.GetGenre());
            }
        }

        return generos;
    }

    // 4. Método
    // Método para sacar la lista de todos los idiomas sin que se repitan
    public static List<string> ObtenerIdiomas(List<Pelicula> peliculas) {
        List<string> idiomas = new List<string>();

        foreach (Pelicula p in peliculas) {
            // Si este idioma no está todavía en la lista lo añadimos
            if (!idiomas.Contains(p.GetLanguage())) {
                idiomas.Add(p.GetLanguage());
            }
        }

        return idiomas;
    }

    // 5. Método
    // Método para filtrar las películas, exportarlas al nuevo formato y mandar los logs
    public static void Exportar(string rutaOrigen, string rutaDestino, string tipoConversion, int opcionFiltro, string valorFiltro) {
        // Preparamos el texto del filtro para ponerlo en el log de inicio
        string textoFiltro = "Exportar todo el documento";
        if (opcionFiltro == 2) {
            textoFiltro = "Por Género: " + valorFiltro;
        }
        if (opcionFiltro == 3) {
            textoFiltro = "Por Año >= " + valorFiltro;
        }
        if (opcionFiltro == 4) {
            textoFiltro = "Por Idioma: " + valorFiltro;
        }

        // Mandamos el log de inicio
        Logger.LogInicio(rutaOrigen, rutaDestino, textoFiltro, tipoConversion);

        // Leemos las películas del archivo de origen
        List<Pelicula> todas;
        if (tipoConversion == "CSV a XML") {
            todas = LeerCsv(rutaOrigen);
        }
        else {
            todas = LeerXml(rutaOrigen);
        }

        // Si no se ha cargado ninguna peli mandamos un aviso y paramos
        if (todas.Count == 0) {
            Logger.LogAviso("No hay películas para exportar");
            return;
        }

        // Miramos película por película si cumple el filtro elegido
        List<Pelicula> filtradas = new List<Pelicula>();

        foreach (Pelicula p in todas) {
            bool pasaElFiltro = false;

            if (opcionFiltro == 1) {
                pasaElFiltro = true;
            }
            else if (opcionFiltro == 2) {
                if (p.GetGenre().ToLower() == valorFiltro.ToLower()) {
                    pasaElFiltro = true;
                }
            }
            else if (opcionFiltro == 3) {
                int añoPeli = SacarAño(p.GetPremiere());
                int añoUsuario = int.Parse(valorFiltro);

                if (añoPeli >= añoUsuario) {
                    pasaElFiltro = true;
                }
            }
            else if (opcionFiltro == 4) {
                if (p.GetLanguage().ToLower() == valorFiltro.ToLower()) {
                    pasaElFiltro = true;
                }
            }

            // Si pasa el filtro la guardamos y mandamos su log de elemento
            if (pasaElFiltro) {
                filtradas.Add(p);
                Logger.LogElemento(p.ToString());
            }
        }

        if (filtradas.Count == 0) {
            Logger.LogAviso("Ninguna película coincide con el filtro: " + textoFiltro);
        }

        // Guardamos las películas filtradas en el archivo nuevo
        if (tipoConversion == "CSV a XML") {
            GuardarXml(filtradas, rutaDestino);
        }
        else {
            GuardarCsv(filtradas, rutaDestino);
        }

        // Mandamos el log de fin diciendo dónde está el archivo nuevo
        Logger.LogFin(rutaDestino);
    }

    // 6. Método
    // Método para guardar la lista de películas en un archivo XML
    private static void GuardarXml(List<Pelicula> peliculas, string rutaDestino) {
        try {
            XmlDocument doc = new XmlDocument();

            // Creamos la primera línea del XML
            XmlDeclaration cabecera = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(cabecera);

            // Creamos la etiqueta principal Peliculas
            XmlElement raiz = doc.CreateElement("Peliculas");
            doc.AppendChild(raiz);

            // Por cada película creamos su bloque Pelicula con sus 6 etiquetas dentro
            foreach (Pelicula p in peliculas) {
                XmlElement bloquePelicula = doc.CreateElement("Pelicula");

                AñadirEtiqueta(doc, bloquePelicula, "Title", p.GetTitle());
                AñadirEtiqueta(doc, bloquePelicula, "Genre", p.GetGenre());
                AñadirEtiqueta(doc, bloquePelicula, "Premiere", p.GetPremiere());
                AñadirEtiqueta(doc, bloquePelicula, "Runtime", p.GetRuntime());
                AñadirEtiqueta(doc, bloquePelicula, "IMDBScore", p.GetImdbScore());
                AñadirEtiqueta(doc, bloquePelicula, "Language", p.GetLanguage());

                raiz.AppendChild(bloquePelicula);
            }

            // Guardamos el archivo XML
            doc.Save(rutaDestino);
        }
        catch (Exception ex) {
            Logger.LogError("Fallo al guardar el archivo XML: " + ex.Message);
        }
    }

    // 7. Método
    // Método auxiliar para meter una etiqueta dentro de Pelicula sin repetir código
    private static void AñadirEtiqueta(XmlDocument doc, XmlElement padre, string nombre, string texto) {
        XmlElement etiqueta = doc.CreateElement(nombre);
        etiqueta.InnerText = texto;
        padre.AppendChild(etiqueta);
    }

    // 8. Método
    // Método para guardar la lista de películas en un archivo CSV
    private static void GuardarCsv(List<Pelicula> peliculas, string rutaDestino) {
        try {
            List<string> lineasSalida = new List<string>();

            // Primera línea con los nombres de las columnas
            lineasSalida.Add("Title,Genre,Premiere,Runtime,IMDB Score,Language");

            // Metemos cada película separada por comas
            foreach (Pelicula p in peliculas) {
                string titulo = p.GetTitle();
                string fecha = p.GetPremiere();

                // Si el título o la fecha tienen una coma dentro les ponemos comillas para no liar el CSV
                if (titulo.Contains(",")) {
                    titulo = "\"" + titulo + "\"";
                }
                if (fecha.Contains(",")) {
                    fecha = "\"" + fecha + "\"";
                }

                string linea = titulo + "," + p.GetGenre() + "," + fecha + "," +
                               p.GetRuntime() + "," + p.GetImdbScore() + "," + p.GetLanguage();

                lineasSalida.Add(linea);
            }

            // Escribimos todas las líneas en el archivo CSV nuevo
            File.WriteAllLines(rutaDestino, lineasSalida);
        }
        catch (Exception ex) {
            Logger.LogError("Fallo al guardar el archivo CSV: " + ex.Message);
        }
    }

    // 9. Método
    // Método para sacar los 4 últimos números de la fecha y obtener el año
    private static int SacarAño(string fecha) {
        try {
            string limpia = fecha.Trim();
            if (limpia.Length >= 4) {
                string ultimosCuatro = limpia.Substring(limpia.Length - 4);
                return int.Parse(ultimosCuatro);
            }
        }
        catch (Exception ex) {
            Logger.LogAviso("No se pudo sacar el año de la fecha '" + fecha + "': " + ex.Message);
        }
        return 0;
    }

    // 10. Método
    // Método para separar una línea del CSV por comas respetando las comillas
    private static List<string> TrocearLineaCsv(string linea) {
        List<string> campos = new List<string>();
        string palabraActual = "";
        bool dentroDeComillas = false;

        for (int i = 0; i < linea.Length; i++) {
            char letra = linea[i];

            if (letra == '"') {
                // Si encontramos unas comillas entramos o salimos de ellas
                if (dentroDeComillas == false) {
                    dentroDeComillas = true;
                }
                else {
                    dentroDeComillas = false;
                }
            }
            else if (letra == ',' && dentroDeComillas == false) {
                // Si es una coma y no estamos dentro de comillas guardamos el campo
                campos.Add(palabraActual.Trim());
                palabraActual = "";
            }
            else {
                // Si es una letra normal la vamos juntando
                palabraActual = palabraActual + letra;
            }
        }

        campos.Add(palabraActual.Trim());
        return campos;
    }
}