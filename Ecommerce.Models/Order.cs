using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]  
        public User User { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime SendDate { get; set; }

        public string SendNumber { get; set; }

        public string Carrier { get; set; }

        [Required]
        public double TotalOrder { get; set; }

        [Required]
        public string Stateorder { get; set; }

        public string statePay { get; set; }

        public DateTime PayDate { get; set; }

        public DateTime PayDateMaximum { get; set; }

        public string TransactionId { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string NameClient { get; set; }

    }
}
