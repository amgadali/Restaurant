using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Restaurant.Models
{
    public class UserAddress
    {
        public int ID { get; set; } 

        [Required(ErrorMessage = "*")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        public double lat { get; set; }
        public double lon { get; set; }

        [Required]
        public string ApplicationUser_Id { get; set; }

        [ForeignKey("ApplicationUser_Id")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }

    public class MenuItem
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Image")]
        public string Image { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "price")]
        public double Price { get; set; }

        public virtual Collection<OrderMenuItems> OrderMenuItems { get; private set; }
        
        public MenuItem()
        {
            OrderMenuItems = new Collection<OrderMenuItems>();

        }
    }

    public class Order
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Date")]
        public DateTime OrderDate { get; set; }

        //[Display(Name = "Status")]// 0 new - 1 preparing - 2 delivering- 3 Delevered - 4 canceled
       // public int OrderStatus_ID { get; set; }

        public string  Client_Id { get; set; }

        [ForeignKey ("Client_Id")]
        public virtual ApplicationUser Client { get; set; }

        // to establish the relation between the order and orderMenuItem
        // this relation is one order to many orderMenuItem
        // thats why we use collection of order meny item as type
        public virtual Collection<OrderMenuItems> OrderMenuItems { get; private set; }

        [Display(Name = "Status")]
        public virtual OrderStatus Status { get; set; }

        public Order()
        {
            OrderMenuItems = new Collection<OrderMenuItems>();

        }
    }


    //to establish the relation between the order and menuItem which is many to many
    // we devided it to 2 relations (order to orderMenuItem ) and (MenuItem to orderMenuItem)
    // this relation is one order to many orderMenuItem
    // thats why we use collection of order meny item as type
    public class OrderMenuItems
    {

       
        //this is a forign key to relate to Order.Id
        [Required]
        [Key, Column(Order = 0)]
        public int Order_Id { get; set; }

        // to establish a relation to the order table 
        [ForeignKey("Order_Id")]// this line to let this relation uses the column OrderMenuItems.Order_Id as ForeignKey 
        public virtual Order Order { get; set; }

        [Required]
        [Key, Column(Order = 1)]
        public int MenuItem_Id { get; set; }

        [ForeignKey("MenuItem_Id")]
        public virtual MenuItem MenuItem { get; set; }

        [DefaultValue(1)]//   
        [Display(Name = "Count")]
        public int Count { get; set; }

        public double Price { get; set; }
    }

    public class OrderStatus
    {
        [KeyAttribute()]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public string Name { get; set; }
    }
   

}