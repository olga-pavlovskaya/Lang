using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Logic;

namespace Lang
{
    public class UserSession
    {
        public RegExpTree Tree { get; set; }
        public ComplexMachine Machine { get; set; }
        public Grammar Grammar { get; set; }
        public MagazineMachine MagMachine { get; set; }
        public Helper Helper { get; set; }
    }
}