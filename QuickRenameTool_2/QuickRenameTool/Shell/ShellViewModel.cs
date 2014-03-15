namespace QuickRenameTool.Shell
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Caliburn.Micro;

    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShell
    {
        // This attribute is needed by MEF
        [ImportingConstructor]
        public ShellViewModel([ImportMany]IEnumerable<IScreen> s)
        {
            // Using the wrong parameters for the constructor, will cause errors in
            // the Bootstrapper GetInstance. Cannot find contract for IShell.

            // Save the screens in the Conductor base.
            Items.AddRange(s);
            
        }

        /// <summary>
        /// Deactives the specified item
        /// </summary>
        /// <param name="item">The item to close.</param>
        /// <param name="close">Indicates whether or not to close the item after deactivating it.</param>
        /// <remarks>If the last tab is closed, the app will close.</remarks>
        public override void DeactivateItem(IScreen item, bool close)
        {
            base.DeactivateItem(item, close);

            // Check if all tabs are closed. If so try to close the app.
            if (Items.Count == 0)
                TryClose();
        }

    } // ShellViewModel
} // namespace QuickRenameTool.Shell