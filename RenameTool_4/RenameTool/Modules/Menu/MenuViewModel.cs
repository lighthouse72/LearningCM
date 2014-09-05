namespace RenameTool.Modules.Menu
{
    using RenameTool.Framework;
    using RenameTool.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Text;
    using Caliburn.Micro;


    [Export(typeof(IMenuManager))]
    [Export(typeof(MenuViewModel))]
    public class MenuViewModel : IMenuManager
    {
        private ObservableCollection<MenuItem> items = new ObservableCollection<MenuItem>();

        public ObservableCollection<MenuItem> Items
        {
            get { return items; }
            set { items = value; }
        }

        [ImportingConstructor]
        public MenuViewModel([ImportMany] IEnumerable<IMenuItem> actionItems)
        {
            this.Items.Add(new MenuItem("File", "_File"));
            //this.AddWithParent("File", new MenuItem("Exit", Exit));
        }

        private IEnumerable<IResult> Exit()
        {
            //_shell.Close();
            //yield break;
            //return null;
            throw new NotImplementedException();
        }

        #region IMenuManager Members

        public IMenuItem this[string index]
        {
            get
            {
                int cnt = 0;
                IMenuItem item = null;

                while (cnt < items.Count && item == null)
                {
                    if (items[cnt].Name == index)
                        item = items[cnt];
                    else if (items[cnt].HasChilderen) { }

                }
                return item;
            }
            private set
            {
                throw new NotImplementedException();
            }
        }
        private IMenuItem GetByName(string name)
        {
            int cnt = 0;
            IMenuItem item = null;

            while (cnt < items.Count && item == null)
            {
                if (items[cnt].Name == name)
                    item = items[cnt];
                else if (items[cnt].HasChilderen)
                    item = this.GetByName(name, items[cnt]);
            }

            return item;
        }

        private IMenuItem GetByName(string name, IMenuItem item)
        {
            int cnt = 0;

            while (cnt < items.Count && item == null)
            {
                if (items[cnt].Name == name)
                    item = items[cnt];
                else if (items[cnt].HasChilderen)
                    item = this.GetByName(name, items[cnt]);
            }

            return item;
        }

        public void Add(MenuItem item)
        {
            Items.Add(item);
        }

        public void AddWithParent(string parentName, MenuItem item)
        { this.AddWithParent(parentName, item, null); }

        public void AddWithParent(string parentName, MenuItem item, IScreen screen)
        {
            //int cnt = 0;
            //bool isItemFound = false;

            //while (cnt < items.Count && !isItemFound)
            //{
            //    if (items[cnt].Name == parentName)
            //    {
            //        isItemFound = true;
            //        items[cnt].Add(item);
            //    }
            //}

            IMenuItem parentItem;

            parentItem = this[parentName];
            if (parentItem != null)
            {
                parentItem.Add(item);
            }

        }

        #endregion
    }
}
