using System;
using System.Configuration;
using System.Net;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Valo.WebRequestMocker.Helpers;
using Valo.WebRequestMocker.Web;

namespace Valo.WebRequestMocker.Tests
{
    [TestClass]
    public class GenerateFileFromIntegration
    {
        [TestMethod]
        public void GenerateWebRelatedResponses()
        {
            using (ClientContext context = new ClientContext(ConfigurationManager.AppSettings["SiteUrl"]))
            {
                context.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Login"], ConfigurationManager.AppSettings["Password"]);

                MockEntryResponseProvider provider = new MockEntryResponseProvider();
                FileMockDataRepo repo = new FileMockDataRepo("Mock.json");
                MockWebRequestExecutorFactory executorFactory = new MockWebRequestExecutorFactory(provider, true, repo);
                executorFactory.OnRequestExecuted += ExecutorFactory_OnRequestExecuted;
                context.WebRequestExecutorFactory = executorFactory;
                context.Load(context.Web);
                context.ExecuteQuery();

                List testList = context.Web.Lists.GetByTitle("Site Pages");
                ListItemCollection listItems = testList.GetItems(CamlQuery.CreateAllItemsQuery());
                context.Load(listItems);
                context.ExecuteQuery();

                User user = context.Web.EnsureUser("sp\\lightyear");
                context.Load(user);
                context.ExecuteQuery();

                context.Web.GetUserEffectivePermissions("sp\\lightyear");
                context.ExecuteQuery();

                Assert.AreEqual("Tea Point", context.Web.Title);

                executorFactory.SaveMockData();
            }
        }

        private void ExecutorFactory_OnRequestExecuted(object sender, Model.RequestExecutedArgs e)
        {
            Assert.IsFalse(String.IsNullOrEmpty(e.ResponseBody));
        }
    }
}
