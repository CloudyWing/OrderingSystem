using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudyWing.OrderingSystem.DataAccess.Entities;

namespace CloudyWing.OrderingSystem.Domain.OrderModel {
    public class OrderQueryEntity {
        public Order? Order { get; set; }

        public User? User { get; set; }
    }
}
