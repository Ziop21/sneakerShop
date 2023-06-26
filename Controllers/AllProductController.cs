using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using PagedList;
using WebApplication1.ViewModel;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    public class AllProductController : Controller
    {
        private sneakerShopEntities db = new sneakerShopEntities();

        public ActionResult Index(FormCollection formCollection)
        {
            FilterViewModel filterViewModel = new FilterViewModel();
          
            #region check sex
            string temp = formCollection["CheckboxSex"];
            filterViewModel.SexCheckString = temp;
            if (temp != null)
            {
                string[] sex = temp.Split(',');

                filterViewModel.SexCheck = new List<string>();
                foreach (var item in sex)
                {
                    filterViewModel.SexCheck.Add(item);
                }
            }
            else
            {
                filterViewModel.SexCheck = new List<string>();
                filterViewModel.SexCheck.Add("Nam");
                filterViewModel.SexCheck.Add("Nữ");
            }
            #endregion

            #region checked cates
            //Get list cates
            temp = formCollection["CheckboxCate"];
            filterViewModel.CategoryCheckIds_String = temp;
            if (temp != null)
            {
                string[] cates = temp.Split(',');

                filterViewModel.CategoryCheckIds = new List<int>();
                foreach (var item in cates)
                {

                    filterViewModel.CategoryCheckIds.Add(Convert.ToInt32(item));
                }
            }
            else
            {
                filterViewModel.CategoryCheckIds = db.Categories.Select(p => p.categoryId).ToList();

            }
            #endregion

            //int sortby = (int)formCollection["sortBy"];
            //if (sortBy == null) filterViewModel.ProductViewModel.SortBy = 0;

            filterViewModel.ProductViewModel.SortBy = 0;
            filterViewModel.MaximumPrice = Convert.ToDouble(formCollection["maxprice"]);
            filterViewModel.MinPrice = Convert.ToDouble(formCollection["minprice"]);
            filterViewModel.ProductViewModel.SortBy = Convert.ToInt32(formCollection["sortBy"]);

            //Get sizes

            //Get list products by filter
            filterViewModel = getFilteredProducts(filterViewModel.MinPrice,
                filterViewModel.MaximumPrice, filterViewModel.SexCheck, filterViewModel.ProductViewModel.SortBy, filterViewModel.CategoryCheckIds, filterViewModel.SearchString);
            
            //Sort 
            switch (filterViewModel.ProductViewModel.SortBy)
            {
                case 0:
                    filterViewModel.productByPriceList = filterViewModel.productByPriceList.OrderByDescending(p => p.categoryId);
                    break;
                case 1:
                    filterViewModel.productByPriceList = filterViewModel.productByPriceList.OrderByDescending(p => p.categoryId);
                    break;
                case 2:
                    filterViewModel.productByPriceList = filterViewModel.productByPriceList.OrderBy(p => p.price);
                    break;
                case 3:
                    filterViewModel.productByPriceList = filterViewModel.productByPriceList.OrderBy(p => p.price);
                    break;
                default:
                    break;
            }

            var tempProdList = filterViewModel.productByPriceList.ToList();
            var productList = new List<Product>();

            foreach (var item in tempProdList)
            {
                if (filterViewModel.productByCategoryList.Contains(item))
                {
                    productList.Add(item);
                }
            }

            int pageSize = 6;


            int page = 1;
            filterViewModel.ProductViewModel.ProductPagedList = productList.ToPagedList(page, pageSize);
            filterViewModel.ProductViewModel.SearchString = "";

            var categories = db.Categories.Select(p => p);
            filterViewModel.ProductViewModel.Categories = categories.ToList();

            return View("Index", filterViewModel);
        }

        public PartialViewResult Pagination(int? page, string modela)
        {
            FilterViewModel filterViewModel = new FilterViewModel();

            filterViewModel = JsonConvert.DeserializeObject<FilterViewModel>(modela);


            #region Get list sexs
            //string temp = filterViewModel.SexCheckString;

            //if (temp != null)
            //{
            //    string[] sex = temp.Split(',');

            //    filterViewModel.SexCheck = new List<string>();
            //    foreach (var item in sex)
            //    {
            //        filterViewModel.SexCheck.Add(item);
            //    }
            //}
            //else
            //    filterViewModel.SexCheck = null;
            #endregion

            #region Get list cates
            //temp = filterViewModel.CategoryCheckIds_String;

            //if (temp != null)
            //{
            //    string[] cates = temp.Split(',');

            //    filterViewModel.CategoryCheckIds = new List<int>();
            //    foreach (var item in cates)
            //    {

            //        filterViewModel.CategoryCheckIds.Add(Convert.ToInt32(item));
            //    }
            //}
            //else
            //    filterViewModel.CategoryCheckIds = null;
            #endregion

            //filterViewModel.MaximumPrice = Convert.ToDouble(formCollection["maxprice"]);
            //filterViewModel.MinPrice = Convert.ToDouble(formCollection["minprice"]);
            if (filterViewModel.ProductViewModel.SortBy == null) filterViewModel.ProductViewModel.SortBy = 0;

            //Get sizes

            //Filters
            filterViewModel = getFilteredProducts(filterViewModel.MinPrice,
                filterViewModel.MaximumPrice, filterViewModel.SexCheck, filterViewModel.ProductViewModel.SortBy, filterViewModel.CategoryCheckIds, filterViewModel.SearchString);

            //Get search string           
            if (!String.IsNullOrEmpty(filterViewModel.SearchString))
            {
                filterViewModel.productByPriceList = filterViewModel.productByPriceList.Where(b => b.productName.ToLower().
                Contains(filterViewModel.SearchString.ToLower()));
            }


            switch (filterViewModel.ProductViewModel.SortBy)
            {
                case 0:
                    filterViewModel.productByPriceList = filterViewModel.productByPriceList.OrderByDescending(p => p.categoryId);
                    break;
                case 1:
                    filterViewModel.productByPriceList = filterViewModel.productByPriceList.OrderByDescending(p => p.categoryId);
                    break;
                case 2:
                    filterViewModel.productByPriceList = filterViewModel.productByPriceList.OrderBy(p => p.price);
                    break;
                case 3:
                    filterViewModel.productByPriceList = filterViewModel.productByPriceList.OrderBy(p => p.price);
                    break;
                default:
                    break;
            }


            #region get products
            var tempProdList = filterViewModel.productByPriceList.ToList();
            var productList = new List<Product>();


            foreach (var item in tempProdList)
            {
                if (filterViewModel.productByCategoryList.Contains(item))
                {
                    filterViewModel.ProductList.Add(item);
                }
            }
            #endregion


            #region set Page
            if (page == null) page = 1;

            int pageSize = 6;

            int pageNumber = (page ?? 1);

            filterViewModel.ProductViewModel.Page = (int)page;

            #endregion


            filterViewModel.ProductViewModel.ProductPagedList = filterViewModel.ProductList.ToPagedList(filterViewModel.ProductViewModel.Page, pageSize);

            var categories = db.Categories.Select(p => p);
            filterViewModel.ProductViewModel.Categories = categories.ToList();

            return PartialView("Index", filterViewModel);
        }
        [HttpPost]
        public PartialViewResult FilteredProducts(FormCollection formCollection)
       
        {
            FilterViewModel filterViewModel = new FilterViewModel();

            #region Get list sexs
            string temp = formCollection["CheckboxSex"];

            if (temp != null)
            {
                string[] sex = temp.Split(',');

                filterViewModel.SexCheck = new List<string>();
                foreach (var item in sex)
                {
                    filterViewModel.SexCheck.Add(item);
                }
            }
            else
                filterViewModel.SexCheck = null;
            #endregion


            #region Get list cates
            temp = formCollection["CheckboxCate"];

            if (temp != null)
            {
                string[] cates = temp.Split(',');

                filterViewModel.CategoryCheckIds = new List<int>();
                foreach (var item in cates)
                {

                    filterViewModel.CategoryCheckIds.Add(Convert.ToInt32(item));
                }
            }
            else
                filterViewModel.CategoryCheckIds = null;
            #endregion

            filterViewModel.MaximumPrice = Convert.ToDouble(formCollection["maxprice"]);
            filterViewModel.MinPrice = Convert.ToDouble(formCollection["minprice"]);
            filterViewModel.ProductViewModel.SortBy = Convert.ToInt32(formCollection["sortBy"]);

            //Get sizes

            //Filters
            filterViewModel = getFilteredProducts(filterViewModel.MinPrice,
                filterViewModel.MaximumPrice, filterViewModel.SexCheck, filterViewModel.ProductViewModel.SortBy, filterViewModel.CategoryCheckIds, filterViewModel.SearchString);

            //Get search string
            filterViewModel.SearchString = formCollection["SearchString"];
            if (!String.IsNullOrEmpty(filterViewModel.SearchString))
            {
                filterViewModel.productByPriceList = filterViewModel.productByPriceList.Where(b => b.productName.ToLower().
                Contains(filterViewModel.SearchString.ToLower()));
            }


            switch (filterViewModel.ProductViewModel.SortBy)
            {
                case 0:
                    filterViewModel.productByPriceList = filterViewModel.productByPriceList.OrderByDescending(p => p.categoryId);
                    break;
                case 1:
                    filterViewModel.productByPriceList = filterViewModel.productByPriceList.OrderByDescending(p => p.categoryId);
                    break;
                case 2:
                    filterViewModel.productByPriceList = filterViewModel.productByPriceList.OrderBy(p => p.price);
                    break;
                case 3:
                    filterViewModel.productByPriceList = filterViewModel.productByPriceList.OrderBy(p => p.price);
                    break;
                default:
                    break;
            }


            #region get products
            var tempProdList = filterViewModel.productByPriceList.ToList();
            var productList = new List<Product>();


            foreach (var item in tempProdList)
            {
                if (filterViewModel.productByCategoryList.Contains(item))
                {
                    filterViewModel.ProductList.Add(item);
                }
            }
            #endregion

            
            
            

            filterViewModel.ProductViewModel.ProductPagedList = filterViewModel.ProductList.ToPagedList(1, 6);

            var categories = db.Categories.Select(p => p);
            filterViewModel.ProductViewModel.Categories = categories.ToList();

            return PartialView("Index", filterViewModel);
        }
        public FilterViewModel getFilteredProducts(double? minimumPrice,
            double? maximumPrice, List<string> SexCheck, int? sortBy, List<int> categoryCheckIds,
            string searchString)
        {
            var categories = db.Categories.Select(p => p);


            if (maximumPrice == 0) maximumPrice = db.Products.OrderByDescending(p => p.price).First().price;


            FilterViewModel filterViewModel = new FilterViewModel();
            filterViewModel.MaximumPrice = maximumPrice;
            filterViewModel.MinPrice = minimumPrice;
            filterViewModel.ProductViewModel.SortBy = sortBy;
            filterViewModel.CategoryCheckIds = categoryCheckIds;
            filterViewModel.SexCheck = SexCheck;
            filterViewModel.SearchString = searchString;

            //set productList
            foreach (var id in filterViewModel.CategoryCheckIds)
            {
                if (SexCheck.Count == 2)
                {
                    var temp = db.Products.Where(p => p.categoryId == id).Where(p => p.price >= minimumPrice && p.price <= maximumPrice)
                    .Select(p => p).OrderByDescending(p => p.categoryId)
                    .Include(p => p.Category).Include(p => p.Stocks)
                    .Include(p => p.imagesProducts).ToList();
                    foreach (var item in temp)
                    {
                        filterViewModel.productByCategoryList.Add(item);
                    }
                }
                else               
                {
                    foreach (var sex in SexCheck)
                    {
                        var temp = db.Products.Where(p => p.categoryId == id).Where(p => p.price >= minimumPrice && p.price <= maximumPrice).Where(p => p.sex.Contains(sex))
                        .Select(p => p).OrderByDescending(p => p.categoryId)
                        .Include(p => p.Category).Include(p => p.Stocks)
                        .Include(p => p.imagesProducts).ToList();
                        foreach (var item in temp)
                        {
                            filterViewModel.productByCategoryList.Add(item);
                        }
                    }
                }

            }
            //Set productPriceList
            filterViewModel.productByPriceList = db.Products.Where(p => p.price >= minimumPrice && p.price <= maximumPrice)
                    .Select(p => p).OrderByDescending(p => p.categoryId)
                    .Include(p => p.Category).Include(p => p.Stocks)
                    .Include(p => p.imagesProducts);
            return filterViewModel;
        }
        //[HttpPost]
        //public PartialViewResult FilteredProducts(FormCollection formCollection)
        //{
        //    FilterViewModel filterViewModel = new FilterViewModel();

        //    //Get list sexs
        //    string temp = formCollection["CheckboxSex"];

        //    if (temp != null)
        //    {
        //        string[] sex = temp.Split(',');

        //        filterViewModel.SexCheck = new List<string>();
        //        foreach (var item in sex)
        //        {
        //            filterViewModel.SexCheck.Add(item);
        //        }
        //    }
        //    else
        //        filterViewModel.SexCheck = null;


        //    //Get list cates
        //    temp = formCollection["CheckboxCate"];

        //    if (temp != null)
        //    {
        //        string[] cates = temp.Split(',');

        //        filterViewModel.CategoryCheckIds = new List<int>();
        //        foreach (var item in cates)
        //        {

        //            filterViewModel.CategoryCheckIds.Add(Convert.ToInt32(item));
        //        }
        //    }
        //    else
        //        filterViewModel.CategoryCheckIds = null;

        //    filterViewModel.MaximumPrice = Convert.ToDouble(formCollection["maxprice"]);
        //    filterViewModel.MinPrice = Convert.ToDouble(formCollection["minprice"]);
        //    filterViewModel.ProductViewModel.SortBy = Convert.ToInt32(formCollection["sortBy"]);

        //    //Get sizes

        //    //Filters
        //    filterViewModel = getFilteredProducts(filterViewModel.MinPrice,
        //        filterViewModel.MaximumPrice, filterViewModel.SexCheck, filterViewModel.ProductViewModel.SortBy, filterViewModel.CategoryCheckIds);




        //    switch (filterViewModel.ProductViewModel.SortBy)
        //    {
        //        case 0:
        //            filterViewModel.productPriceList = filterViewModel.productPriceList.OrderByDescending(p => p.categoryId);
        //            break;
        //        case 1:
        //            filterViewModel.productPriceList = filterViewModel.productPriceList.OrderByDescending(p => p.categoryId);
        //            break;
        //        case 2:
        //            filterViewModel.productPriceList = filterViewModel.productPriceList.OrderBy(p => p.price);
        //            break;
        //        case 3:
        //            filterViewModel.productPriceList = filterViewModel.productPriceList.OrderBy(p => p.price);
        //            break;
        //        default:
        //            break;
        //    }

        //    var tempProdList = filterViewModel.productPriceList.ToList();
        //    var productList = new List<Product>();


        //    foreach (var item in tempProdList)
        //    {
        //        if (filterViewModel.productList.Contains(item))
        //        {
        //            productList.Add(item);
        //        }
        //    }
        //    filterViewModel.ProductViewModel.SearchString = "";
        //    filterViewModel.ProductViewModel.ProductPagedList = filterViewModel.productList.ToPagedList(1, 6);

        //    var categories = db.Categories.Select(p => p);
        //    filterViewModel.ProductViewModel.Categories = categories.ToList();

        //    return PartialView("FilterProducts", filterViewModel);
        //}



        //[HttpPost]
        //public async ActionResult FilteredProducts(int? page, string searchString, double? minimumPrice,
        //    double? maximumPrice, List<int> categoryCheckIds, int? sortBy, List<String> SexCheckIds)
        //{
        //    FilterViewModel filterViewModel = getFilteredProducts(minimumPrice, maximumPrice, categoryCheckIds, sortBy);


        //    if (page == null) page = 1;

        //    var categories = db.Categories.Select(p => p);

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        searchString = searchString.ToLower();
        //        filterViewModel.productList = filterViewModel.productList.Where(b => b.productName.ToLower().Contains(searchString));
        //    }

        //    switch (sortBy)
        //    {
        //        case 0:
        //            filterViewModel.productList = filterViewModel.productList.OrderByDescending(p => p.categoryId);
        //            break;
        //        case 1:
        //            filterViewModel.productList = filterViewModel.productList.OrderByDescending(p => p.categoryId);
        //            break;
        //        case 2:
        //            filterViewModel.productList = filterViewModel.productList.OrderBy(p => p.price);
        //            break;
        //        case 3:
        //            filterViewModel.productList = filterViewModel.productList.OrderBy(p => p.price);
        //            break;
        //        default:
        //            break;
        //    }
        //    int pageSize = 6;

        //    int pageNumber = (page ?? 1);

        //    filterViewModel.ProductViewModel.Categories = categories.ToList();
        //    filterViewModel.ProductViewModel.ProductPagedList = filterViewModel.productList.ToPagedList(pageNumber, pageSize);
        //    filterViewModel.ProductViewModel.SearchString = searchString;
        //    filterViewModel.ProductViewModel.SortBy = sortBy;
        //    filterViewModel.ProductViewModel.Page = page;



        //    return PartialView("FilterProducts", filterViewModel);
        //}
        //public FilterViewModel getFilteredProducts(double? minimumPrice,
        //    double? maximumPrice, List<int> categoryCheckIds, int? sortBy, FormCollection formCollection)
        //{
        //    var product = db.Products.Include(p => p.Category).Include(p => p.Stocks)
        //        .Include(p => p.imagesProducts);
        //    var categories = db.Categories.Select(p => p);

        //    if (!minimumPrice.HasValue) minimumPrice = 0;

        //    if (!maximumPrice.HasValue) maximumPrice = product.OrderByDescending(p => p.price).First().price;


        //    categoryCheckIds = new List<int>();
        //    if (categoryCheckIds.Count == 0)
        //    {
        //        categoryCheckIds = db.Categories.Select(p => p.categoryId).ToList();
        //    }
        //    if (!sortBy.HasValue) sortBy = 0;


        //    FilterViewModel filterViewModel = new FilterViewModel();
        //    filterViewModel.MaximumPrice = maximumPrice;
        //    filterViewModel.MinPrice = minimumPrice;
        //    filterViewModel.CategoryCheckIds = categoryCheckIds;

        //    filterViewModel.productList = db.Products.Where(p => p.price >= minimumPrice && p.price <= maximumPrice)
        //            .Select(p => p).OrderByDescending(p => p.categoryId)
        //            .Include(p => p.Category).Include(p => p.Stocks)
        //            .Include(p => p.imagesProducts);

        //    return filterViewModel;
        //}

    }
}