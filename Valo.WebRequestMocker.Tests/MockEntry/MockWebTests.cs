using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valo.WebRequestMocker.Model;
using Valo.WebRequestMocker.Web;

namespace Valo.WebRequestMocker.Tests.MockEntry
{
    [TestClass]
    public class MockWebTests
    {
        string TestSiteUrl = "https://testsite.sharepoint.com/sites/test";
        [TestMethod]
        public void MockEntryResponseProvider_Web_Test_GetWeb()
        {
            MockEntryResponseProvider provider = new MockEntryResponseProvider();
            provider.ResponseEntries.Add(new MockResponseEntry()
            {
                Url = TestSiteUrl,
                PropertyName = "Web",
                ReturnValue = new { Title = "Test Web", WelcomePage = "SitePages\u002fHome.aspx", Id = @"\/Guid(73131455-6953-4d5a-bc42-e5fb2db96a98)\/" }
            });
            using (ClientContext context = new ClientContext(TestSiteUrl))
            {
                context.WebRequestExecutorFactory = new MockWebRequestExecutorFactory(provider);
                context.Load(context.Web);
                context.ExecuteQuery();

                Assert.AreEqual("Test Web", context.Web.Title);
                Assert.AreEqual("SitePages/Home.aspx", context.Web.WelcomePage);
                Assert.AreEqual(Guid.Parse("73131455-6953-4d5a-bc42-e5fb2db96a98"), context.Web.Id);

            }
        }
        [TestMethod]
        public void MockEntryResponseProvider_Web_Test_GetPermissionMask()
        {
            MockEntryResponseProvider provider = new MockEntryResponseProvider();
            provider.ResponseEntries.Add(new MockResponseEntry()
            {
                Url = TestSiteUrl,
                Method = "GetUserEffectivePermissions",
                NameValueParameters = new Dictionary<string, string>() { { "User", "SP\\TestUser" } },
                ReturnValue = new { _ObjectType_ = "SP.BasePermissions", High = 432, Low = 1011028583 }
            });
            using (ClientContext context = new ClientContext(TestSiteUrl))
            {
                context.WebRequestExecutorFactory = new MockWebRequestExecutorFactory(provider);
                ClientResult<BasePermissions> permissionMask = context.Web.GetUserEffectivePermissions("SP\\TestUser");
                context.ExecuteQuery();
                Assert.IsTrue(permissionMask.Value.Has(PermissionKind.EditListItems));
            }
        }
        [TestMethod]
        public void MockEntryResponseProvider_Web_Test_GetPropertyBag()
        {
            MockEntryResponseProvider provider = new MockEntryResponseProvider();
            provider.ResponseEntries.Add(new MockResponseEntry()
            {
                Url = TestSiteUrl,
                PropertyName = "AllProperties",
                ReturnValue = new
                {
                    _ObjectType_ = "SP.PropertyValues",
                    Prop1 = "Test Property 1",
                    Prop2 = "Test Property 2",
                }
            });
            using (ClientContext context = new ClientContext(TestSiteUrl))
            {
                context.WebRequestExecutorFactory = new MockWebRequestExecutorFactory(provider);
                Microsoft.SharePoint.Client.Web currentWeb = context.Web;
                PropertyValues allProperties = currentWeb.AllProperties;
                context.Load(allProperties);
                context.ExecuteQuery();

                Assert.AreEqual("Test Property 1", allProperties["Prop1"]);
                Assert.AreEqual("Test Property 2", allProperties["Prop2"]);
            }
        }
        [TestMethod]
        public void MockEntryResponseProvider_Web_Test_EnsureUser()
        {
            MockEntryResponseProvider provider = new MockEntryResponseProvider();
            provider.ResponseEntries.Add(new MockResponseEntry()
            {
                Url = TestSiteUrl,
                Method = "EnsureUser",
                ReturnValue = new
                {
                    _ObjectType_ = "SP.User",
                    Id = 3,
                    IsHiddenInUI = false,
                    LoginName = "SP\\TestUser",
                    Title = "Test User",
                    PrincipalType = 1
                }
            });
            using (ClientContext context = new ClientContext(TestSiteUrl))
            {
                context.WebRequestExecutorFactory = new MockWebRequestExecutorFactory(provider);
                User user = context.Web.EnsureUser("SP\\TestUser");
                context.Load(user);
                context.ExecuteQuery();
                Assert.AreEqual("Test User", user.Title);
            }
        }
    }
}
