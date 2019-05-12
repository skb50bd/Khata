# KHATA - simple, smart, powerful

## Introduction

**KHATA** is a lightweight ERP solution for small to medium sized businesses. *Khata* helps you with a lot of business interactions.

It is built with industry leading technologies. The project is mainly developed in `C#` and other `.Net` technologies.

*Khata* has a beautiful user interface, created with modern web technologies. It now has server side rendering with `ASP.Net Core Razor Pages`, so that you can enjoy a snappy application experience from any device, be it a desktop or a smart-phone.  

## Getting Started

To server *Khata* locally, you must have `dotnet` installed in your machine. **.Net** is cross-platform, so is *Khata*. You can run *Khata* wherever _.Net_ runs.

You can install the latest version (2.2 or later) of `dotnet` from [here](https://dotnet.microsoft.com/download).

Once you have `dotnet` installed, you can download / clone the repository into you local machine and use it. One thing to be aware, *Khata* uses a database. Currently, it supports _SQL Server_, _PostgreSQL_, and _SQLite_. Depending on your situation, update the `appsettings.json` to specify your chosen data-provider.

Now, you can `cd` into the `WebUI` project directory and, run `dotnet run` to spin up the web server. The server should start listening to [`localhost:5000`](http://localhost:5000), and you can open a browser and hit the server.

### Notes

You should check the `appsettings.json` file, if you are having trouble using the application. There are some directory information that needs to be changed for different environments.

If you have done everything right, you will see something like this: 
![home-page][home-page]

And now you can login with the default admin with username: `brotal`, password: `Pwd+123`.

Okay Have fun...

[home-page]: ./screenshots/home-page.png
