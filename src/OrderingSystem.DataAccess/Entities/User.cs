﻿namespace CloudyWing.OrderingSystem.DataAccess.Entities {
    public class User {
        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Name { get; set; }

        public Role Role { get; set; }
    }
}
