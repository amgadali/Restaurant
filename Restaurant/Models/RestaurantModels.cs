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

        public virtual Collection<OrderMenuItems> OrderMenuItems { get; private set; }

        [Display(Name = "Status")]
        public virtual OrderStatus Status { get; set; }

        public Order()
        {
            OrderMenuItems = new Collection<OrderMenuItems>();

        }
    }

    public class OrderMenuItems
    {

        //public long Id { get; set; }
        [Required]
        [Key, Column(Order = 0)]
        public int Order_Id { get; set; }

        [ForeignKey("Order_Id")]
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