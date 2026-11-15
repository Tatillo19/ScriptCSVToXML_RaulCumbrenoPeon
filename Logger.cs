using System;
using System.Diagnostics;

public class Logger {
    // Nombre con el que saldra nuestro programa en el Visor de Eventos de Windows
    private static string SOURCE = "PracticaLogs_Netflix";
    private static string LOG_NAME = "Application";

    // 1. Metodo
    // Metodo para crear el origen del log en Windows al arrancar el programa
    public static void Inicializar() {
        try {
            // Si el origen no existe en Windows lo creamos
            if (!EventLog.SourceExists(SOURCE)) {
                EventLog.CreateEventSource(SOURCE, LOG_NAME);
            }
        }
        catch (Exception ex) {
            Console.WriteLine("No se pudo crear el origen del log (abre VS Code como Administrador la primera vez): " + ex.Message);
        }
    }

    // 2. Metodo
    // Metodo para sacar el mensaje por consola y guardarlo en el Visor de Eventos de Windows
    public static void EscribirLog(string mensaje, EventLogEntryType tipo, int id) {
        // Primero lo mostramos en la consola
        Console.WriteLine("[" + tipo + "] (ID: " + id + ") -> " + mensaje);

        // Despues lo guardamos en Windows
        try {
            EventLog.WriteEntry(SOURCE, mensaje, tipo, id);
        }
        catch (Exception ex) {
            Console.WriteLine("Fallo al guardar el log en Windows: " + ex.Message);
        }
    }

    // 3. Metodo
    // Metodo para registrar el inicio de la exportacion con origen, destino, filtro y tipo de conversion
    public static void LogInicio(string origen, string destino, string filtro, string tipoConversion) {
        string mensaje = "INICIO DE EXPORTACIÓN\n" +
                         "Origen: " + origen + "\n" +
                         "Destino: " + destino + "\n" +
                         "Filtro: " + filtro + "\n" +
                         "Tipo de conversión: " + tipoConversion;

        EscribirLog(mensaje, EventLogEntryType.Information, 100);
    }

    // 4. Metodo
    // Metodo para registrar cada pelicula que se lee y exporta
    public static void LogElemento(string datosPelicula) {
        string mensaje = "Película leída -> " + datosPelicula;
        EscribirLog(mensaje, EventLogEntryType.Information, 101);
    }

    // 5. Metodo
    // Metodo para registrar que ha terminado la exportacion y donde esta el fichero nuevo
    public static void LogFin(string rutaDestino) {
        string mensaje = "FIN DE EXPORTACIÓN\nArchivo nuevo guardado en: " + rutaDestino;
        EscribirLog(mensaje, EventLogEntryType.Information, 102);
    }

    // 6. Metodo
    // Metodo para registrar un aviso cuando pasa algo raro que no rompe el programa
    public static void LogAviso(string mensajeAviso) {
        EscribirLog("AVISO: " + mensajeAviso, EventLogEntryType.Warning, 200);
    }

    // 7. Metodo
    // Metodo para registrar un error cuando falla algun archivo o dato
    public static void LogError(string mensajeError) {
        EscribirLog("ERROR: " + mensajeError, EventLogEntryType.Error, 300);
    }
}