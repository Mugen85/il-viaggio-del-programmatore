using System;
using System.IO;
using System.Text.RegularExpressions;

string rootDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), ".."));
string[] htmlFiles = Directory.GetFiles(rootDir, "*.html", SearchOption.AllDirectories);

int updatedCount = 0;

foreach (var filePath in htmlFiles)
{
    // Filtro più largo per non saltare nessuna sottocartella (es. mindset-section)
    if (filePath.Contains("BlogRefactorTool", StringComparison.OrdinalIgnoreCase)) continue;
    if (!filePath.Contains("blog", StringComparison.OrdinalIgnoreCase)) continue;

    string content = File.ReadAllText(filePath);

    // Regex spietata: trova qualsiasi <div> che abbia la classe "author-avatar", a prescindere da spazi, a capo o cosa c'è dentro.
    string pattern = @"<div[^>]*class\s*=\s*[""']author-avatar[""'][^>]*>.*?<\/div>";

    if (Regex.IsMatch(content, pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase))
    {
        string updatedContent = Regex.Replace(content, pattern, @"<i class=""bi bi-person-bounding-box fs-5""></i>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        File.WriteAllText(filePath, updatedContent);
        Console.WriteLine($"[TAGLIATA] M rimossa da: {Path.GetFileName(filePath)}");
        updatedCount++;
    }
    // Diagnostica: se la parola author-avatar c'è, ma la regex non la cattura
    else if (content.Contains("author-avatar"))
    {
        Console.WriteLine($"[ANOMALIA] {Path.GetFileName(filePath)} contiene 'author-avatar' ma ha una struttura sconosciuta.");
    }
}

Console.WriteLine($"\nFine lavoro. Avatar rimossi da {updatedCount} file.");