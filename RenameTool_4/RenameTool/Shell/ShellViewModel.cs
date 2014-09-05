namespace RenameTool.Shell
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using Caliburn.Micro;

    [Export(typeof(IShell))]
    public class ShellViewModel : IShell
    {
        #region Constructor
        // The Import is done to fill the menu.
        [ImportingConstructor]
        public ShellViewModel([ImportMany]IEnumerable<IScreen> screen) { }
        #endregion Constructor

        [Import]
        public Modules.Menu.MenuViewModel MenuRegion { get; set; }

        [Import]
        public Modules.Tabs.TabsViewModel Tabs { get; set; }

    } // ShellViewModel

} // namespace QuickRenameTool.Shell