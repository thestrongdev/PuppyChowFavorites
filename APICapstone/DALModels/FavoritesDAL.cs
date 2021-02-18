using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APICapstone.DALModels
{
    public class FavoritesDAL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FaveId { get; set; }
        public string Title { get; set; }
        public string Ingredients { get; set; }

        public string href { get; set; }

        [ForeignKey("AspNetUsers")]
        public string Id { get; set; }

        public IdentityUser User { get; set; }
    }
}
