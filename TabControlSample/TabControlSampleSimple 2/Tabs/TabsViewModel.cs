namespace TabControlSample.Tabs
{
    using System.ComponentModel.Composition;
    using Caliburn.Micro;

    [Export(typeof(TabsViewModel))]
    public class TabsViewModel : Conductor<IScreen>.Collection.OneActive
    {
        /// <summary>
        ///  Constructor is needed by mef
        /// </summary>
        [ImportingConstructor]
        public TabsViewModel()
        {

        }
    }
}
