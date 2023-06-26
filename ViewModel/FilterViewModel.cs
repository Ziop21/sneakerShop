using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModel
{
    public class FilterViewModel
    {
        public double? MaximumPrice { get; set; }
        public double? MinPrice { get; set; }
        //public Pager Pager { get; set; }  
        public List<String> SexCheck { get; set; }
        public String SearchString { get; set; }
        public String SexCheckString { get; set; }
        public List<int> CategoryCheckIds { get; set; }
        public String CategoryCheckIds_String { get; set; }
        [JsonIgnore]
        public List<Product> ProductList { get; set; }
        [JsonIgnore]
        public List<Product> productByCategoryList { get; set; }
        [JsonIgnore]
        public IQueryable<Product> productByPriceList { get; set; }
        [JsonIgnore]
        public ProductViewModel ProductViewModel { get; set; }
        //public IEnumerable<Size> Sizes { get; set; }
        public FilterViewModel()
        {
            CategoryCheckIds = new List<int>();
            productByCategoryList = new List<Product>();
            ProductViewModel = new ProductViewModel();
            ProductList = new List<Product>();
        }
    }
}