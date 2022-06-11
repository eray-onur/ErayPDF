# ErayPDF
This project is an attempt to provide an alternative way to convert plain HTML files to PDF without relying on anything that utilizes QT Webkit (looking at you, WK).
<hr>

### F.A.Q

<hr>

#### Is the project production-ready?

Only for A4-sized prints. Multiple size support is still a work in progress.

#### Does this converter support latest HTML5/CSS3 features?
Absolutely.
#### What is the caveat?
a Chromium binary MUST be added from https://chromium.woolyss.com/ to the Binaries directory at root path or printing won't work! Couldn't get it into project as it'd turn the repo to LFS, sorry.
As ErayPDF depends on Chromium 102, a portable browser, any project that utilizes it will face an overhead of over ~200 MBs in file size.
<hr>

#### Usage Examples
<hr>
##### Returning generated PDF's persisted path:
```
    string createdPdfPath = new DocumentBuilder().FromFilePath(htmlPath).AsFilePath("example");
```
##### Returning generated PDF as a byte array.
```
    byte[] createdPdfBytes = docBuilder.FromHtmlContent("<html></html>").Result.AsBinary();
```
##### Returning generated PDF as a byte array, while persisting the generated HTML file.
```
    byte[] createdPdfBytes = docBuilder.FromHtmlContent("<html></html>", true).Result.AsBinary();
```
##### Returning generated PDF as a base64 string.
```
    string createdPdfBase64 = docBuilder.FromFilePath(htmlPath).AsBase64String();
```


            

            
