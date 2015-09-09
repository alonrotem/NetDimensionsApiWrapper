# NetDimensions API Wrapper
This is an initial implementation of a .NET wrapper for <a href="http://www.netdimensions.com/talent-management-suite/" target="_blank">NetDimensions's Talent Suite</a> main API functions.
To learn more, see <a href="https://wiki.netdimensions.com" target="_blank">NetDimensions Wiki</a> (requires an account).

## TOC
In this Readme:
- [About the wrapper](#about-the-wrapper)
  - [NuGet References](#nuget-references)
  - [Adding the wrapper to your solution](#adding-the-wrapper-to-your-solution)
- [Wrapper supported methods](#wrapper-supported-methods)
  - [Construction and initialization](#construction-and-initialization) 
  - [User methods](#user-methods)
  - [Organization methods](#organization-methods)
  - [User/organization methods](#userorganization-methods)
- [User fields](#user-fields)

## About the wrapper
Working with the wrapper is straightforward, as it sends/returns objects of the types represented in the related C# classes. However, it unifies work against different underlying API methods, which require different formats (some JSON, some XML of different schemes), different authentication modes (some with a user and password, some with a system key) etc.

It was built and tested with <a href="http://www.netdimensions.com/talent-management-suite/" target="_blank">NetDimensions Talent Suite</a> version 10.3.0.693 STANDARD.

### NuGet References
This wrapper references <a href="http://www.nuget.org/packages/Newtonsoft.Json/" target="_blank">Newtonsoft.Json</a>, which should be auto-restored and added to the project at build-time (See [Adding the wrapper to your solution](#adding-the-wrapper-to-your-solution) below).

### Adding the wrapper to your solution
1. Add the NetDimensionsWrapper project to your solution.
1. Add a reference of the NetDimensionsWrapper project to your client project (web / console / Winforms / WPF / whateva application).
1. Build the solution. NuGet packages (<a href="http://www.nuget.org/packages/Newtonsoft.Json/" target="_blank">Newtonsoft.Json</a>) should be automatically restored. If not:   
 - In Visual Studio, click **View** on the main menu and select **Solution Explorer**
 - Right-click the solution and select **Enable NuGet Package Restore**.
 - Rebuild your solution.

[[^ Back to TOC](#toc)]

## Wrapper supported methods ##

### Construction and initialization
To initialize the connection with the NetDimensions LMS, an instance of the `NDWrapper` class must be initialized by calling the constructor method. In order to communicate with the LMS, you need to supply user credentials and system credentials. 

As stated in the NetDimensions Wiki article, <a href="https://wiki.netdimensions.com/confluence/display/ptk/Authentication+modes" target="_blank">Authentication modes</a>, different Talent Suite API methods require different levels of authentication:
- User authentication   
Invocation of user-authenticated methods require a username and a password of a sufficiently-privileged user.

- System authentication  
"*When calling a function that uses system authentication, the caller must supply a global system password ... The value of the password is configured in the `WEB-INF/conf/ekp.properties` configuration file using the `authentication.key` property.*" (from the Wiki).

**Also note** (from the wiki): "*When using **HTTP basic access authentication**, credentials are passed in what is essentially clear text*". This is the method used by default by the wrapper. 

To initialize a NDWrapper object, its constuctor requires 4 arguments, and all 4 need to be supplied (all are strings):
 - **userAuthUsername** and **userAuthPassword**  
 Those are used by the wrapper for API methods which require user authentication (see above).

 - **systemAuthKey**   
 This is used by the wrapper for API methods which require system authentication (see above).

 - **lmsBaseUrl**  
 This is the base URL where your NetDimensions Talent Suite is installed. Specific API service URLs are appended to it by the wrapper at runtime.

### User methods 

- **CreateUsers** (single or batch multiple)  
This function receives details of 1 or more user to create (each can contain details of its assigned organization).   
Note that some user's fields are mandatory for creating new users. Those are listed in the table below, [User fields](#user-fields). 
This function will:  
  - Create the users  
  - Create their assigned organizations, if those don’t exist  
  - Assign each user to their respective organization.

 
- **DeleteUsers** (single or batch multiple)  
This function receives 1 or more user IDs, and will delete all of them.

- **GetUser** (single)   
This function receives a user’s ID or an email address, and returns the user’s details (if found), including all its attributes and their assigned organization.   
Note that not all the user's fields are returned by the **GetUser** function. The ones which are returned are listed in the table below, [User fields](#user-fields). 

### Organization methods 
 - **CreateOrganization** (single)   
Given an organization hierarchy of codes and an optional description, this method creates an organization in the system.

- **DeleteOrganization** (single)   
Given an organization hierarchy of codes, this method deletes an organization from the system.

- **GetOrganization** (single)   
This method returns the basic data of an organization in the system (its code, ID, description and hierarchy or parents).

### User/organization methods 

- **AddUsersToOrganization** (single or batch multiple)   
Given one or more user IDs and an organization code, this method assigns users to the organization.

- **GetUsersInOrganization** (single)   
Given an organization code and optional user status (e.g. all the "active" users), it returns the IDs of the users in that organization.

[[^ Back to TOC](#toc)]

## User fields
Some of the fields are required when creating a new user, but mind you that not all the fields are returned by the API’s GetUser method (and not much can be done about this..).

Property for new/update | Required for new user | Returned with GetUser
---| :---: | :----:
UserID  |  Y  |   Y
FirstName  |  Y  |   Y
MiddleName  |   |   Y
LastName  |  Y  |   Y
OtherName  |   |   Y
Gender  |   |   
DateOfBirth  |   |   
Password  |  Y  |   
Status  |   |   Y
UseExternalAuthentication  |   |   
EmployeeNumber  |   |   Y
ExpirationDate  |   |   
Language  |   |   
Email  |   |   Y
JoinDate  |   |     
Organization  |   |   Y
PrimaryRole  |   |   Y
DirectAppraiserUserID  |   |   Y
DepartmentID  |   |   Y
DepartmentName  |   |   Y
HrManagerName  |   |   
HrManagerEmal  |   |   
ManagerName  |   |   
ManagerEmail  |   |   
CostCenter  |   |   Y
Skin  |   |   
EmploymentCountry  |   |   
Address1  |   |   Y
Address2  |   |   Y
City  |   |   Y
ProvinceState  |   |   Y
PostalCode  |   |   Y
Country  |   |   
Phone  |   |   Y
UserAttr1  |   |  Y  
UserAttr2  |   |  Y  
UserAttr3  |   |  Y  
UserAttr4  |   |  Y  
UserAttr5  |   |  Y  
UserAttr6  |   |  Y  
UserAttr7  |   |  Y  
UserAttr8  |   |  Y  

[[^ Back to TOC](#toc)]
