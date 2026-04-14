using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MechTools.SitemapBuilder
{
    /// <summary>
    /// Tool da officina per generare in automatico la sitemap.xml di un sito statico.
    /// Legge la data di ultima modifica dei file per garantire che Googlebot abbia dati reali.
    /// </summary>
    public class Program
    {
        // Il dominio base (con www, coerente con le direttive SEO)
        private const string BaseUrl = "https://www.ilviaggiodelprogrammatore.com";

        public static void Main(string[] args)
        {
            Console.WriteLine("Avvio diagnostica e generazione Sitemap...");

            // Risale automaticamente di un livello rispetto a /SitemapBuilder 
            // per puntare alla root del sito dove risiedono i file HTML
            string targetDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..")); 
            string outputPath = Path.Combine(targetDirectory, "sitemap.xml");

            try
            {
                var sitemapDoc = GenerateSitemap(targetDirectory, BaseUrl);
                sitemapDoc.Save(outputPath);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[SUCCESS] sitemap.xml generata correttamente in: {outputPath}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERROR] Guasto al motore durante la generazione: {ex.Message}");
                Console.ResetColor();
            }
        }

        private static XDocument GenerateSitemap(string rootDirectory, string baseUrl)
        {
            // Namespace richiesto dallo standard sitemaps.org
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";

            var urlset = new XElement(xmlns + "urlset");

            // Cerca tutti i file .html in tutte le sottocartelle
            var htmlFiles = Directory.GetFiles(rootDirectory, "*.html", SearchOption.AllDirectories);

            foreach (var file in htmlFiles)
            {
                // Calcola il percorso relativo per costruire l'URL
                string relativePath = Path.GetRelativePath(rootDirectory, file)
                                          .Replace("\\", "/"); // Normalizza i path per il web

                // Se è l'index.html principale, l'URL sarà solo la radice
                string locUrl = relativePath.Equals("index.html", StringComparison.OrdinalIgnoreCase) 
                                ? baseUrl + "/" 
                                : $"{baseUrl}/{relativePath}";

                // La magia: prendiamo la data REALE in cui hai modificato il file sul disco
                DateTime lastModified = File.GetLastWriteTime(file);

                var urlElement = new XElement(xmlns + "url",
                    new XElement(xmlns + "loc", locUrl),
                    // Formato standard W3C Datetime (YYYY-MM-DD)
                    new XElement(xmlns + "lastmod", lastModified.ToString("yyyy-MM-dd")) 
                );

                urlset.Add(urlElement);
            }

            return new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                urlset
            );
        }
    }
}