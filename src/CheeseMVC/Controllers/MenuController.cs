using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Data;
using CheeseMVC.Models;
using Microsoft.EntityFrameworkCore;
using CheeseMVC.ViewModels;

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private readonly CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<Menu> menus = context.Menu.ToList();

            return View(menus);
        }   

        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();

            return View(addMenuViewModel);

        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu newMenu = new Menu
                {
                    Name = addMenuViewModel.Name,
                };

                context.Menu.Add(newMenu);
                context.SaveChanges();

                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            }

            return View(addMenuViewModel);

        }

        public IActionResult ViewMenu(int id)
        {
            Menu theMenu = context.Menu.Single(menu => menu.ID == id);

            List<CheeseMenu> items = context
                .CheeseMenu
                .Include(item => item.Cheese)
                .Where(cm => cm.MenuID == id)
                .ToList();

            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel
            {
                Menu = theMenu,
                Items = items
            };

            return View(viewMenuViewModel);

        }

        public IActionResult AddItem(int id)
        {
            Menu theMenu = context.Menu.Single(menu => menu.ID == id);

            AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel(theMenu, context.Cheeses.ToList());

            return View(addMenuItemViewModel);

        }

        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            if (ModelState.IsValid)
            {
                int cheeseID = addMenuItemViewModel.CheeseID;
                int menuID = addMenuItemViewModel.MenuID;

                IList<CheeseMenu> existingitems = context.CheeseMenu
                    .Where(cm => cm.CheeseID == cheeseID)
                    .Where(cm => cm.MenuID == menuID).ToList();

                if (existingitems.Count == 0)
                {
                    CheeseMenu newMenu = new CheeseMenu
                    {
                        Menu = context.Menu.Single( c =>c.ID == menuID),
                        Cheese = context.Cheeses.Single( c => c.ID == cheeseID),
                    };

                    context.CheeseMenu.Add(newMenu);
                    context.SaveChanges();

                    return Redirect("/Menu/ViewMenu/" + addMenuItemViewModel.MenuID);

                }
                return Redirect("/Menu/ViewMenu/" + addMenuItemViewModel.MenuID);
            }


            return View(addMenuItemViewModel);
        }
    }

}