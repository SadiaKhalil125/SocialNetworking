namespace Bonded.Models.Interfaces
{
    public interface IUserRepository
    {
        User GetUserById(int id);
        User GetUserByEmail(string email);
        bool ValidateUser(string email, string password);
        IEnumerable<User> GetAllUsers();
        Task<bool> AddUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> UpdateUserAsync(User user);
        //bool AddUserAsync(User user);
        //bool UpdateUserAsync(User user);
        //bool DeleteUserAsync(int id);
        public List<User> GetUsersByIds(List<int> userIds);
        public List<User> GetUsersByName(string searchTerm);
        Task<User> GetUserByIdAsync(int id);
        bool ResetPassword(string email, string newPassword);
        //bool ResetPasswordAsync(string email, string newPasswordHash);
        public string GetUserPassword(string email);
        }
}
