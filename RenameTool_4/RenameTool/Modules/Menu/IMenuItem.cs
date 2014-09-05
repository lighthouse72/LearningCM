namespace RenameTool.Modules.Menu
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Caliburn.Micro;

    public interface IMenuItem
    {
        string DisplayText { get; }
        string Name { get; }
        void Add(params MenuItemBase[] items);
    }
}
