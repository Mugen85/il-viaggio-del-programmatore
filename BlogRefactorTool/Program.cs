using System;
using System.IO;
using System.Text.RegularExpressions;

string rootDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), ".."));
string[] htmlFiles = Directory.GetFiles(rootDir, "*.html", SearchOption.AllDirectories);

int updatedCount = 0;

foreach (var filePath in htmlFiles)
{
    string fileName = Path.GetFileName(filePath);
    
    // Ignoriamo file di sistema e pagine hub
    string[] ignoredFiles = { "header.html", "footer.html", "index.html", "chisono.html", "risorse.html", "blog.html", "mindset.html", "development.html", "glossario-csharp.html", "risorse-anki-csharp.html" };
    if (Array.Exists(ignoredFiles, name => name.Equals(fileName, StringComparison.OrdinalIgnoreCase))) continue;
    if (filePath.Contains("BlogRefactorTool", StringComparison.OrdinalIgnoreCase)) continue;
    if (!filePath.Contains("blog" + Path.DirectorySeparatorChar)) continue;

    string content = File.ReadAllText(filePath);

    // 1. Estrazione del titolo pulito
    Match titleTagMatch = Regex.Match(content, @"<title>(.*?)<\/title>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
    string articleTitle = "";

    if (titleTagMatch.Success)
    {
        articleTitle = titleTagMatch.Groups[1].Value;
        articleTitle = Regex.Replace(articleTitle, @"\s*[-–—]\s*Il Viaggio del Programmatore.*$", "", RegexOptions.IgnoreCase).Trim();
    }

    if (string.IsNullOrEmpty(articleTitle))
    {
        MatchCollection h1Matches = Regex.Matches(content, @"<h1[^>]*>(.*?)<\/h1>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        foreach (Match match in h1Matches)
        {
            string text = Regex.Replace(match.Groups[1].Value, @"<[^>]+>", "").Trim();
            if (!string.IsNullOrEmpty(text) && !text.Equals("Sviluppo Software", StringComparison.OrdinalIgnoreCase) && !text.Equals("Mindset & Vita", StringComparison.OrdinalIgnoreCase))
            {
                articleTitle = text;
                break;
            }
        }
    }

    if (string.IsNullOrEmpty(articleTitle)) articleTitle = Path.GetFileNameWithoutExtension(fileName).Replace("-", " ");

    // 2. Estrazione data
    Match dateMatch = Regex.Match(content, @"(?:(\d{1,2}\s+[^\d<]+\s+\d{4})|([A-Za-z]+\s+\d{1,2},\s+\d{4}))");
    string dateString = dateMatch.Success ? dateMatch.Value : "Ottobre 2026";

    // 3. Calcolo del tempo di lettura
    string pureText = Regex.Replace(content, @"<[^>]*>", "");
    int wordCount = pureText.Split(new char[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
    int readTime = Math.Max(1, wordCount / 200);

    // 4. Setup Categoria (per i colori blu/arancione)
    bool isMindset = filePath.Contains("mindset", StringComparison.OrdinalIgnoreCase);
    string categoryClass = isMindset ? "mindset" : "development";
    string badgeText = isMindset ? "Mindset & Vita" : "Sviluppo Software";
    string iconClass = isMindset ? "bi-compass" : "bi-braces-asterisk";

    // 5. Costruzione dell'Header DEFINITIVO (Senza M, con colori dinamici e icona tech)
    string newHeader = $@"<header class=""article-hero {categoryClass}"">
    <div class=""hero-inner"">
        <div class=""hero-badge"">
            <i class=""bi {iconClass}""></i> {badgeText}
        </div>
        <h1 class=""hero-title"">{articleTitle}</h1>
        <div class=""hero-meta-bar"">
            <div class=""meta-item"">
                <i class=""bi bi-person-bounding-box fs-5""></i>
                <span class=""fw-bold text-dark"">Marco Morello</span>
            </div>
            <div class=""meta-item"">
                <i class=""bi bi-calendar3""></i> {dateString}
            </div>
            <div class=""meta-item"">
                <i class=""bi bi-clock-history""></i> {readTime} min read
            </div>
        </div>
    </div>
</header>";

    string updatedContent = content;

    // Sostituzione blindata
    if (Regex.IsMatch(updatedContent, @"<header[^>]*class=""[^""]*(?:article-hero|blog-header)[^""]*""[^>]*>.*?<\/header>", RegexOptions.Singleline | RegexOptions.IgnoreCase))
    {
        updatedContent = Regex.Replace(updatedContent, @"<header[^>]*class=""[^""]*(?:article-hero|blog-header)[^""]*""[^>]*>.*?<\/header>", newHeader, RegexOptions.Singleline | RegexOptions.IgnoreCase);
    }
    else
    {
        updatedContent = Regex.Replace(updatedContent, @"<header\b[^>]*>.*?<\/header>", newHeader, RegexOptions.Singleline | RegexOptions.IgnoreCase);
    }

    // Pulizia doppioni nel corpo
    string duplicateH1Pattern = $@"(?!<header[^>]*>.*?)(?:<h1[^>]*>\s*" + Regex.Escape(articleTitle) + @"\s*<\/h1>)";
    updatedContent = Regex.Replace(updatedContent, duplicateH1Pattern, "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

    File.WriteAllText(filePath, updatedContent);
    Console.WriteLine($"[Stile Definitivo Applicato] {fileName}");
    updatedCount++;
}

Console.WriteLine($"\nMotore aggiornato e Avatar M ELIMINATO per sempre. Totale file riscritti: {updatedCount}");