# RcTemplate
## This project was prepared with ASP .Net Core 7.0 and aims to provide a basic outline with Onion Architecture

This outline includes fundamental structures that may be needed when starting a project, along with examples. The general structure is as follows:
## Features
Solution<br>
-- Domain<br>
--Application<br>
--Infrastructure<br>
--Persistence<br>
--API<br>
--Web<br>
--ConsoleUtility<br>
--GrpcServer<br>
--GrpcClient<br>
 - The project utilizes Microsoft.Identity and JWT for Authentication and Authorization processes.<br>
- Validation is done with FluentValidation and mapping is done with AutoMapper.<br>
-  SignalR API and Web communication are possible.<br>
-  Background tasks can be executed with Hangfire.<br>
-  Role-based authorization is implemented. Claims are created and assigned to roles within the application, allowing for extensive authorization based on roles. Operations like Add, Update, Delete, View are role-based.<br>

Entities are stored in Domain.<br>
Features in Application include:<br>
Interfaces IAuthenticationService and ITokenService responsible for token generation and authentication processes.<br>
Modules are created with Permissions.<br>
SignalR messages are handled with Hub.<br>
Multilingual structure is established with Language. Simply creating a JSON file for required languages is sufficient.<br>
Interfaces of classes responsible for database operations are stored in Repositories.<br>
Shared infrastructure is set up to provide common responses to requests.<br>
Static Services include the following services:<br>
-- ClaimApiService for basic information of API users (Name, Surname, Roles, etc.),<br>
-- ClaimService for basic information of web users (Name, Surname, Roles, etc.),<br>
-- ErrorService for capturing validation errors or any other errors,<br>
-- GeneralService includes methods for Assembly required by HashService, reading from JSON file if necessary,<br>
-- HashService for encrypting and decrypting data,<br>
-- LanguageService for translations of defined keys in selected languages,<br>
-- LogService for writing information or error logs to a text file,<br>
Validators folder contains validation rules for classes.<br>
ViewModels folder contains parameters requested from users or return values to users.<br>
Web endpoint configurations are stored in Configuration class.<br>
Infrastructure may include services like MailService, SmsService. However, they are not present in this project.<br>
Features in Persistence include:<br>
DbContext<br>
CreateDbObjects creates database objects like stored procedures, functions, triggers when the project starts.<br>
Permission executes authentication operations of the application.<br>
Repository executes database operations. It includes methods that return generic lists without the need for DbSet<T> apart from general operations.<br>
Seeds add necessary data to the database when the application starts.<br>
Services include methods responsible for token generation and authentication.<br>
Configuration decrypts the encrypted connection information stored in appsettings.json file.<br>
Necessary services are registered with ServiceRegistration.<br>
Features in Web include:<br>
SmartAdmin template is pre-installed.<br>
AuthorizationFilter for Hangfire Dashboard is available.<br>
Features in API include:<br>
Example of AutoMapper.<br>
Example of general error handling.<br>
gRPC methods are present in GrpcServer. There are methods for generating tokens and methods protected by Authorize Filter.<br>
Example methods are present in GrpcClient to request token and access methods protected by Authorize Filter.<br>
ConsoleUtility includes example methods for encryption and decryption with hashservice. You can start operations<br> by typing "rctemplate"
