using Webscraper;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using Windows.Storage;
using System.Runtime.InteropServices;

IWebDriver driver;
scraping();


void scraping()
{
    Console.WriteLine("Wat wil je web scrapen?");
    Console.WriteLine("1 Youtube");
    Console.WriteLine("2 ICT Jobs");
    Console.WriteLine("3 NMBS trein map");
    Console.WriteLine("Antwoord met 1 tot 3.");

    string antwoord = Console.ReadLine();

    if (antwoord == "1")
    {
        Console.WriteLine("Youtube zoekterm?");
        string zoekterm = Console.ReadLine();
        YT(zoekterm);
    }
    else if (antwoord == "2")
    {
        Console.WriteLine("Keyword zoekterm?");
        string zoekterm = Console.ReadLine();
        ICTjobs(zoekterm);

    }
    else if (antwoord == "3")
    {
        Console.WriteLine("Station naam?");
        string zoekterm = Console.ReadLine();
        NMBS(zoekterm);
    }
    else
    {
        Console.WriteLine("Antwoord 1 TOT 3");
        antwoord = Console.ReadLine();
    }
}

void YT(string zoekterm)
{
    
    driver = new FirefoxDriver("C:\\Users\\wiela\\OneDrive\\Bureaublad\\DevOps\\Webscraper");
    String YT1 = " https://www.youtube.com/results?search_query=";
    String YT2 = "&sp=CAI%253D";

    driver.Url = YT1 + zoekterm + YT2;
    driver.FindElement(By.CssSelector(".yt-spec-button-shape-next.yt-spec-button-shape-next--filled.yt-spec-button-shape-next--call-to-action.yt-spec-button-shape-next--size-m ")).Click();
    string jsonString = "";
    Console.WriteLine("\n");


    for (int x = 1; x <= 5; x++)
    {

        var link = driver.FindElement(By.CssSelector("ytd-video-renderer.ytd-item-section-renderer:nth-child("+ x +") > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > h3:nth-child(1) > a:nth-child(2)"));
        var title = driver.FindElement(By.CssSelector("ytd-video-renderer.ytd-item-section-renderer:nth-child("+ x +") > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > h3:nth-child(1) > a:nth-child(2) > yt-formatted-string:nth-child(2)"));
        var uploader = driver.FindElement(By.CssSelector("ytd-video-renderer.ytd-item-section-renderer:nth-child("+ x +") > div:nth-child(1) > div:nth-child(2) > div:nth-child(2) > ytd-channel-name:nth-child(2) > div:nth-child(1) > div:nth-child(1) > yt-formatted-string:nth-child(1) > a:nth-child(1)"));
        var views = driver.FindElement(By.CssSelector("ytd-video-renderer.ytd-item-section-renderer:nth-child("+ x +") > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > ytd-video-meta-block:nth-child(2) > div:nth-child(1) > div:nth-child(2) > span:nth-child(3)"));

        var Youtube = new YouTube
        {
            links = link.GetAttribute("href"),
            titles = title.Text,
            uploaders = uploader.Text,
            viewz = views.Text
        };

        Console.WriteLine("Link: " + link.GetAttribute("href"));
        Console.WriteLine("Title: " + title.Text);
        Console.WriteLine("Uploader: " + uploader.Text);
        Console.WriteLine("Views: " + views.Text);
        Console.WriteLine("\n");

        var options = new JsonSerializerOptions { WriteIndented = true };
        jsonString += JsonSerializer.Serialize(Youtube, options);

    }
    driver.Close();
    Opslaan(jsonString);
}

void ICTjobs(string zoekterm)
{

    driver = new FirefoxDriver("C:\\Users\\wiela\\OneDrive\\Bureaublad\\DevOps\\Webscraper");
    String ICTJ = "https://www.ictjob.be/nl/it-vacatures-zoeken?keywords=" + zoekterm + "/";
    driver.Url = ICTJ;
    System.Threading.Thread.Sleep(5000);
    driver.FindElement(By.CssSelector("#sort-by-date")).Click();
    string jsonString = "";
    Console.WriteLine("\n");

    for (int x = 1; x <= 6; x++)
    {
        if (x == 4) {
            x++;
        }

        var titel = driver.FindElement(By.CssSelector("li.search-item:nth-child("+ x +") > span:nth-child(2) > a:nth-child(1) > h2:nth-child(1)"));
        var bedrijf = driver.FindElement(By.CssSelector("li.search-item:nth-child("+ x +") > span:nth-child(2) > span:nth-child(2)"));
        var locatie = driver.FindElement(By.CssSelector("li.search-item:nth-child("+ x +") > span:nth-child(2) > span:nth-child(3) > span:nth-child(2) > span:nth-child(1) > span:nth-child(1)"));
        var keyword = driver.FindElement(By.CssSelector("li.search-item:nth-child("+ x +") > span:nth-child(2) > span:nth-child(4)"));
        var link = driver.FindElement(By.CssSelector("li.search-item:nth-child("+ x +") > span:nth-child(2) > a:nth-child(1)"));

        var ict = new Ict
        {
            titels = titel.Text,
            bedrijven = bedrijf.Text,
            locaties = locatie.Text,
            keywords = keyword.Text,
            links = link.GetAttribute("href")
        };

        Console.WriteLine("Titel: " + titel.Text);
        Console.WriteLine("Bedrijf: " + bedrijf.Text);
        Console.WriteLine("Locatie " + locatie.Text);
        Console.WriteLine("Keywords: " + keyword.Text);
        Console.WriteLine("Link: " + link.GetAttribute("href"));
        Console.WriteLine("\n");

        var options = new JsonSerializerOptions { WriteIndented = true };
        jsonString += JsonSerializer.Serialize(ict, options);
    }
    driver.Close();
    Opslaan(jsonString);

}

