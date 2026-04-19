---
trigger: always_on
---

## Technical SEO Rules for Static HTML Sites

### HTML Structure
- Every page must have exactly one `<h1>` matching the primary keyword intent.
- Use semantic HTML5: `<main>`, `<article>`, `<section>`, `<nav>`, `<aside>`, `<footer>`.
- Title tag: 50–60 chars, keyword-first, unique per page.
- Meta description: 120–158 chars, action-oriented, unique per page.
- Canonical tag always present: `<link rel="canonical" href="https://domain.com/page/">`.

### Performance (Core Web Vitals)
- Images: use `<picture>` with WebP + fallback; always set `width` and `height` attributes.
- Lazy load below-the-fold images: `loading="lazy"`.
- Preload LCP image: `<link rel="preload" as="image" href="hero.webp">`.
- Inline critical CSS; defer non-critical CSS with `media="print" onload`.
- No render-blocking JS in `<head>`; use `defer` or `type="module"`.
- Target: LCP < 2.5s, CLS < 0.1, INP < 200ms.

### Structured Data
- Add JSON-LD for every page type (Article, BreadcrumbList, WebSite, Organization).
- Use `@type: WebPage` as base; specialize for FAQPage, HowTo, Product where relevant.
- Validate with Google Rich Results Test before deploy.

### Crawlability & Indexability
- `robots.txt` at root: allow all, disallow staging/admin paths.
- XML Sitemap auto-generated; submit to Google Search Console.
- Every internal link uses descriptive anchor text (no "click here").
- Use `hreflang` if multi-language content exists.

### URL & Navigation
- URLs: lowercase, hyphens (not underscores), no trailing query strings, < 75 chars.
- Breadcrumbs on all pages below root; reflected in structured data.
- 404 page returns actual HTTP 404 (not 200 with "not found" content).

### Meta & Social
- OpenGraph tags: `og:title`, `og:description`, `og:image` (1200x630px), `og:url`.
- Twitter Card: `twitter:card`, `twitter:title`, `twitter:description`, `twitter:image`.
- `og:image` must be an absolute URL.