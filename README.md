# ErayPDF
This project is an attempt to provide an alternative way to convert plain HTML files to PDF without relying on anything that utilizes QT Webkit (looking at you, WK).
<hr>

### F.A.Q

<hr>

#### When does ErayPDF become a valid choice for a PDF generation strategy?

* When there is a requirement to generate PDF from an html file or string,
* Projects that utilize WK for PDF generation will benefit from modern styling syntax support (ie. Flexbox, Grid, Hexcodes for colors),
* Stability due to using an embedded browser for PDF generation. In local environment, browser versions or even browsers themselves can change which makes webdrivers useless until update.

#### Supported Operating Systems

OS | Windows 10 | Ubuntu 20.04 | OSX
--- | --- | --- | ---
Supported | YES | NOT CONFIRMED | NOT CONFIRMED

#### Does this converter support latest HTML5/CSS3 features?
Absolutely. As ErayPDF will work with any version of chromium and chromedriver (as long as they are the same version), every bleeding edge HTML/CSS features will be supported.

#### What is the caveat?
A Chromium binary MUST be added from https://chromium.woolyss.com/ to the Binaries directory at root path or printing won't work! Couldn't get it into project as it'd turn the repo to LFS, sorry.
As ErayPDF depends on Chromium 102, a portable browser, any project that utilizes it will face an overhead of over ~200 MBs in file size.
Also, "chrome_driver.exe" file must be added right inside the chromium binaries (at the same directory level as "chrome.exe") or document builder will fail.
Finally, chrome driver and chromium must be the same version (ex. chromium version 102, chromedriver version 102).
<hr>

#### Usage Examples

<hr>

#### Returning generated PDF's persisted path:

```
    string createdPdfPath = new DocumentBuilder().FromFilePath(htmlPath).AsFilePath("example");
```

#### Returning generated PDF as a byte array:

```
    byte[] createdPdfBytes = new DocumentBuilder().FromHtmlContent("<html></html>").Result.AsBinary();
```

#### Returning generated PDF as a byte array, while persisting the generated HTML file:

```
    byte[] createdPdfBytes = new DocumentBuilder().FromHtmlContent("<html></html>", true).Result.AsBinary();
```

#### Returning generated PDF as a base64 string:

```
    string createdPdfBase64 = new DocumentBuilder().FromFilePath(htmlPath).AsBase64String();
```


            

            
