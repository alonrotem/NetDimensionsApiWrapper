using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace NetDimensionsWrapper.NetDimensions
{
    /// <summary>
    /// A wrapper for NetDimensions Talent Suite API functions
    /// </summary>
    public class NDWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NDWrapper" /> class.
        /// </summary>
        /// <param name="userAuthUsername">Username for API methods which require USER authentication</param>
        /// <param name="userAuthPassword">Passwotd for API methods which require USER authentication</param>
        /// <param name="systemAutoKey">Authentication key for API methods which require SYSTEM authentication</param>
        /// <param name="lmsBaseUrl">Base URL of the LMS</param>
        public NDWrapper(string userAuthUsername, string userAuthPassword, string systemAuthKey, string lmsBaseUrl)
        {
            this.UserAuthUser = userAuthUsername;
            this.UserAuthPass = userAuthPassword;
            this.SysAuthKey = systemAuthKey;
            if (lmsBaseUrl.EndsWith("/"))
                lmsBaseUrl = lmsBaseUrl.Trim('/', ' ');
            this.LmsUrl = lmsBaseUrl;
        }

        #region User methods

        /// <summary>
        /// Creates users in the LMS. Also adds their related organizations if needed, and assigns the users to them.
        /// </summary>
        /// <param name="users">The users to create.</param>
        /// <returns>The results of the users creation. Including any errors and warnings from the LMS.</returns>
        public UserActionResults CreateUsers(NetDimensionsUser[] users) 
        {
            if (users.Length == 0)
                return null;
            if ((users.Any(u => string.IsNullOrWhiteSpace(u.UserID))) ||
                (users.Any(u => string.IsNullOrWhiteSpace(u.LastName)) ||
                (users.Any(u => string.IsNullOrWhiteSpace(u.FirstName))) ||
                (users.Any(u => string.IsNullOrWhiteSpace(u.Password))) ||
                (users.Any(u => string.IsNullOrWhiteSpace(u.Email)))))
            {
                throw new ArgumentException("To create a user, the following fields are mandatory: UserID, LastName, FirstName, Password, Email");
            }

            UserActionResults result = null;

            string serviceApiUrl = string.Format(NetDimensionsConstants.ApiUsersCsv, this.LmsUrl);
            string usersCsv = NetDimensionsUser.GetCsv(NetDimensionsConstants.UserActions.AddOrUpdate, users);

            using (WebClient apiWebClient = new WebClient())
            {
                apiWebClient.Headers[HttpRequestHeader.ContentType] = "text/plain";
                apiWebClient.Credentials = new NetworkCredential(this.UserAuthUser, this.SysAuthKey);

                string csvResponse = Encoding.UTF8.GetString(apiWebClient.UploadData(
                    serviceApiUrl, 
                    "POST",
                    Encoding.ASCII.GetBytes(usersCsv)));

                result = new UserActionResults(csvResponse);
            }

            Dictionary<NetDimensionsOrganization, List<string>> organizationUsers = new Dictionary<NetDimensionsOrganization, List<string>>();
            foreach (NetDimensionsUser user in users)
            {
                if (user.Organization!= null)
                {
                    NetDimensionsOrganization organizationInList = organizationUsers.Keys.FirstOrDefault(o => o.Code == user.Organization.Code);
                    if (organizationInList == null)
                    {
                        organizationUsers.Add(user.Organization, new List<string>());
                    }
                    organizationInList = organizationUsers.Keys.FirstOrDefault(o => o.Code == user.Organization.Code);
                    if (string.IsNullOrWhiteSpace(organizationInList.Description))
                        organizationInList.Description = user.Organization.Description;

                    if (!organizationUsers[organizationInList].Contains(user.UserID))
                        organizationUsers[organizationInList].Add(user.UserID);
                }
            }
            foreach (KeyValuePair<NetDimensionsOrganization, List<string>> organization in organizationUsers)
            {
                this.CreateOrganization(organization.Key.Code, organization.Key.Description);
                this.AddUsersToOrganization(organization.Value.ToArray(), organization.Key.Code);
            }
            return result;
        }

        /// <summary>
        /// Deletes users from the LMS.
        /// </summary>
        /// <param name="userIds">The IDs of the user to delete.</param>
        /// <returns>The results of the users deletion. Including any errors and warnings from the LMS.</returns>
        public UserActionResults DeleteUsers(string[] userIds)
        {
            if (userIds.Length == 0)
                return null;

            UserActionResults result = null;
            StringBuilder usersCsvBuilder = new StringBuilder();
            usersCsvBuilder.Append("Actions,UserID");

            foreach (string userId in userIds)
            {
                if (!string.IsNullOrWhiteSpace(userId))
                    usersCsvBuilder.AppendFormat("\n{0},{1}", NetDimensionsConstants.UserActions.Delete, userId);
            }

            string serviceApiUrl = string.Format(NetDimensionsConstants.ApiUsersCsv, this.LmsUrl);
            using (WebClient apiWebClient = new WebClient())
            {
                apiWebClient.Headers[HttpRequestHeader.ContentType] = "text/plain";
                apiWebClient.Credentials = new NetworkCredential(this.UserAuthUser, this.SysAuthKey);

                string csvResponse = Encoding.UTF8.GetString(
                    apiWebClient.UploadData(
                        serviceApiUrl, 
                        "POST", 
                        Encoding.ASCII.GetBytes(usersCsvBuilder.ToString())));

                result = new UserActionResults(csvResponse);
            }
            return result;
        }

        /// <summary>
        /// Gets details of a user from the LMS.
        /// </summary>
        /// <param name="userId_or_email">The user's ID or email address.</param>
        /// <param name="forceAsUseId">If set to true, search will be for a user by their ID only (even if it contains @. otherwise this is auto-determined)</param>
        /// <returns>The user's details. Note that not all fields are returned from the LMS.</returns>
        public NetDimensionsUser GetUser(string userIdOrEmail, bool forceAsUseId = false)
        {
            NetDimensionsUser user = null;
            string serviceApiUrl = string.Format(NetDimensionsConstants.ApiGetUsers, this.LmsUrl);
            using (WebClient apiWebClient = new WebClient())
            {
                apiWebClient.Headers[HttpRequestHeader.ContentType] = "text/plain";
                apiWebClient.Credentials = new NetworkCredential(this.UserAuthUser, this.UserAuthPass);
                if (!userIdOrEmail.Contains("@") || forceAsUseId)
                {
                    apiWebClient.QueryString.Add("userId", userIdOrEmail);
                }
                else
                {
                    apiWebClient.QueryString.Add("email", userIdOrEmail);
                }

                apiWebClient.QueryString.Add("format", "json");
                string jsonResponse = Encoding.UTF8.GetString(apiWebClient.DownloadData(serviceApiUrl));

                dynamic dynamicResponseObject = JObject.Parse(jsonResponse);
                if ((dynamicResponseObject != null) && (dynamicResponseObject["users"] != null) && (dynamicResponseObject["users"].Count > 0) && (dynamicResponseObject["users"][0] != null))
                {
                    dynamic jsonUser = dynamicResponseObject["users"][0];
                    user = JsonConvert.DeserializeObject<NetDimensionsUser>(jsonUser.ToString());

                    if (jsonUser["attributes"] != null && jsonUser["attributes"] is JArray && ((JArray)jsonUser["attributes"]).Count == 8)
                    {
                        dynamic userAttributes = jsonUser["attributes"];
                        user.UserAttr1 = userAttributes[0]["description"].ToString();
                        user.UserAttr2 = userAttributes[1]["description"].ToString();
                        user.UserAttr3 = userAttributes[2]["description"].ToString();
                        user.UserAttr4 = userAttributes[3]["description"].ToString();
                        user.UserAttr5 = userAttributes[4]["description"].ToString();
                        user.UserAttr6 = userAttributes[5]["description"].ToString();
                        user.UserAttr7 = userAttributes[6]["description"].ToString();
                        user.UserAttr8 = userAttributes[7]["description"].ToString();
                    }
                }
            }
            return user;
        }

        #endregion

        #region Organization methods

        /// <summary>
        /// Creates an organization.
        /// </summary>
        /// <param name="organizationHierarchyCode">The organization's code in a  comma-delimited hierarchy list of organizations (not counting ROOT). e.g.: "PSC,AutoGen"</param>
        /// <param name="OrganizationDescription">The organization's description.</param>
        public void CreateOrganization(string organizationHierarchyCode, string organizationDescription)
        {
            this.PerformOrganizationAction(
                NetDimensionsConstants.OrganizationActions.Add,
                organizationHierarchyCode,
                organizationDescription);
        }

        /// <summary>
        /// Deletes an organization.
        /// </summary>
        /// <param name="organizationHierarchyCode">The organization's code in a  comma-delimited hierarchy list of organizations (not counting ROOT). e.g.: "PSC,AutoGen"</param>
        public void DeleteOrganization(string organizationHierarchyCode)
        {
            this.PerformOrganizationAction(
                NetDimensionsConstants.OrganizationActions.Delete,
                organizationHierarchyCode,
                string.Empty);
        }

        /// <summary>
        /// Gets details of an organization.
        /// </summary>
        /// <param name="organizationHierarchyCode">The organization's code in a  comma-delimited hierarchy list of organizations (not counting ROOT). e.g.: "PSC,AutoGen"</param>
        /// <returns>Details of the organization.</returns>
        public NetDimensionsOrganization GetOrganization(string organizationHierarchyCode)
        {
            if (string.IsNullOrWhiteSpace(organizationHierarchyCode))
            {
                return null;
            }

            string serviceApiUrl = string.Format(NetDimensionsConstants.ApiGetOrganization, this.LmsUrl);
            NetDimensionsOrganization currentOrganization = null;
            string currentLevelOrganizationsJs = string.Empty;
            using (WebClient apiWebClient = new WebClient())
            {
                apiWebClient.Headers[HttpRequestHeader.ContentType] = "application/xml";
                apiWebClient.Credentials = new NetworkCredential(this.UserAuthUser, this.UserAuthPass);
                apiWebClient.QueryString.Add("id", "*ROOT*");
                apiWebClient.QueryString.Add("recursive", "false");
                apiWebClient.QueryString.Add("format", "json");
                try
                {
                    currentLevelOrganizationsJs = apiWebClient.UploadString(serviceApiUrl, "GET");
                }
                catch (WebException)
                { 
                    //Not found or not authorised
                    return null;
                }
                currentOrganization = JsonConvert.DeserializeObject<NetDimensionsOrganization>(currentLevelOrganizationsJs);

                string[] organizationsHierarchy = organizationHierarchyCode.Split(',');
                foreach (string org in organizationsHierarchy)
                {
                    currentOrganization = currentOrganization.Children.FirstOrDefault(o => o.Code.Trim().Equals(org.Trim(), StringComparison.InvariantCultureIgnoreCase));
                    if (currentOrganization != null)
                    {
                        apiWebClient.QueryString["id"] = currentOrganization.Id;
                        try
                        {
                            currentLevelOrganizationsJs = apiWebClient.UploadString(serviceApiUrl, "GET");
                        }
                        catch (WebException)
                        {
                            //Not found or not authorised
                            return null;
                        }
                        currentOrganization = JsonConvert.DeserializeObject<NetDimensionsOrganization>(currentLevelOrganizationsJs);
                    }
                }
            }
            return currentOrganization;
        }

        /// <summary>
        /// Creates or deletes an organization in the system.
        /// </summary>
        /// <param name="action">The action to perform. Either add or remove.</param>
        /// <param name="organizationHierarchyCode">The organization's code in a  comma-delimited hierarchy list of organizations (not counting ROOT). e.g.: "PSC,AutoGen"</param>
        /// <param name="organizationDescription">The organization's description (relevant when creating).</param>
        private void PerformOrganizationAction(NetDimensionsConstants.OrganizationActions action, string organizationHierarchyCode, string organizationDescription)
        {
            string serviceApiUrl = string.Format(NetDimensionsConstants.ApiOrganizations, this.LmsUrl);

            var xmldoc = new XmlDocument();
            XmlNode docNode = xmldoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmldoc.AppendChild(docNode);

            XmlNode enterprise = xmldoc.AppendNode("enterprise");
                XmlNode properties = enterprise.AppendNode("properties");
                    properties.AppendNode("datasource", "Telerik");
                    properties.AppendNode("datetime", DateTime.Now.ToString("yyyy-mm-dd"));

                XmlNode group = enterprise.AppendNode("group");
                group.AppendAttribute("recstatus", action.ToString());

                    XmlNode sourcedid = group.AppendNode("sourcedid");
                    sourcedid.AppendNode("id", string.Format("organization:{0}", organizationHierarchyCode));

                    XmlNode description = group.AppendNode("description");
                    description.AppendNode("short", organizationDescription);

            using (WebClient apiWebClient = new WebClient())
            {
                apiWebClient.Headers[HttpRequestHeader.ContentType] = "application/xml";
                apiWebClient.Credentials = new NetworkCredential(this.UserAuthUser, this.SysAuthKey);
                apiWebClient.UploadString(serviceApiUrl, "POST", xmldoc.OuterXml);
            }
        }

        #endregion

        #region User/Organization-relations methods

        /// <summary>
        /// Adds the users an organization.
        /// </summary>
        /// <param name="userIds">IDs of the users to add to the organization.</param>
        /// <param name="organizationHierarchyCode">The organization's code in a  comma-delimited hierarchy list of organizations (not counting ROOT). e.g.: "PSC,AutoGen"</param>
        public void AddUsersToOrganization(string[] userIds, string organizationHierarchyCode)
        {
            if (userIds.Length == 0 || string.IsNullOrWhiteSpace(organizationHierarchyCode))
            {
                return;
            }

            string serviceApiUrl = string.Format(NetDimensionsConstants.ApiOrganizations, this.LmsUrl);

            var xmldoc = new XmlDocument();
            XmlNode docNode = xmldoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmldoc.AppendChild(docNode);

            XmlNode enterprise = xmldoc.AppendNode("enterprise");
            XmlNode properties = enterprise.AppendNode("properties");
            properties.AppendNode("datasource", "Telerik");
            properties.AppendNode("datetime", DateTime.Now.ToString("yyyy-mm-dd"));

            XmlNode sourcedid = enterprise.AppendNode("sourcedid");
            sourcedid.AppendNode("sourcedid", "Telerik");
            sourcedid.AppendNode("id", userIds[0]);

            XmlNode membership = enterprise.AppendNode("membership");
            XmlNode membershipsourcedid = membership.AppendNode("sourcedid");
            membershipsourcedid.AppendNode("sourcedid", "EKP");
            membershipsourcedid.AppendNode("id", string.Format("organization:{0}", organizationHierarchyCode));

            foreach (string userId in userIds)
            {
                XmlNode member = membership.AppendNode("member");
                member.AppendNode("idtype", "1");
                XmlNode membersourcedid = member.AppendNode("sourcedid");
                membersourcedid.AppendNode("sourcedid", "Telerik");
                membersourcedid.AppendNode("id", userId);
            }

            using (WebClient apiWebClient = new WebClient())
            {
                apiWebClient.Headers[HttpRequestHeader.ContentType] = "application/xml";
                apiWebClient.Credentials = new NetworkCredential(this.UserAuthUser, this.SysAuthKey);
                apiWebClient.UploadString(serviceApiUrl, "POST", xmldoc.OuterXml);
            }
        }

        /// <summary>
        /// Gets the IDs users of the users in an organization.
        /// </summary>
        /// <param name="organizationHierarchyCode">The organization's code in a  comma-delimited hierarchy list of organizations (not counting ROOT). e.g.: "PSC,AutoGen"</param>
        /// <param name="stat">Optional: The state of users to get (e.g. only active users).</param>
        /// <returns>IDs users of the users in the specified organization</returns>
        public string[] GetUsersInOrganization(string organizationHierarchyCode, string stat = "")
        {
            List<string> users = new List<string>();
            NetDimensionsOrganization org = this.GetOrganization(organizationHierarchyCode);
            if (org != null)
            {
                string serviceApiUrl = string.Format(NetDimensionsConstants.ApiGetUserInOrganization, this.LmsUrl);
                using (WebClient apiWebClient = new WebClient())
                {
                    apiWebClient.Headers[HttpRequestHeader.ContentType] = "application/xml";
                    apiWebClient.Credentials = new NetworkCredential(this.UserAuthUser, this.SysAuthKey);
                    apiWebClient.QueryString.Add("organizationId", org.Id);
                    
                    if(!string.IsNullOrWhiteSpace(stat))
                    {
                        apiWebClient.QueryString.Add("status", stat);
                    }
                        
                    string retVal = apiWebClient.UploadString(serviceApiUrl, "GET");

                    XmlDocument doc = new XmlDocument();
                    XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
                    doc.LoadXml(retVal);
                    ns.AddNamespace(doc.DocumentElement.Prefix, doc.DocumentElement.NamespaceURI);
                    XmlNodeList userNodes = doc.DocumentElement.SelectNodes(string.Format("//{0}:user/{0}:id", doc.DocumentElement.Prefix), ns);
                    foreach (XmlNode xn in userNodes)
                    {
                        users.Add(xn.InnerText);
                }
            }
            }

            return users.ToArray();
        }

        #endregion

        public string UserAuthUser { get; private set; }
        public string UserAuthPass { get; private set; }
        
        public string SysAuthKey { get; private set; }
        
        public string LmsUrl { get; private set; }
    }
}