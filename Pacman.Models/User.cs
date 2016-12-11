﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Pacman.Models
{
    public class User
    {
        public enum Roles
        {
            user,
            admin
        }

        [Key]
        public int Id { get; protected set; }

        [Required]
        public string FirstName { get; protected set; }

        [Required]
        public string LastName { get; protected set; }

        public DateTime? BurthDate { get; protected set; }

        public virtual Country Country { get; protected set; }
        public int CountryId { get; protected set; }
        
        public virtual City City { get; protected set; }
        public int CityId { get; protected set; }

        public virtual PlayerStatistic PlayerStatistic { get; set; }
    }
}