using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ProductCatalog.Mvc.Controllers;

namespace ProductCatalog.Mvc.Tests.Controllers
{
    [TestClass]
    public class CatalogControllerTest
    {
        [TestMethod]
        public void Test_Index()
        {
            CatalogController controller = new CatalogController();

            ViewResult result = controller.Index(string.Empty) as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Test_Details()
        {
            CatalogController controller = new CatalogController();

            ViewResult result = controller.Details(1) as ViewResult;

            Assert.IsNotNull(result);
        }
    }
}
