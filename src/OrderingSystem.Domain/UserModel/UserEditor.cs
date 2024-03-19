using CloudyWing.OrderingSystem.DataAccess.Entities;

namespace CloudyWing.OrderingSystem.Domain.Services.UserModel {
    public class UserEditor {
        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Name { get; set; }

        public Role Role { get; set; }
    }
}
