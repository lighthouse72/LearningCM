namespace RenameTool.ViewModels
{
    using RenameTool.Framework;
    using RenameTool.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Text;


     [Export(typeof(IMenuManager))]
    [Export(typeof(MenuViewModel))]
    public class MenuViewModel : ActionItemManager, IMenuManager
    {
        [ImportingConstructor]
        public MenuViewModel([ImportMany] IEnumerable<IActionItem> actionItems)
        {
            this.ShowItem(new ActionItem("File", null));
            foreach (var menuViewModel in actionItems)
                Items.Add(menuViewModel);
        }
        public bool test() { return false; }
    }
}
