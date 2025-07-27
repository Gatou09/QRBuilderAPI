# QRBuilderAPI

A simple and secure RESTful API to generate personalized QR codes.
It allows authenticated users to create a QR code from a text or a URL, with customizable colors.
A small web frontend accompanies the API to test the functionality easily.

---

## Features

-  Generation of QR Code from text or URLs
-  Customization of colors (foreground and background)
-  Secure JWT authentication
-  Built-in Swagger documentation
-  Minimal HTML/CSS/JS frontend (in `wwwroot/qr.html`)

---

## Technologies used

- ASP.NET Core 8
- QRCoder + SkiaSharp for graphics generation
- JWT for authentication
- HTML + JS + CSS for the frontend POC (no framework required)

---

## Structure of the project
```
QRBuilderAPI/
├── Controllers/
├── Models/
├── Services/
└── wwwroot/
    ├── qrcodes/ 
    ├── qr.html
        ├── css/style.css
        └── js/app.js
```

---

## Starting the backend

1. Clone or open the project in Visual Studio / Rider
2. Launch the project:
   dotnet run
3. Swagger will be available here:
   https://localhost:7093/swagger
   or
   http://localhost:5181/swagger
---

## Starting the frontend

> No installation required.

- Open in your browser:
  https://localhost:7093/qr.html
  or
  http://localhost:5181/qr.html
  
Do not open the file `qr.html` directly from the disk (`file://>) — it must go through the server. NET for the API calls to work.

---

## Test credentials

You can log in with this pre-registered account:

Email : test@qr.io  
Password: Test123!

---

## QR request example

POST /qr/generate

{
  "type": "url",
  "value": "https://github.com/Gatou09/QRBuilderAPI",
  "color": "#000000",
  "backgroundColor": "#ffffff"
}

---

## Security

- All sensitive routes (such as `/qr/generate`) are protected by a JWT token
- Passwords are temporarily stored in memory only
- Swagger supports authentication via "Authorize"

---

## Author

Project carried out by **Gaétan Gomez** as part of the WebAPI 2025 evaluation
