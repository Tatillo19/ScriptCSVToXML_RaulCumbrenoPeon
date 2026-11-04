public class Pelicula
{
    public string Title { get; set; }
    public string Genre { get; set; }
    public string Premiere { get; set; }
    public string Runtime { get; set; }
    public string IMDBScore { get; set; }
    public string Language { get; set; }

    public Pelicula(string pTit, string pGenre, string pPremiere, string pRuntime, string pImdbScore, string pLanguage)
    {
        Title = pTit;
        Genre = pGenre;
        Premiere = pPremiere;
        Runtime = pRuntime;
        IMDBScore = pImdbScore;
        Language = pLanguage;
    }
}