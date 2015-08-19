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
