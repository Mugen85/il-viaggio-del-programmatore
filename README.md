# **SitemapBuilder 🏍️**

Tool da officina in C\# (.NET) progettato per la generazione e manutenzione automatica della sitemap.xml del sito statico *Il Viaggio del Programmatore*.

## **Il problema (la diagnosi)**

Gestire a mano i tag \<lastmod\> in una sitemap di un sito in HTML puro (senza CMS) porta inevitabilmente a debito tecnico e dimenticanze. Un \<lastmod\> non aggiornato "ingolfa" il crawler di Google (Googlebot), che ignora le pagine modificate credendole vecchie, causando drastici cali di indicizzazione.

## **La soluzione (l'intervento)**

Questa Console App automatizza il processo. Agisce in locale prima del deploy:

1. Scansiona la directory padre alla ricerca di tutti i file .html.  
2. Legge la **data reale di ultima modifica** direttamente dal file system (OS).  
3. Genera (o sovrascrive) un file sitemap.xml validato secondo lo standard sitemaps.org, eliminando metadati inutili o deprecati (\<priority\>, \<changefreq\>).

## **Come si usa**

Il tool è pensato per essere eseguito come step di pre-rilascio nel flusso di lavoro locale.

1. Apri il terminale all'interno di questa cartella (SitemapBuilder).  
2. Avvia il motore con la .NET CLI:  
   dotnet run

3. Verifica l'output in console. Il file sitemap.xml aggiornato sarà disponibile nella root del sito, pronto per l'upload via FTP.

## **Architettura & fix noti**

* **Framework:** .NET 8 / 9 (Console Application).  
* **Google Drive Sync Lock:** Nel file .csproj è stata inserita la direttiva \<UseAppHost\>false\</UseAppHost\>. Questo previene le eccezioni IOException dovute al blocco in lettura/scrittura degli eseguibili da parte dei demoni di sincronizzazione cloud (come Google Drive) durante la fase di build.  
* **Sicurezza:** Questo tool sorgente rimane nel repository locale. **NON** deve essere esposto in produzione sul server web pubblico.
