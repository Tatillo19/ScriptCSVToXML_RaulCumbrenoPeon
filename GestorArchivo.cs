using System.IO;
using System.Text;
using System.Xml;

public class GestorArchivo
{
    public static List<Pelicula> LeerCsv(string ruta)
    {
        List<Pelicula> peliculas = new List<Pelicula>();
        string[] lineas = File.ReadAllLines(ruta);

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

        return peliculas;
    }

    public static List<Pelicula> LeerXml(string ruta)
    {
        List<Pelicula> peliculas = new List<Pelicula>();
        XmlDocument doc = new XmlDocument();
        doc.Load(ruta);

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

        return peliculas;
    }

    public static List<string> ObtenerValoresUnicos(List<Pelicula> peliculas, int opcion){
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

        return valoresDistintos;
    }

    public static List<Pelicula> FiltrarPorValor(List<Pelicula> peliculas, int opcion, string valorElegido){
        List<Pelicula> resultado = new List<Pelicula>();

        foreach (Pelicula pelicula in peliculas) {
            string valorComparar = "";
            if (opcion == 4) {
                valorComparar = pelicula.Genre;
            }
            else {
                valorComparar = pelicula.Language;
            }

            if (valorComparar == valorElegido) {
                resultado.Add(pelicula);
            }
        }

        return resultado;
    }

    public static List<Pelicula> FiltrarPorAnio(List<Pelicula> peliculas, int anioMinimo){
        List<Pelicula> resultado = new List<Pelicula>();

        foreach (Pelicula pelicula in peliculas) {
            string fechaEstreno = pelicula.Premiere;
            string[] partesFecha = fechaEstreno.Split(',');
            string anioTexto = partesFecha[1].Trim();
            int anioDeLaPelicula = int.Parse(anioTexto);

            if (anioDeLaPelicula >= anioMinimo) {
                resultado.Add(pelicula);
            }
        }

        return resultado;
    }

    public static int EscribirXml(string ruta, List<Pelicula> peliculas){
        int exportadas = 0;
        XmlWriter escritor = XmlWriter.Create(ruta);
        escritor.WriteStartDocument();
        escritor.WriteStartElement("Peliculas");

        foreach (Pelicula pelicula in peliculas) {
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

        return exportadas;
    }

    public static int EscribirCsv(string ruta, List<Pelicula> peliculas){
        int exportadas = 0;
        StringBuilder textoCsv = new StringBuilder();
        textoCsv.AppendLine("Title,Genre,Premiere,Runtime,IMDB Score,Language");

        foreach (Pelicula pelicula in peliculas) {
            string[] campos = {pelicula.Title, pelicula.Genre, pelicula.Premiere, pelicula.Runtime, pelicula.IMDBScore, pelicula.Language};

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

        File.WriteAllText(ruta, textoCsv.ToString());
        return exportadas;
    }
}