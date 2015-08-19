# NetDimensions API Wrapper
This is an initial implementation of a .NET wrapper for NetDimensions's Talent Suite main API functions.

## Wrapper supported methods ##
The following methods are available:

### User methods 

- **CreateUsers** (single or batch multiple)
You can supply this method with 1 or more user details (each can contain details of its assigned organization).  
This function will:  
  - Create the users  
  - Create their assigned organizations, if those don’t exist  
  - Assign the users to their respective organization.

 
- **DeleteUsers** (single or batch multiple) 
This function receives 1 or more user IDs, and will delete all of them.

- **GetUser** (single)   
This function receives a user’s ID or an email address, and returns the user’s details (if found), including all its attributes and their assigned organization.

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
