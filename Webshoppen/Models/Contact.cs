using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Webshoppen.Models
{
    public class Contact
    {
        public Contact()
        {
            Image = "no-image.jpg";
        }

        public int ID { get; set; }
        [AllowHtml]
        public string PageContent { get; set; }
        public string Image { get; set; }
    }
}