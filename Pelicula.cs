using System;

public class Pelicula {
    private string title;
    private string genre;
    private string premiere;
    private string runtime;
    private string imdbScore;
    private string language;

    public Pelicula(string pTitle, string pGenre, string pPremiere, string pRuntime, string pImdbScore, string pLanguage) {
        this.title = pTitle;
        this.genre = pGenre;
        this.premiere = pPremiere;
        this.runtime = pRuntime;
        this.imdbScore = pImdbScore;
        this.language = pLanguage;
    }

    public string GetTitle() {
        return title;
    }

    public string GetGenre() {
        return genre;
    }

    public string GetPremiere() {
        return premiere;
    }

    public string GetRuntime() {
        return runtime;
    }

    public string GetImdbScore() {
        return imdbScore;
    }

    public string GetLanguage() {
        return language;
    }

    public override string ToString() {
        return "Título: " + title + " | Género: " + genre + " | Estreno: " + premiere +
               " | Duración: " + runtime + " min | Nota: " + imdbScore + " | Idioma: " + language;
    }
}