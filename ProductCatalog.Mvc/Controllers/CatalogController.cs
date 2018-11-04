using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProductCatalog.Mvc.Models;

namespace ProductCatalog.Mvc.Controllers
{
    public class CatalogController : Controller
    {
        public ActionResult Index(string q)
        {
            var products = GetProducts(q);

            ViewBag.Query = q;

            return View(products);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel product)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.ApiBaseUrl);

                var responseTask = client.PostAsJsonAsync("Product", product);
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = string.Format("Product: {0} is successfully created", product.Name);
                }
                else
                {
                    TempData["ErrorMessage"] = string.Format(Resources.ERR_SERVER_ERROR, "creating");
                }

                return RedirectToAction("Index");
            }
        }

        public ActionResult Details(int id)
        {
            var product = GetProduct(id);

            return View(product);
        }

        public ActionResult Edit(int id)
        {
            var product = GetProduct(id);

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel product)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.ApiBaseUrl);

                var responseTask = client.PutAsJsonAsync(string.Format("Product/{0}", product.Id), product);
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = string.Format("Product: {0} is successfully edited", product.Name);
                }
                else
                {
                    TempData["ErrorMessage"] = string.Format(Resources.ERR_SERVER_ERROR, "editing");
                }

                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(int id, string name)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.ApiBaseUrl);

                var responseTask = client.DeleteAsync(string.Format("Product/{0}", id));
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = string.Format("Product: {0} is successfully deleted", name);
                }
                else
                {
                    TempData["ErrorMessage"] = string.Format(Resources.ERR_SERVER_ERROR, "deleting");
                }

                return RedirectToAction("Index");
            }
        }

        public ActionResult Export(string q)
        {
            var products = GetProducts(q);

            var table = new System.Data.DataTable("productCatalog");
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Photo", typeof(string));
            table.Columns.Add("Price", typeof(string));
            table.Columns.Add("Last Updated", typeof(string));

            foreach (var product in products)
            {
                table.Rows.Add(product.Id, product.Name, product.Photo, product.PriceStr, product.LastUpdated.ToShortDateString());
            }

            var grid = new GridView { DataSource = table };
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=ProductCatalog.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index", new { q = q });
        }

        private IEnumerable<ProductViewModel> GetProducts(string q)
        {
            IEnumerable<ProductViewModel> products;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.ApiBaseUrl);

                var responseTask = string.IsNullOrEmpty(q)
                    ? client.GetAsync("Product")
                    : client.GetAsync(string.Format("ProductByQuery/{0}", q));
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ProductViewModel>>();
                    readTask.Wait();

                    products = readTask.Result;
                }
                else
                {
                    products = Enumerable.Empty<ProductViewModel>();
                    TempData["ErrorMessage"] = string.Format(Resources.ERR_SERVER_ERROR, "listing");
                }
            }

            return products;
        }

        private ProductViewModel GetProduct(int id)
        {
            ProductViewModel product;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.ApiBaseUrl);

                var responseTask = client.GetAsync(string.Format("Product/{0}", id));
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ProductViewModel>();
                    readTask.Wait();

                    product = readTask.Result;
                }
                else
                {
                    product = new ProductViewModel();
                    TempData["ErrorMessage"] = string.Format(Resources.ERR_SERVER_ERROR, "viewing");
                }
            }

            return product;
        }
    }
}