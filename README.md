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
    - [A word about organization codes](#a-word-about-organization-codes) 
    - [Supported organization methods](#supported-organization-methods)
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
"<i>When calling a function that uses system authentication, the caller must supply a global system password ... The value of the password is configured in the</i> <b>`WEB-INF/conf/ekp.properties`</b> <i>configuration file using the</i><b> `authentication.key` </b><i>property.</i>" (<a href="https://wiki.netdimensions.com/confluence/display/ptk/Authentication+modes" target="_blank">from the Wiki</a>).

**Also note** (from the wiki): "<i>When using <b>HTTP basic access</b> authentication, credentials are passed in what is essentially <b>clear text</b></i>". <b>This is the method used by default by the wrapper</b>. 

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

  **Arguments:**  
   - users (an array of NetDimensionUser objects)  
   The users to create, with all their relevant details + optional organization. See [User fields](#user-fields) below, to learn which fields are mandatory.

  **Returns:** An UserActionResults object, parsed results from the LMS, including the success status (success/fail), errors and warnings, if received.

 
- **DeleteUsers** (single or batch multiple)  
This function receives 1 or more user IDs, and will delete all of them.

  **Arguments:**  
 - userIds (an array of strings)   
   IDs of the users to add to the delete.

  **Returns:** An UserActionResults object, parsed results from the LMS, including the success status (success/fail), errors and warnings, if received.

- **GetUser** (single)   
This function receives a user’s ID or an email address, and returns the user’s details (if found), including all its attributes and their assigned organization. 
  **Arguments:**  
 - userIdOrEmail (string)   
   The **ID or email address ** of the user to retrive.
 
 - forceAsUseId (boolean, optional. Default: false)  
 When this is set to true, the userIdOrEmail argument is forcefully treated as a user ID, even if it's formatted as an email address (otherwise, it's automatically determined by its structure). 

  **Returns:** A NetDimensionsUser object.  
  Note that not all the user's fields are returned by the **GetUser** function. The ones which are returned are listed in the table below, [User fields](#user-fields). 

[[^ Back to TOC](#toc)]

### Organization methods 

#### A word about organization codes
Organizations in the NetDimensions LMS are hierarchical. Each Organization has its own:
-  **Unique** ID, can be a GUID for example (`id` field)
-  It's own code, unique in its hierarchical level among siblings (`code` field)
-  Description text (`description` field)
-  Child-organization (`children` field)
  
To get information about organizations in your system, use the following service:  
<i>https://**&lt;your LMS root URL&gt;**/api/organization?id=*ROOT*&recursive=true&format=json&assignmentId=</i>  

This URL's method is `GET`, so if your user has sufficient permissions to access the API, while logged in, just open this URL in your browser to get a full hierarchical JSON of all the organizations.

**Using the codes**
To create an organization (using the **CreateOrganization** method below, or embedded in the user's properties, when calling **CreateUsers**), you should list the organizations' codes (each of the `code` fields), in a comma-delimited list - **not including the root **organization.

For example, here is a sample JSON structure that can be received by calling the organizations service: 

```json
{
    "id": "*ROOT*",
    "code": "ROOT",
    "description": "ALL",
    "attributes": [ ],
    "children": [
		{
			"id": "F2137E1C-8BFD-468E-89A7-ACFBB95DD9ED",
			"code": "MyCompany",
			"description": "My company",
			"attributes": [ ],
			"children": [
				{
					"id": "DC8A048D-003E-4E48-B238-82DEF4C0C3C5",
					"code": "PaidUsers",
					"description": "Paid users",
					"attributes": [ ],
					"children": [ ]
				},
				{
					"id": "8BAC5C54-4490-4DCD-9E91-95582FA24FB4",
					"code": "UnpaidUsers",
					"description": "Unpaid users",
					"attributes": [ ],
					"children": [ ]
				}
			]
		},
		{
			"id": "95B511D1-4521-44A8-94E5-819E9AA50BF6",
			"code": "MyOtherCompany",
			"description": "My other company",
			"attributes": [ ],
			"children": [ ]
		}
	]
} 
```

To assign users to the "Unpaid users" organization, supply the following string:
`MyCompany,UnpaidUsers`

To create a sub-organization under the "Unpaid users" organization, supply the following string:
`MyCompany,UnpaidUsers,<your new organization code>`

[[^ Back to TOC](#toc)]

#### Supported organization methods

 - **CreateOrganization** (single)   
  Given an organization hierarchy of codes and an optional description, this method creates an organization in the system.  
  **Arguments:**  
   - organizationHierarchyCode (string)  
   An organization comma-separated list of hierarchical codes (see [A word about organization codes](#a-word-about-organization-codes) above)

  - organizationDescription (string)   
  A description text for the newly created organization

  **Returns:** Void.

- **DeleteOrganization** (single)   
Given an organization hierarchy of codes, this method deletes an organization from the system.  
  **Arguments:**  
   - organizationHierarchyCode (string)  
   An organization comma-separated list of hierarchical codes (see [A word about organization codes](#a-word-about-organization-codes) above)

  **Returns:** Void.	


- **GetOrganization** (single)   
This method returns the basic data of an organization in the system (its code, ID, description and hierarchy or parents).  
  **Arguments:**  
   - organizationHierarchyCode (string)  
   An organization comma-separated list of hierarchical codes (see [A word about organization codes](#a-word-about-organization-codes) above)

  **Returns:** A NetDimensionsOrganization object, with a full hierarchy of parents.	

[[^ Back to TOC](#toc)]

### User/organization methods 

- **AddUsersToOrganization** (single or batch multiple)   
Given one or more user IDs and an organization code, this method assigns users to the organization.
  **Arguments:**  
   - userIds (an array of strings)   
   IDs of the users to add to the selected organization.
   
   - organizationHierarchyCode (string)  
   An organization comma-separated list of hierarchical codes (see [A word about organization codes](#a-word-about-organization-codes) above)

  **Returns:** Void.

- **GetUsersInOrganization** (single)   
Given an organization code and optional user status (e.g. all the "active" users), it returns the IDs of the users in that organization.
  **Arguments:**  
   - organizationHierarchyCode (string)  
   An organization comma-separated list of hierarchical codes (see [A word about organization codes](#a-word-about-organization-codes) above)  

 - stat (string, optional. Default: empty string)  
	Set this string to the state of the users to retrieve (e.g. "active"), if relevant.

  **Returns:** An array of strings, IDs of the user in the selected organization.

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
