namespace TabControlSample.Shell
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Caliburn.Micro;

    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShell
    {
        [ImportingConstructor]
        public ShellViewModel() { }

        int count = 1;
        public void OpenTab()
        {
            ActivateItem(new Tabs.TabViewModel
            {
                DisplayName = "Tab " + count++
            });
        }

    }
}