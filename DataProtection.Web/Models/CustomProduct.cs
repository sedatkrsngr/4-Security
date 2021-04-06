using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataProtection.Web.Models
{
    public partial class Product// Bu şekilde şifrelenmiş Id için ayrı bir class oluşturup veritabandan da aramasın diye notmapped eklediks
    {
        [NotMapped]
        public string sifrelenmisId { get; set; }
    }
}
