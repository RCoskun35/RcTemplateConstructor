# RcTemplate
## This project was prepared with ASP .Net Core 7.0 and aims to provide a basic outline with Onion Architecture

This outline includes fundamental structures that may be needed when starting a project, along with examples. The general structure is as follows:
## Features
Solution
-- Domain
--Application
--Infrastructure
--Persistence
--API
--Web
--ConsoleUtility
--GrpcServer
--GrpcClient
 - The project utilizes Microsoft.Identity and JWT for Authentication and Authorization processes.
- Validation is done with FluentValidation and mapping is done with AutoMapper.
-  SignalR API and Web communication are possible.
-  Background tasks can be executed with Hangfire.
-  Role-based authorization is implemented. Claims are created and assigned to roles within the application, allowing for extensive authorization based on roles. Operations like Add, Update, Delete, View are role-based.

Entities are stored in Domain.
Features in Application include:
Interfaces IAuthenticationService and ITokenService responsible for token generation and authentication processes.
Modules are created with Permissions.
SignalR messages are handled with Hub.
Multilingual structure is established with Language. Simply creating a JSON file for required languages is sufficient.
Interfaces of classes responsible for database operations are stored in Repositories.
Shared infrastructure is set up to provide common responses to requests.
Static Services include the following services:
-- ClaimApiService for basic information of API users (Name, Surname, Roles, etc.),
-- ClaimService for basic information of web users (Name, Surname, Roles, etc.),
-- ErrorService for capturing validation errors or any other errors,
-- GeneralService includes methods for Assembly required by HashService, reading from JSON file if necessary,
-- HashService for encrypting and decrypting data,
-- LanguageService for translations of defined keys in selected languages,
-- LogService for writing information or error logs to a text file,
Validators folder contains validation rules for classes.
ViewModels folder contains parameters requested from users or return values to users.
Web endpoint configurations are stored in Configuration class.
Infrastructure may include services like MailService, SmsService. However, they are not present in this project.
Features in Persistence include:
DbContext
CreateDbObjects creates database objects like stored procedures, functions, triggers when the project starts.
Permission executes authentication operations of the application.
Repository executes database operations. It includes methods that return generic lists without the need for DbSet<T> apart from general operations.
Seeds add necessary data to the database when the application starts.
Services include methods responsible for token generation and authentication.
Configuration decrypts the encrypted connection information stored in appsettings.json file.
Necessary services are registered with ServiceRegistration.
Features in Web include:
SmartAdmin template is pre-installed.
AuthorizationFilter for Hangfire Dashboard is available.
Features in API include:
Example of AutoMapper.
Example of general error handling.
gRPC methods are present in GrpcServer. There are methods for generating tokens and methods protected by Authorize Filter.
Example methods are present in GrpcClient to request token and access methods protected by Authorize Filter.
ConsoleUtility includes example methods for encryption and decryption with hashservice. You can start operations by typing "rctemplate"
