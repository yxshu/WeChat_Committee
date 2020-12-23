using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WeChat_Committee.Model
{
    public class BaseMenu
            {
        MENUTYPE menutype;
        string name;

        public BaseMenu(MENUTYPE menutype, string name)
        {
            this.menutype = menutype;
            this.name = name;
        }

        public MENUTYPE Menutype { get => menutype; set => menutype = value; }
        public string Name { get => name; set => name = value; }
    }
}