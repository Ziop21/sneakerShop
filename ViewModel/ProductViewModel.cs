using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModel
{
    public class ProductViewModel
    {
        public int Page;
        public IPagedList<Product> ProductPagedList { get; set; }
        public List<Category> Categories { get; set; }
        public int? SortBy { get; set; }
        public string SearchString { get; set; }

        public ProductViewModel()
        {
            Categories = new List<Category>();
            ProductPagedList = new PagedList<Product>(null, 1, 1);
        }

    }
}