using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan
{
    class MenuItems
    {
        public string name { get; set; }
        public bool active { get; set; }

        public event EventHandler Click;

        public MenuItems(string name)
        {
            this.name = name;
            this.active = true;
        }

        public void OnClick()
        {
            if (Click != null)
                Click(this, null);
        }
    }
}
