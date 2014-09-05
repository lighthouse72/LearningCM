namespace RenameTool.Modules.Tabs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections.ObjectModel;
    using Caliburn.Micro;

    public interface ITabsManager
    {
        void ActivateItem(IScreen screen);
        //void DeactivateItem(IScreen screen, bool close);
    }
}
