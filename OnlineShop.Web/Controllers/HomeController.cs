using AutoMapper;
using OnlineShop.Common;
using OnlineShop.Data.Models;
using OnlineShop.Service;
using OnlineShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Web.Controllers
{
    
    public class HomeController : Controller
    {

        IProductCategoryService _productCategoryService;
        IProductService _productService;
        ICommonService _commonService;
        IManufactureService _manufactureService;

        public HomeController(IProductCategoryService productCategoryService,
            ICommonService commonService,
            IProductService productService,
            IManufactureService manufactureService)
        {
            _productCategoryService = productCategoryService;
            _productService = productService;
            _commonService = commonService;
            _manufactureService = manufactureService;
        }

        // Sản phẩm mới nhất
        // Sản phẩm mổi bật (sản phẩm hot)
        // Sản phẩm bán chạy
        public ActionResult Index()
        {
            var lasttestProduct = _productService.GetLastest(8);
            var hotProduct = _productService.GetHotProduct(8).ToList();
            var promotionProduct = _productService.GetPromotionProduct(8).ToList();
            var manufactures = _manufactureService.GetAll().ToList();
            var lstCategoryAll= _productCategoryService.GetAll().ToList();

            var lastestproductViewModal = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lasttestProduct);
            var hotProductViewModal = Mapper.Map<List<Product>, List<ProductViewModel>>(hotProduct);
            var promotionProductViewModal = Mapper.Map<List<Product>, List<ProductViewModel>>(promotionProduct);
            var manufacturesViewModal = Mapper.Map<List<Manufacture>, List<ManufactureViewModel>>(manufactures);
            //var lstCategoryParentViewModal = Mapper.Map<List<ProductCategory>, List<ProductCategoryViewModel>>(lstCategoryParent);

            var homeViewModel = new HomeViewModel();

            homeViewModel.LastestProduct = lastestproductViewModal;
            homeViewModel.HotProduct = hotProductViewModal;
            homeViewModel.PromotionProduct = promotionProductViewModal;
            homeViewModel.Manufactures = manufacturesViewModal;

            var listManufacture = new List<ManufacturetInTabViewModel>();
            foreach (var item in lstCategoryAll.Where(x=>x.ParentID ==  null))
            {
                var child = lstCategoryAll.Where(x => x.ParentID == item.ID);
                if (child.Count() > 0)
                {
                    var list = new List<ManufactureViewModel>();
                    foreach (var xitem in child)
                    {
                        var listManufactureByCategory = _manufactureService.GetAllInCategoryID(xitem.ID);
                        if (listManufactureByCategory.Count() > 0)
                        {
                            var modelManufacture = Mapper.Map<IEnumerable<Manufacture>, IEnumerable<ManufactureViewModel>>(listManufactureByCategory);

                            list.AddRange(modelManufacture);
                        }                        
                    }
                    var itemManufacture = new ManufacturetInTabViewModel()
                    {
                        Id = item.ID,
                        CategoryName = item.Name,
                        ListManufacture = list
                    };

                    if (itemManufacture.ListManufacture.Count() > 0)
                    {
                        listManufacture.Add(itemManufacture);
                    }                    
                }                
            }

          homeViewModel.ManufacturetInTab = listManufacture;

            return View(homeViewModel);
        }

        [HttpGet]
        public JsonResult LoadProductManufacture(string id, string cateID)
        {
            if (!string.IsNullOrEmpty(id) || !string.IsNullOrEmpty(cateID))
            {
                var listChilCate = _productCategoryService.GetProductCategoryByParentID(new Guid(cateID));
                var lstProductByManufac = new List<Product>();
                foreach (var item in listChilCate)
                {
                    var lstProduct = _productService.GetByManufacture(new Guid(id), item.ID, 8);
                    lstProductByManufac.AddRange(lstProduct);
                }

                
                var model = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lstProductByManufac);

                return Json(new
                {
                    data = model,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {                  
                    status = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetProductDetail(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var productModel = _productService.GetById(new Guid(id));
                var viewModel = Mapper.Map<Product, ProductViewModel>(productModel);
                return Json(new
                {
                    data = viewModel,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    status = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Header()
        {
            var productCategory = _productCategoryService.GetAll();
            var productCategoryViewModal = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(productCategory);
            var menuViewModel = new MenuViewModel();

            menuViewModel.ProductCategory = productCategoryViewModal;
            return PartialView(menuViewModel);
        }

        public ActionResult Footer()
        {
            return PartialView();
        }
    }
}