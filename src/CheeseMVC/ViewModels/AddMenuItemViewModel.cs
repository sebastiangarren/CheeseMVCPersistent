using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.OData.Query.SemanticAst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace CheeseMVC.ViewModels
{
    public class AddMenuItemViewModel
    {
        
        public Menu Menu { get; set; }

        public int MenuID { get; set; }

        [Required(ErrorMessage = "Must Choose A Valid Cheese")]
        public int CheeseID { get; set; }

        public List<SelectListItem> Cheeses { get; set; }
    

        public AddMenuItemViewModel()
        {

        }

        public AddMenuItemViewModel(Menu menu, IEnumerable<Cheese> cheeses)
        {

            Menu = menu;

            Cheeses = new List<SelectListItem>();
            
            foreach (Cheese cheese in cheeses)
            {
                Cheeses.Add(new SelectListItem
                {
                    Value = cheese.ID.ToString(),
                    Text = cheese.Name
                });

            }
           
        }


    }
}
