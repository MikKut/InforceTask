# InforceTask

## INFORCE .NET TASK

### TECHNICAL STACK

- ASP.NET MVC (Framework or Core)
- Angular (Latest version), React
- EntityFramework CodeFirst approach - Any Framework for Unit tests ACTUAL

### TASK

We need to build URL Shortener. The goals are to create an application that will shorten
any URL and to have possibility to navigate by this short equivalent.
There are such views: Login view, Short URLs Table view, Short URL Info view, About
view. Adding some Unit tests will be a huge plus.
#### LOGIN VIEW
On the Login View you should be able to enter Login, Password and Authorize yourself.
You need to have Admin and ordinary users.
#### SHORT URLS TABLE VIEW
This page has table with all URLs and their equivalent after shortening, deleting of the
records is possible(only authorized users). You can view details about these URLs by
navigating to Short URL Info view with correct Id.
Also it has "Add new Url" section, which is visible only to authorized users, where you can
enter URL and convert it to a short representation, after that it should be added to the
table automatically.
If such a URL already exists - error message should be shown.
Every authorized user can add new records("Add new Url" section) and view(redirects to
the Short URL Info view), delete records created by themselves (URLs should be unique).
Admin users can add new records("Add new Url" section) and view(redirects to the Short
URL Info view), delete all existing records. Anonymous users can only see this table.
All changes should be visible right after they are done without reloading the page.
This page should be implemented with Angular.
Short URL Info (Anonymous can't access this page)
This page contains info about URL (CreatedBy, CreatedDate, any other fields).
#### About view
This should contain a description of your Url Shortener algorithm. Visible for
everyone(even not authorize) but can be edited only by admin users. Just an ordinary
Razor page with submit action.
Guidelines
Only spend up to two days working on this task.
If we have a technical interview, and I hope we do, we will focus on enhancing this application
and discussing how you worked through some of these problems. It's important that we see your
best work, if that means that you do not satisfy all of the requirements here. That is okay, we
don't expect everyone to finish all parts at time.
If you have to choose between refactoring and making one piece of this perfect and
implementing the next feature, choose refactoring because we want to see how your best
work looks.
We want to see clear, correct code that uses the suitable data structures and patterns,
and we want to see your style.

# Architectural Overview
There will be created a web application with a server-side component and a client-side component. The server-side component will be built using ASP.NET MVC Core with EntityFramework, and the client-side component will be built using Angular.

## Server-Side Architecture
The server-side component will follow a standard MVC pattern:

### Model: There will be used Entity Framework (Code-First Approach) to define your data models. The primary models will be User, URL, and About.
Controller: There will be multiple controllers such as UserController, URLController, and AboutController.
### View: There will be used Razor for some views, but most of your views will be rendered by Angular on the client-side.

## Client-Side Architecture
On the client-side, Angular will consume the server-side APIs to provide interactivity:

### Model: Models for User, URL, and About on client-side will mirror server-side models.
### Component: There will be Angular components corresponding to each view. The AppComponent will be a root component.
### Service: Angular services will be used to communicate with your server-side APIs.

## Design Patterns
You will use the following patterns in your application:

### Repository Pattern: This pattern will be used for all database operations. It provides a clean separation of concerns and a uniform interface for data access.
### Unit of Work Pattern: This pattern will be used to handle transactions.
### Dependency Injection (DI): DI will be used for managing dependencies between objects.

## Key Features
### URL Shortening
The core feature of the application is URL shortening. The simplest way to implement this is by using a base62 encoding algorithm. This converts the URL's ID (an integer) into a base62 string, which is used as the short URL.

### URL Redirect
When a user visits a short URL, the application will decode the base62 string back into an ID, look up the corresponding URL in the database, and then redirect the user to that URL.

### URL Info
Each URL will have associated data like CreatedBy, CreatedDate, and any other fields you choose to include. This data will be stored in the URL table in the database.

## Unit Testing
There will be used a .NET test framework xUnit for server-side unit tests.

## Application Flow
User arrives at the Login View, enters credentials, and authorizes themselves.
If the user is valid, they are redirected to the Short URLs Table view.
On the Short URLs Table view, the user can see a table with all URLs and their short equivalents. They can also add a new URL (if authorized).
If a user enters a URL that already exists, an error message is displayed.
After a URL is added, it appears in the table immediately (implemented through Angular).
By clicking on a short URL in the table, the user is redirected to the Short URL Info view.
On the About view, the user can see a description of the URL Shortening algorithm. If the user is an admin, they can edit this description.
