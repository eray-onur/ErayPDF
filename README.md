# ErayPDF
This project is an attempt to provide an alternative way to convert plain HTML files to PDF without the usage of QT Webkit.
### F.A.Q
#### Is the project production-ready?
No. Conversion process is too hard-coded and inflexible to be used in a production project unless the requirement is generating a PDF with A4 page dimensions. Won't take long before it gets ready for general use cases, however.

#### Does this converter support latest HTML5/CSS3 features?
Yes.
#### What is the caveat?
A modern browser is REQUIRED for this library to function. Ancient browsers such as Internet Explorer is not supported, and will NEVER be supported in the future.
```
Html2PdfConverter.ConvertToBytes()
```
```
Html2PdfConverter.ConvertToMemoryStream()
```