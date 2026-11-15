using System;

public class Pelicula {
    // Aqui guardamos los 6 datos de cada pelicula
    private string title;
    private string genre;
    private string premiere;
    private string runtime;
    private string imdbScore;
    private string language;

    // Constructor para crear una pelicula nueva con sus 6 datos
    public Pelicula(string pTitle, string pGenre, string pPremiere, string pRuntime, string pImdbScore, string pLanguage) {
        this.title = pTitle;
        this.genre = pGenre;
        this.premiere = pPremiere;
        this.runtime = pRuntime;
        this.imdbScore = pImdbScore;
        this.language = pLanguage;
    }

    // 1. Metodo
    // Metodo para devolver el titulo de la pelicula
    public string GetTitle() {
        return title;
    }

    // 2. Metodo
    // Metodo para devolver el genero de la pelicula
    public string GetGenre() {
        return genre;
    }

    // 3. Metodo
    // Metodo para devolver la fecha de estreno de la pelicula
    public string GetPremiere() {
        return premiere;
    }

    // 4. Metodo
    // Metodo para devolver la duracion de la pelicula
    public string GetRuntime() {
        return runtime;
    }

    // 5. Metodo
    // Metodo para devolver la nota IMDB de la pelicula
    public string GetImdbScore() {
        return imdbScore;
    }

    // 6. Metodo
    // Metodo para devolver el idioma de la pelicula
    public string GetLanguage() {
        return language;
    }

    // 7. Metodo
    // Metodo para juntar todos los datos de la pelicula en una sola frase
    public override string ToString() {
        return "Título: " + title + " | Género: " + genre + " | Estreno: " + premiere +
               " | Duración: " + runtime + " min | Nota: " + imdbScore + " | Idioma: " + language;
    }
}