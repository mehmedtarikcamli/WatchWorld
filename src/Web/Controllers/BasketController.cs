using Microsoft.AspNetCore.Mvc;
using Web.Interfaces;

namespace Web.Controllers
{
	public class BasketController : Controller
	{
		private readonly IBasketViewModelService _basketViewModelService;

		public BasketController(IBasketViewModelService basketViewModelService)
		{
			_basketViewModelService = basketViewModelService;
		}

		public async Task<IActionResult> Index()
		{
			var basket = await _basketViewModelService.GetBasketViewModelAsync();
			return View(basket);
		}

		[HttpPost]
		public async Task<IActionResult> AddItem(int productId, int quantity = 1)
		{
			var basket = await _basketViewModelService.AddItemToBasketAsync(productId, quantity);
			return Json(basket);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Empty()
		{
			await _basketViewModelService.EmptyBasketAsync();
			// tempdata içi string farklı
			TempData["Message"] = "Your Basket is now empty.";
			return RedirectToAction("Index");
		}

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> RemoveItem(int productId)
		{
			await _basketViewModelService.RemoveItemAsync(productId);
			TempData["Message"] = "Item removed from the basket.";
			return RedirectToAction("Index");
		}

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Update([ModelBinder(Name = "quantities")] Dictionary<int, int> quantities)
		{
			// Görev : basket view model servisteSetQunatitiesAsync metodunu oluşturup burada çağırın.
			await _basketViewModelService.SetQuantitiesAsync(quantities);
			TempData["Message"] = "Products in the cart have been updated.";
			return RedirectToAction("Index");
		}
	}
}
