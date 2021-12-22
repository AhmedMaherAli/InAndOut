using InAndOut.Data;
using InAndOut.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InAndOut.Controllers
{
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        [BindProperty]
        public Item item { get; set; }
        public ItemController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Authorize]
        public IActionResult Index()
        {
            IEnumerable<Item> items = _dbContext.Item;
            return View(items);
        }


        public IActionResult Create()
        {

            item = new Item();
            return View(item);
        }
        public IActionResult Delete(int id)
        {
            Item ItemFromDb = _dbContext.Item.FirstOrDefault(u => u.Id == id);
            if (ItemFromDb == null)
            {
                return Json(new { success=false, message= "Error while Deleting" });
            }
            _dbContext.Item.Remove(ItemFromDb);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int?id)
        {
            if(ModelState.IsValid)
            {
                _dbContext.Item.Add(item);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(item);
        }


    }
}
