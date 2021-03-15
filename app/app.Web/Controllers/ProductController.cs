using app.Domain.Entities.Product;
using app.Service.Services;
using app.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace app.Web.Controllers
{
    [Authorize]
    [Route("products")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        /// <summary>
        /// Product controller constructor
        /// </summary>
        /// <param name="productService">injected product service</param>
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Product list page
        /// </summary>
        /// <returns>products view with a list of product viewmodels</returns>
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var model = new ProductListVM();

            var products = await _productService.GetAllProducts();

            if (products.Any())
            {
                model.Products = products.Select(p => new ProductVM(p)).ToList();
            }

            return View(model);
        }

        /// <summary>
        /// Product details page
        /// </summary>
        /// <param name="slug">product url slug</param>
        /// <returns>details view with product details viewmodel</returns>
        [HttpGet("{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            var product = await _productService.GetProduct(slug);

            if (product == null) // if product does not exist, redirect to products page
            {
                return RedirectToAction(nameof(Index));
            }

            var model = new ProductDetailsVM(product);

            return View(model);
        }

        /// <summary>
        /// GET: product create
        /// </summary>
        /// <returns>product create page with create product form data</returns>
        [HttpGet("create")]
        public ActionResult Create()
        {
            var model = new ProductSaveVM()
            {
                ColorList = _productService.GetProductColors().Select(o => new ColorOptionVM(o)).ToList(),
            };

            return View("Save", model);
        }

        /// <summary>
        /// POST: product create
        /// </summary>
        /// <param name="vm">created product viewmodel</param>
        /// <returns>redirects to products page if successful. Returns current page if error</returns>
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm]ProductSaveVM vm)
        {
            if (ModelState.IsValid)
            {
                var product = BuildProductDTO(vm);

                var createResult = await _productService.CreateProduct(product);

                if (createResult)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            vm.ColorList = _productService.GetProductColors().Select(o => new ColorOptionVM(o)).ToList();
            vm.Error = true; // since we made it here, flag the error and return to create page

            return View("Save", vm);
        }

        /// <summary>
        /// GET: edit product
        /// </summary>
        /// <param name="slug">url slug of product to edit</param>
        /// <returns>edit page if product exists, products page if product does not exist</returns>
        [HttpGet("edit/{slug}")]
        public async Task<IActionResult> Edit(string slug)
        {
            var product = await _productService.GetProduct(slug);

            if (product != null)
            {
                var model = new ProductSaveVM(product)
                {
                    Color = _productService.GetColorId(product.Color),
                    ColorList = _productService.GetProductColors().Select(o => new ColorOptionVM(o)).ToList(),
                };

                return View("Save", model);
            }

            return RedirectToAction(nameof(Index)); //redirect to products page if product does not exist
        }

        /// <summary>
        /// POST: edit product
        /// </summary>
        /// <param name="vm">edit product viewmodel</param>
        /// <returns>redirects to products page if successful. Returns current page if error</returns>
        [HttpPost("edit/{slug}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ProductSaveVM vm)
        {
            if (ModelState.IsValid)
            {
                var product = BuildProductDTO(vm, true);

                var editResult = await _productService.UpdateProduct(product);

                if (editResult)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            vm.ColorList = _productService.GetProductColors().Select(o => new ColorOptionVM(o)).ToList();
            vm.Error = true; // since we made it here, flag the error and return to save page

            return View("Save", vm);
        }

        /// <summary>
        /// POST: delete product
        /// </summary>
        /// <param name="vm">deleted product vm</param>
        /// <returns>redirects to products page if successful, returns to details page if not</returns>
        [HttpPost("delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] ProductDeleteVM vm)
        {
            if (ModelState.IsValid)
            {
                var deleteResult = await _productService.DeleteProduct(vm.Id);

                if (deleteResult)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction("details", "product", new { slug = vm.Slug});
        }

        /// <summary>
        /// Build product DTO to be sent to the service layer -> repo layer
        /// </summary>
        /// <param name="vm">vm from create/edit page</param>
        /// <param name="edit">determines if this is a new product or existing (set on the controller)</param>
        /// <returns>product dto</returns>
        private Product BuildProductDTO(ProductSaveVM vm, bool edit = false)
        {
            var dto = new Product
            {
                Id = !edit ? 0 : vm.Id,
                Name = vm.Name,
                Color = _productService.GetColorName(vm.Color),
                Price = vm.Price,
                Slug = !edit ? Guid.NewGuid().ToString() : string.Empty //slug is never updated on an edit so setting to empty
            };

            return dto;
        }
    }
}
