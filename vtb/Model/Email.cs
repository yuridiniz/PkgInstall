using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace vtb.Model
{
    public class Email
    {
        [XmlAttribute("Valor")]
        public string Valor { get; set; }
    }
}
