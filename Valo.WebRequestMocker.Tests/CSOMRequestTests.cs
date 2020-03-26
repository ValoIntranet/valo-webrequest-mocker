using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valo.WebRequestMocker.Model;
using Valo.WebRequestMocker.Web;

namespace Valo.WebRequestMocker.Tests
{
    [TestClass]
    public class CSOMRequestTests
    {
        [TestMethod]
        public void CSOMRequest_Test_ListItemsQueryDeserialization()
        {
            string request = "<Request AddExpandoFieldTypeSuffix=\"true\" SchemaVersion=\"15.0.0.0\" LibraryVersion=\"16.0.0.0\" ApplicationName=\".NET Library\" xmlns=\"http://schemas.microsoft.com/sharepoint/clientquery/2009\"><Actions><ObjectPath Id=\"2\" ObjectPathId=\"1\" /><ObjectPath Id=\"4\" ObjectPathId=\"3\" /><ObjectPath Id=\"6\" ObjectPathId=\"5\" /><ObjectPath Id=\"8\" ObjectPathId=\"7\" /><ObjectIdentityQuery Id=\"9\" ObjectPathId=\"7\" /><ObjectPath Id=\"11\" ObjectPathId=\"10\" /><Query Id=\"12\" ObjectPathId=\"10\"><Query SelectAllProperties=\"true\"><Properties /></Query><ChildItemQuery SelectAllProperties=\"true\"><Properties /></ChildItemQuery></Query></Actions><ObjectPaths><StaticProperty Id=\"1\" TypeId=\"{3747adcd-a3c3-41b9-bfab-4a64dd2f1e0a}\" Name=\"Current\" /><Property Id=\"3\" ParentId=\"1\" Name=\"Web\" /><Property Id=\"5\" ParentId=\"3\" Name=\"Lists\" /><Method Id=\"7\" ParentId=\"5\" Name=\"GetByTitle\"><Parameters><Parameter Type=\"String\">TestList</Parameter></Parameters></Method><Method Id=\"10\" ParentId=\"7\" Name=\"GetItems\"><Parameters><Parameter TypeId=\"{3d248d7b-fc86-40a3-aa97-02a75d69fb8a}\"><Property Name=\"AllowIncrementalResults\" Type=\"Boolean\">false</Property><Property Name=\"DatesInUtc\" Type=\"Boolean\">true</Property><Property Name=\"FolderServerRelativePath\" Type=\"Null\" /><Property Name=\"FolderServerRelativeUrl\" Type=\"Null\" /><Property Name=\"ListItemCollectionPosition\" Type=\"Null\" /><Property Name=\"ViewXml\" Type=\"String\">&lt;View Scope=\"RecursiveAll\"&gt;&lt;ViewFields&gt;&lt;FieldRef Name=\"Id\" /&gt;&lt;FieldRef Name=\"Title\" /&gt;&lt;FieldRef Name=\"Author\" /&gt;&lt;FieldRef Name=\"Editor\" /&gt;&lt;/ViewFields&gt;&lt;RowLimit&gt;5&lt;/RowLimit&gt;&lt;/View&gt;</Property></Parameter></Parameters></Method></ObjectPaths></Request>";

            CSOMRequest deserializedRequest = MockEntryResponseProvider.GetRequest(request);
            ObjectPathMethod getItemsMethod = deserializedRequest.ObjectPaths.Last() as ObjectPathMethod;
            Assert.AreEqual("{3d248d7b-fc86-40a3-aa97-02a75d69fb8a}", getItemsMethod.Parameters.First().TypeId);
            Assert.AreEqual(6, getItemsMethod.Parameters.First().Properties.Count);

        }
    }
}
