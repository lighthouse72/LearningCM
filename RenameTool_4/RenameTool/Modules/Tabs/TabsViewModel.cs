namespace RenameTool.Modules.Tabs
{
    using System.ComponentModel.Composition;
    using Caliburn.Micro;

    [Export(typeof(ITabsManager))]
    [Export(typeof(TabsViewModel))]
    public class TabsViewModel : Conductor<IScreen>.Collection.OneActive, ITabsManager
    {
        [ImportingConstructor]
        public TabsViewModel()
        {

        }

        /// <summary>
        ///  Activates the specified item.
        /// </summary>
        /// <param name="screen">Screen that need to be acivated</param>
        /// <remarks>If screen is not in items list is will be added, other wise it will be actived.</remarks>
        public override void ActivateItem(Caliburn.Micro.IScreen screen)
        {
            base.ActivateItem(screen);
        }

        ///// <summary>
        ///// Deactives the specified item
        ///// </summary>
        ///// <param name="item">The item to close.</param>
        ///// <param name="close">Indicates whether or not to close the item after deactivating it.</param>
        ///// <remarks>If the last tab is closed, the app will close.</remarks>
        //public override void DeactivateItem(IScreen screen, bool close)
        //{
        //    base.DeactivateItem(item, close);

        //    int index = this.tabItems.IndexOf(screen);
        //    this.tabItems.RemoveAt(index);
        //}

    }
}
