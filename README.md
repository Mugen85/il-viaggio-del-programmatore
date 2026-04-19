# 🚀 Il Viaggio del Programmatore

Benvenuto nel repository del mio sito personale e laboratorio digitale: **[Il Viaggio del Programmatore](https://www.ilviaggiodelprogrammatore.com)**.

Questo spazio non è solo una vetrina, ma la dimostrazione concreta di come affronto e risolvo i problemi quotidiani nello sviluppo software, unendo la flessibilità del web alla potenza dell'ecosistema **.NET e C#**.

---

## 👋 Chi sono

Mi chiamo Marco Morello, sono uno sviluppatore software e aspirante architetto software specializzato nell'ecosistema **Microsoft .NET**. 

Il mio approccio al codice è pragmatico e orientato al risultato: scrivo soluzioni pulite, manutenibili e pensate per portare **valore reale al business**. Sono una persona normale ma con grandi ambizioni, abituato a rimboccarmi le maniche per superare le sfide tecniche.

💼 **Cerchi un collaboratore freelance?**
Se cerchi un professionista serio, motivato e affamato di crescita per il tuo prossimo progetto .NET, o per portare online la tua attività con un sito su misura, sei nel posto giusto. [Contattami sul sito!](https://www.ilviaggiodelprogrammatore.com)

---

## 🛠️ Il Progetto e l'Architettura

L'architettura del sito segue una filosofia volutamente minimalista ed efficiente. Il frontend è scritto in puro **HTML, CSS (Bootstrap) e JavaScript**, per garantire performance massime, un controllo totale sull'UX e un'ottimizzazione SEO perfetta.

Tuttavia, dietro le quinte, sfrutto la flessibilità di **C# e .NET** per automatizzare i processi e mantenere il sito in salute. Un esempio di questo è il mio **SitemapBuilder**, incluso in questo repository.

### 🏍️ Case Study: SitemapBuilder

Come dimostrazione del mio modo di lavorare, ho sviluppato un tool interno per risolvere un problema concreto di manutenzione.

**Il problema (la diagnosi):** 
Gestire a mano i tag `<lastmod>` in una sitemap di un sito statico porta inevitabilmente a debito tecnico e dimenticanze. Un `<lastmod>` non aggiornato "ingolfa" il crawler di Google, causando drastici cali di indicizzazione.

**La soluzione (l'intervento):** 
Ho creato una Console App in **.NET** che agisce in locale prima del deploy:
1. **Scansiona** la directory alla ricerca di tutti i file `.html`.
2. **Estrae** la data reale di ultima modifica dal file system (OS).
3. **Genera (o sovrascrive)** un file `sitemap.xml` validato secondo lo standard sitemaps.org, eliminando metadati deprecati.

**Dettagli Tecnici & Problem Solving:**
- **Tecnologia:** Sviluppato in **.NET 8 / 9**.
- **Fix architetturali:** Ho implementato la direttiva `<UseAppHost>false</UseAppHost>` nel `.csproj` per prevenire eccezioni `IOException` dovute al blocco in lettura/scrittura degli eseguibili da parte dei demoni cloud (come Google Drive) durante la build.
- **Sicurezza:** Il tool è eseguito rigorosamente nel flusso di lavoro locale, mantenendo pulito e sicuro l'ambiente di produzione.

---

## 🤝 Lavoriamo Insieme

Hai un'idea in mente o hai bisogno di scalare un'applicazione esistente? Ecco come posso aiutarti:
- **Sviluppo Backend (.NET / C#)**: Creazione di API robuste, architetture scalabili e automazione di processi aziendali.
- **Siti Web su Misura**: Presenze online veloci, responsive e curate nei minimi dettagli (come questo sito).
- **Risoluzione Problemi**: Diagnosi, refactoring e ottimizzazione di codebase esistenti.

📬 **Ti va di parlarne?** 
Visita la sezione [Chi Sono](https://www.ilviaggiodelprogrammatore.com/chisono.html) sul mio sito, oppure scrivimi su LinkedIn. Non vedo l'ora di scoprire il tuo prossimo progetto!

---
*Il codice è uno strumento. Risolvere problemi è il vero mestiere.*
