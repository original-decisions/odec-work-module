using System;
using System.ComponentModel.DataAnnotations;
using odec.Framework.Generic;

namespace odec.Server.Model.Work.Models
{
    public class PortfolioItem:Glossary<int>
    {
        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }
        public DateTime? ProjectFinishDate { get; set; }
        [Required]
        public int UserId { get; set; }
        public User.User User { get; set; }

    }
}