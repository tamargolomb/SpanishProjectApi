using System.ComponentModel.DataAnnotations;

namespace DTO.User
{

    public class AddUserDTO
    {
        [Required(ErrorMessage = "user name is required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "user name must be between 2 and 20 characters")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "user email is required")]
        [EmailAddress]
        public string UserEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "user password is required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "user password must be between 2 and 20 characters")]
        public string UserPassword { get; set; } = string.Empty;
    }

}
