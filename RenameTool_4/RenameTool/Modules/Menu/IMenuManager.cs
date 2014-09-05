namespace RenameTool.Modules.Menu
{
    using System.Collections.ObjectModel;

    public interface IMenuManager
    {
        void Add(MenuItem item);
        void AddWithParent(string parentName, MenuItem item);
    }
}
