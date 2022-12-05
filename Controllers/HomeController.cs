using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRUD_TASK.Models;
using System.Data.Entity;
using System.Data.SqlClient;
using PagedList;


namespace CRUD_TASK.Controllers
{
    public class HomeController : Controller
    {
        // GET: Product
        ProductContext db = new ProductContext();
       

        public ActionResult Index(string Sorting_Order, string Search_Data, string Filter_Value, int ? Page_No)
        {
            ViewBag.CurrentSortOrder = Sorting_Order;
            ViewBag.SortingProductName = String.IsNullOrEmpty(Sorting_Order) ?"ProuductName" : "ProuductName";
            ViewBag.SortingProductId = Sorting_Order == "ProductId" ?"ProductId" : "ProductId";
            ViewBag.SortingCategoryId = Sorting_Order == "CategoryId" ? "CategoryId" : "CategoryId";
            ViewBag.SortingCategoryName = String.IsNullOrEmpty(Sorting_Order) ? "CategoryName" : "CategoryName";

            if (Search_Data != null)
            {
                Page_No = 1;
            }
            else
            {
                Search_Data = Filter_Value;
            }

            ViewBag.FilterValue = Search_Data;

            var Products = from p in db.Products select p;

            if (!String.IsNullOrEmpty(Search_Data))
            {
                Products = Products.Where(p => p.ProductName.ToUpper().Contains(Search_Data.ToUpper())
                    || p.CategoryName.ToUpper().Contains(Search_Data.ToUpper()));
            }
            switch (Sorting_Order)
            {
                case "ProductId":
                    Products = Products.OrderBy(p => p.ProductId);
                    break;
                case "ProductName":
                    Products = Products.OrderBy(p => p.ProductName);
                    break;
                case "CategoryId":
                    Products = Products.OrderBy(p => p.CategoryId);
                    break;
                case "CategoryName":
                    Products = Products.OrderBy(p => p.CategoryName);
                    break;
                default:
                    Products = Products.OrderBy(P => P.ProductName) ;
                    break;
            }



            int Size_Of_Page = 10;
            int No_Of_Page = (Page_No ?? 1);
            return View(Products.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product p)
        {
            if (ModelState.IsValid == true)
            {
                db.Products.Add(p);
                int a = db.SaveChanges();
                if (a > 0)
                {

                    TempData["InsertMessage"] = "<script>alert('Data Inserted !!')</script>";
                    return RedirectToAction("Index");
                }
                else
                {

                    TempData["InsertMessage"] = "<script>alert('Data Not Inserted !!')</script>";
                }
            }

            return View();
        }
        public ActionResult Edit(int ProductId)
        {
            var row = db.Products.Where(model => model.ProductId == ProductId).FirstOrDefault();
            return View(row);
        }
        [HttpPost]
        public ActionResult Edit(Product p)
        {
            if (ModelState.IsValid == true)
            {
                db.Entry(p).State = EntityState.Modified;
                int a = db.SaveChanges();
                if (a > 0)
                {

                    TempData["UpdateMessage"] = "<script>alert('Data Updated !!')</script>";

                    return RedirectToAction("Index");


                }
                else
                {
                    ViewBag.UpdateMessage = "<script>alert('Data Not Updated !!')</script>";
                }
            }

            return View();
        }

        public ActionResult Delete(int ProductId)
        {
            var ProductIdRow = db.Products.Where(model => model.ProductId == ProductId).FirstOrDefault();
            return View(ProductIdRow);
        }

        [HttpPost]

        public ActionResult Delete(Product p)
        {
            db.Entry(p).State = EntityState.Deleted;
            int a = db.SaveChanges();
            if (a > 0)
            {
                TempData["DeleteMessage"] = "<script>alert('Data Deleted !!')</script>";
            }
            else
            {
                TempData["DeleteMessage"] = "<script>alert('Data Not Deleted !!')</script>";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Details(int ProductId)
        {
            var DetailsById = db.Products.Where(model => model.ProductId == ProductId).FirstOrDefault();
            return View(DetailsById);
        }
    }


}
