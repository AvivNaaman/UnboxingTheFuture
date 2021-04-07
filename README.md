# Unboxing The Future - Registration website

![.NET Core](https://github.com/avivnaaman/UnboxingTheFuture/workflows/.NET%20Core/badge.svg?branch=master)

This repo is an official release repo of the "Unboxing the future" conference registration website. Database is empty due to [State of Israel's Privacy Laws](https://www.knesset.gov.il/review/data/heb/law/kns9_privacy.pdf).

## Setting Up & Starting
1. Make sure you have the apprpriate .NET Core version (3.1LTS) to start the application
2. Change your administrator settings on `appsettings.json`:`AppConfig`:`DefaultAdminUser`
3. Start using cli using `dotnet run --project AtidRegister` or using VS using F5

## Techonlogies, Tools & Libraries
* ASP.NET Core MVC
* EF Core for Data access
* Chart.js for Admin panel data visualization
* Bootstrap for Layout, Styling & Components
* Microsoft ASP.NET Core Identity Provider
* BuildBundlerMinifier for Assets build & Minifcation
* Twilio for Bulk SMS Messgaing (Not Complete)

## Features
[All Images](https://imgur.com/a/C5bFqEN)
### Login Page
Beautfiul-looking login page, sorted by class and grade for each student.
![Login Page](https://i.imgur.com/IWr6lNF.png)
### Registration Process
Prioritization of lectures, multiple choice option and minimum selection amount.
![Registration Page](https://imgur.com/WRAhHsw.png)
### Phone Number collection
Collection of phone numbers at the end of the registration process, if not in existence, yet.
![Phone number input form](https://imgur.com/9HaojKJ.png)
### Q&A
Dynamic, Administrator-defined Questions & Answers page.
![Questions and answers view](https://imgur.com/HstiEkx.png)
### Admin panel
Advanced admin panel, featuring options such as full data export, statistics visualization using Chart.js, Bulk user creation using CSV files, Dynamic Contents, People, Time Strips & Categories Editor, Single user creation (admin/student) and more.. 
![Admin statistics dashboard](https://imgur.com/1Lcw9zP.png)
