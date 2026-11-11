using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

string rootDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), ".."));
string[] htmlFiles = Directory.GetFiles(rootDir, "*.html", SearchOption.AllDirectories);

// Messa in sicurezza 1: UTF8 pulito
var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
int updatedCount = 0;

foreach (var filePath in htmlFiles)
{
    string fileName = Path.GetFileName(filePath);
    
    // Ignoriamo file di sistema e hub
    string[] ignoredFiles = { "header.html", "footer.html", "index.html", "chisono.html", "risorse.html", "blog.html", "mindset.html", "development.html", "glossario-csharp.html", "risorse-anki-csharp.html" };
    if (Array.Exists(ignoredFiles, name => name.Equals(fileName, StringComparison.OrdinalIgnoreCase))) continue;
    if (filePath.Contains("BlogRefactorTool", StringComparison.OrdinalIgnoreCase)) continue;
    if (!filePath.Contains("blog" + Path.DirectorySeparatorChar)) continue;

    string content = File.ReadAllText(filePath, encoding);
    
    // --- 1. ESTRAZIONE DATI ---
    Match titleTagMatch = Regex.Match(content, @"<title>(.*?)<\/title>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
    string articleTitle = "";
    if (titleTagMatch.Success)
    {
        articleTitle = titleTagMatch.Groups[1].Value;
        articleTitle = Regex.Replace(articleTitle, @"\s*[-–—]\s*Il Viaggio del Programmatore.*$", "", RegexOptions.IgnoreCase).Trim();
    }
    if (string.IsNullOrEmpty(articleTitle)) articleTitle = Path.GetFileNameWithoutExtension(fileName).Replace("-", " ");

    Match dateMatch = Regex.Match(content, @"(?:(\d{1,2}\s+[^\d<]+\s+\d{4})|([A-Za-z]+\s+\d{1,2},\s+\d{4}))");
    string dateString = dateMatch.Success ? dateMatch.Value : "Ottobre 2026";

    string pureText = Regex.Replace(content, @"<[^>]*>", "");
    int wordCount = pureText.Split(new char[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
    int readTime = Math.Max(1, wordCount / 200);

    bool isMindset = filePath.Contains("mindset", StringComparison.OrdinalIgnoreCase);
    string categoryClass = isMindset ? "mindset" : "development";
    string badgeText = isMindset ? "Mindset & Vita" : "Sviluppo Software";
    string iconClass = isMindset ? "bi-compass" : "bi-braces-asterisk";

    // Calcolo URL Canonico Esatto
    string relativePath = filePath.Substring(rootDir.Length).Replace("\\", "/").TrimStart('/');
    string canonicalUrl = $"https://www.ilviaggiodelprogrammatore.com/{relativePath}";

    // --- 2. HEADER BODY ---
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

    // --- 3. COSTRUZIONE TAG SEO HEAD ---
    string seoTags = $@"
    <title>{articleTitle} | Il Viaggio del Programmatore</title>
    <link rel=""canonical"" href=""{canonicalUrl}"">
    <meta property=""og:title"" content=""{articleTitle} | Il Viaggio del Programmatore"">
    <meta property=""og:url"" content=""{canonicalUrl}"">
    <meta property=""og:type"" content=""article"">";

    // --- 4. SOSTITUZIONI ---
    string updatedContent = content;

    // Body
    if (Regex.IsMatch(updatedContent, @"<header[^>]*class=""[^""]*(?:article-hero|blog-header)[^""]*""[^>]*>.*?<\/header>", RegexOptions.Singleline | RegexOptions.IgnoreCase))
    {
        updatedContent = Regex.Replace(updatedContent, @"<header[^>]*class=""[^""]*(?:article-hero|blog-header)[^""]*""[^>]*>.*?<\/header>", newHeader, RegexOptions.Singleline | RegexOptions.IgnoreCase);
    }
    else
    {
        updatedContent = Regex.Replace(updatedContent, @"<header\b[^>]*>.*?<\/header>", newHeader, RegexOptions.Singleline | RegexOptions.IgnoreCase);
    }

    // H1
    string duplicateH1Pattern = $@"(?!<header[^>]*>.*?)(?:<h1[^>]*>\s*" + Regex.Escape(articleTitle) + @"\s*<\/h1>)";
    updatedContent = Regex.Replace(updatedContent, duplicateH1Pattern, "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

    // Head (rimozione vecchi tag per evitare duplicati e iniezione nuovi prima di </head>)
    updatedContent = Regex.Replace(updatedContent, @"<title>.*?<\/title>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
    updatedContent = Regex.Replace(updatedContent, @"<link\s+rel=""canonical""[^>]*>", "", RegexOptions.IgnoreCase);
    updatedContent = Regex.Replace(updatedContent, @"<meta\s+property=""og:(title|url|type)""[^>]*>", "", RegexOptions.IgnoreCase);
    
    updatedContent = Regex.Replace(updatedContent, @"<\/head>", seoTags + "\n</head>", RegexOptions.IgnoreCase);

    // --- 5. SALVATAGGIO CON FAIL-SAFE ---
    if (updatedContent != content)
    {
        string backupPath = filePath + ".bak";
        File.Copy(filePath, backupPath, overwrite: true);
        File.WriteAllText(filePath, updatedContent, encoding);
        Console.WriteLine($"[Stile & SEO Inject] {fileName}");
        updatedCount++;
    }
}

Console.WriteLine($"\nMotore aggiornato. Totale file riscritti: {updatedCount}");