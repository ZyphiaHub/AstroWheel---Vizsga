﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AstroWheelAPI.Models
{
    [Index(nameof(CharacterIndex), IsUnique = true)]
    public class Character
    {
        [Key]
        public int CharacterId { get; set; }
        [Required]
        [StringLength(50)]
        public string AstroSign { get; set; } = string.Empty;
        [Required]
        [StringLength(10)]
        public string Gender { get; set; } = string.Empty;
        public int CharacterIndex { get; set; }
    }
}