void NMBS(string Station)
{

    driver = new FirefoxDriver("C:\\Users\\wiela\\OneDrive\\Bureaublad\\DevOps\\Webscraper");
    String Treinmap = "https://trainmap.belgiantrain.be/?lang=nl";
    driver.Url = Treinmap;
    driver.FindElement(By.CssSelector(".search-input-station-background")).SendKeys(Station);
    System.Threading.Thread.Sleep(200);
    driver.FindElement(By.CssSelector(".search-results > div:nth-child(1)")).Click();
    System.Threading.Thread.Sleep(200);
    string jsonString = "";
    Console.WriteLine("\n");

    for (int x = 1; x <= 5; x++)
    {

        var tijd = driver.FindElement(By.CssSelector("div.departureboard:nth-child("+ x +") > div:nth-child(1) > span:nth-child(1)"));
        var destenatie = driver.FindElement(By.CssSelector("div.departureboard:nth-child("+ x +") > div:nth-child(4)"));
        var type = driver.FindElement(By.CssSelector("div.departureboard:nth-child("+ x +") > div:nth-child(2)"));
        var spoor = driver.FindElement(By.CssSelector("div.departureboard:nth-child("+ x +") > div:nth-child(3) > div:nth-child(1)"));

        var check = driver.FindElements(By.CssSelector("div.departureboard:nth-child(" + x + ") > div:nth-child(1) > span:nth-child(2)"));
        string vertragingText = "";
        if (check.Count >= 1)
        {
            var vertraging = driver.FindElement(By.CssSelector("div.departureboard:nth-child("+ x +") > div:nth-child(1) > span:nth-child(2)"));
            vertragingText = vertraging.Text;
        }
        else
        {
            vertragingText = "No Delay";
        }

        var nmbs = new Nmbs
        {
            tijden = tijd.Text,
            vertragingen = vertragingText,
            destenaties = destenatie.Text,
            types = type.Text,
            spooren = spoor.Text
        };

        Console.WriteLine("Vertrek tijd: " + tijd.Text);
        Console.WriteLine("Vertraging: " + vertragingText);
        Console.WriteLine("Destenatie: " + destenatie.Text);
        Console.WriteLine("Type: " + type.Text);
        Console.WriteLine("Spoor: " + spoor.Text);
        Console.WriteLine("\n");

        var options = new JsonSerializerOptions { WriteIndented = true };
        jsonString += JsonSerializer.Serialize(nmbs, options);
    }
    driver.Close();
    Opslaan(jsonString);
}

void Opslaan(string Data)
{

    Console.WriteLine("1 Data Opslaan");
    Console.WriteLine("2 Niet Opslaan");
    Console.WriteLine("Antwoord met 1 of 2");

    string antwoord = Console.ReadLine();

    if (antwoord == "1")
    {
        Console.WriteLine("Geef filename");
        string naam = Console.ReadLine();
        string downloadsPath = KnownFolders.GetPath(KnownFolder.Downloads);
        string pad = downloadsPath + "/" + naam + ".json";
        System.IO.File.WriteAllText(pad, Data);
        Console.WriteLine("Succesvol Opgeslagen \n");
        System.Threading.Thread.Sleep(1000);
        scraping();
    }
    else if (antwoord == "2")
    {
        Console.WriteLine("Data Niet Opgeslagen");
        scraping();
    }
    else
    {
        Console.WriteLine("Antwoord met 1 of 2");
    }
}



public class YouTube
{
    public string links { get; set; }
    public string titles { get; set; }
    public string uploaders { get; set; }
    public string viewz { get; set; }
}

public class Ict
{
    public string titels { get; set; }
    public string bedrijven { get; set; }
    public string locaties { get; set; }
    public string keywords { get; set; }
    public string links { get; set; }
}


public class Nmbs
{
    public string tijden { get; set; }
    public string vertragingen { get; set; }
    public string destenaties { get; set; }
    public string types { get; set; }
    public string spooren { get; set; }
}



public enum KnownFolder
{
    Contacts,
    Downloads,
    Favorites,
    Links,
    SavedGames,
    SavedSearches
}

public static class KnownFolders
{
    private static readonly Dictionary<KnownFolder, Guid> _guids = new()
    {
        [KnownFolder.Contacts] = new("56784854-C6CB-462B-8169-88E350ACB882"),
        [KnownFolder.Downloads] = new("374DE290-123F-4565-9164-39C4925E467B"),
        [KnownFolder.Favorites] = new("1777F761-68AD-4D8A-87BD-30B759FA33DD"),
        [KnownFolder.Links] = new("BFB9D5E0-C6A9-404C-B2B2-AE6DB6AF4968"),
        [KnownFolder.SavedGames] = new("4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4"),
        [KnownFolder.SavedSearches] = new("7D1D3A04-DEBB-4115-95CF-2F29DA2920DA")
    };

    public static string GetPath(KnownFolder knownFolder)
    {
        return SHGetKnownFolderPath(_guids[knownFolder], 0);
    }

    [DllImport("shell32",
        CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
    private static extern string SHGetKnownFolderPath(
        [MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags,
        nint hToken = 0);
}