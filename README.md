# Library Application
> The application is designed to manage a small or medium-sized library. Created with ASP.NET.

## Table of contents
* [Status](#status)
* [General info](#general-info)
* [Technologies](#technologies)
* [Features](#features)
* [Screenshots](#screenshots)

## Status
The project is not yet completed. It is still under development.

## General info
The application is designed to enhance the operations of small to medium-sized libraries in an online environment. It allows users to browse the library catalog and simplifies the book lending process. The application allows for tracking the entire loan process, from the initiation of an order online, through the physical collection of the book at the library, to its eventual return. Users can also view their borrowing history and any potential fees incurred for overdue items or damages/loss of borrowed books.

Librarians confirm all lending and return transactions in the system and have access to information about overdue loans and corresponding fees.

The application is built in Clean Architecture and Service-Repository pattern.

## Technologies
* .NET 6.0
* ASP.NET, HTML5, CSS, MSSQL
* WebAPI
* Dependency Injection
* Entity Framework Core 6.0.10
* LINQ
* Fluent Validation 11.2.2
* AutoMapper 12.0.0
* Bootstrap 5.1

## Features
1. Book Catalog Management – CRUD operations for librarians.
2. Book Catalog Access – allows users to view the book catalog and add books to their loan basket.
3. Loan Management:
    - Users: can create, view, track, and cancel loans.
    - Librarians: can view, cancel and supervise the loan process (including confirmation of physical book check out and return, along with the creation of a protocol for returned/damaged books and associated fines).
4. User Management – librarians can view user profiles, loan history, total fines imposed and have ability to block the user from creating new loans.
5. User Account Creation and Editing – each user can manage their own account, which includes basic data and loan history.
6. Third-Party Authentication – users can create an account and log in with Google account authentication.
7. Role System – includes Reader, Librarian and Administrator roles.
8. User Role Management – administrators can manage user roles.

## Screenshots
### Home Page
![Home Page](/LibraryMVC.Web/wwwroot/screenshots/home_page.png)
### Login
![Login](/LibraryMVC.Web/wwwroot/screenshots/login_page.png)
### Registration
![Registration](/LibraryMVC.Web/wwwroot/screenshots/registration_page.png)
### Library Catalog
![Library catalog](/LibraryMVC.Web/wwwroot/screenshots/library_catalog.png)
### Catalog Management
![Catalog management](/LibraryMVC.Web/wwwroot/screenshots/catalog_management.png)
### User Details
![User Details](/LibraryMVC.Web/wwwroot/screenshots/user_details.png)
### Loan Check-Out Confirmation Page
![Loan Check-Out Confirmation Page](/LibraryMVC.Web/wwwroot/screenshots/loan_check_out_confirmation.png)
### Loan Return Confirmation Page
![Loan Return Confirmation Page](/LibraryMVC.Web/wwwroot/screenshots/loan_return_confirmation.png)
### Order Overview
![Order Overview](/LibraryMVC.Web/wwwroot/screenshots/order_overview.png)
