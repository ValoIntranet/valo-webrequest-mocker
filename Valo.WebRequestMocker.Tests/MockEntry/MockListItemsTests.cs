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
    public class MockListItemsTests
    {
        static string TestSiteUrl = "https://testsite.sharepoint.com/sites/test";
        MockResponseEntry itemsResponse = new MockResponseEntry()
        {
            Url = TestSiteUrl,
            Method = "GetItems",
            ParentParameterValues = new List<string>() { "Test List" },
            ReturnValue = new
            {
                _ObjectType_ = "SP.ListItemCollection",
                _Child_Items_ = new[] {
                        new {
                            _ObjectType_ = "SP.ListItem",
                            Id = 1,
                            Title = "Test Item 1",
                            Author = new {
                                _ObjectType_ = "SP.FieldUserValue",
                                LookupId = 1073741823,
                                LookupValue = "System Account"
                                }
                            },
                        new {
                            _ObjectType_ = "SP.ListItem",
                            Id = 2,
                            Title = "Test Item 2",
                            Author = new {
                                _ObjectType_ = "SP.FieldUserValue",
                                LookupId = 1073741823,
                                LookupValue = "System Account"
                                }
                            },
                        new {
                            _ObjectType_ = "SP.ListItem",
                            Id = 3,
                            Title = "Test Item 3",
                            Author = new {
                                _ObjectType_ = "SP.FieldUserValue",
                                LookupId = 1073741823,
                                LookupValue = "System Account"
                                }
                            }
                    }
            }
        };
        [TestMethod]
        public void MockEntryResponseProvider_ListItems_Test_AllItemsQuery()
        {
            MockEntryResponseProvider provider = new MockEntryResponseProvider();
            provider.ResponseEntries.Add(itemsResponse);
            using (ClientContext context = new ClientContext(TestSiteUrl))
            {
                context.WebRequestExecutorFactory = new MockWebRequestExecutorFactory(provider);

                List testList = context.Web.Lists.GetByTitle("Test List");
                ListItemCollection listItems = testList.GetItems(CamlQuery.CreateAllItemsQuery());
                context.Load(listItems);
                context.ExecuteQuery();

                ListItem item = listItems[1];
                Assert.AreEqual(item["Title"], "Test Item 2");
                FieldLookupValue authorLkp = item["Author"] as FieldLookupValue;
                Assert.AreEqual(authorLkp.LookupId, 1073741823);
                Assert.AreEqual(authorLkp.LookupValue, "System Account");

            }
        }
        [TestMethod]
        public void MockEntryResponseProvider_ListItems_Test_ValidateQuery()
        {
            MockEntryResponseProvider provider = new MockEntryResponseProvider();
            itemsResponse.NameValueParameters.Add("ViewXml", "<View Scope=\"RecursiveAll\">\r\n    <Query>\r\n    </Query>\r\n</View>");
            provider.ResponseEntries.Add(itemsResponse);
            using (ClientContext context = new ClientContext(TestSiteUrl))
            {
                context.WebRequestExecutorFactory = new MockWebRequestExecutorFactory(provider);

                List testList = context.Web.Lists.GetByTitle("Test List");
                ListItemCollection listItems = testList.GetItems(CamlQuery.CreateAllItemsQuery());
                context.Load(listItems);
                context.ExecuteQuery();

                ListItem item = listItems[1];
                Assert.AreEqual(item["Title"], "Test Item 2");
                FieldLookupValue authorLkp = item["Author"] as FieldLookupValue;
                Assert.AreEqual(authorLkp.LookupId, 1073741823);
                Assert.AreEqual(authorLkp.LookupValue, "System Account");

            }
        }
    }
}
