namespace TabControlSample.Shell
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using Caliburn.Micro;

    [Export(typeof(IShell))]
    public class ShellViewModel : IShell
    {
        [ImportingConstructor]
        public ShellViewModel()
        {
        }

        int count = 1;

        public void OpenTab()
        {
            Tabs.ActivateItem(new Tabs.TabViewModel
            {
                DisplayName = "Tab " + count++
            });
        }

        [Import]
        public Tabs.TabsViewModel Tabs { get; set; }

    }
}