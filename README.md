# NetDimensions API Wrapper
This is an initial implementation of a .NET wrapper for <a href="http://www.netdimensions.com/talent-management-suite/" target="_blank">NetDimensions's Talent Suite</a> main API functions.
To learn more, see <a href="https://wiki.netdimensions.com" target="_blank">NetDimensions Wiki</a> (requires an account).

## About the wrapper
Working with the wrapper is straightforward, as it sends/returns objects of the types represented in the related C# classes. However, it unifies work against different underlying API methods, which require different formats (some JSON, some XML of different schemes), different authentication modes (some with a user and password, some with a system key) etc.

It was built and tested with <a href="http://www.netdimensions.com/talent-management-suite/" target="_blank">NetDimensions Talent Suite</a> version 10.3.0.693 STANDARD.

[[^ Back to top](#netdimensions-api-wrapper)]

## Wrapper supported methods ##
The following methods are available:

### User methods 

- **CreateUsers** (single or batch multiple)  
You can supply this method with 1 or more user details (each can contain details of its assigned organization).   
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

[[^ Back to top](#netdimensions-api-wrapper)]

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

[[^ Back to top](#netdimensions-api-wrapper)]
