﻿using System.ComponentModel.DataAnnotations;

namespace TiendaVirtual.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
