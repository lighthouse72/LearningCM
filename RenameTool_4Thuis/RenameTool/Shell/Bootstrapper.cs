namespace RenameTool.Shell
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using System.Linq;
    using System.Windows;
    using Caliburn.Micro;

    public class Bootstrapper : BootstrapperBase
    {
        private CompositionContainer container;

        public Bootstrapper()
        {
            Start();
        }

        protected override void Configure()
        {
            //// Silverlight
            //container = CompositionHost.Initialize(
            //    new AggregateCatalog(
            //        AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()
            //        )
            //    );

            // WPF
            container = new CompositionContainer(
                new AggregateCatalog(
                    AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()
                    )
                );

            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(container);

            container.Compose(batch);
            AddCustomConventions();
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            var contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = container.GetExportedValues<object>(contract);

            if (exports.Any())
                return exports.First();

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override void BuildUp(object instance)
        {
            container.SatisfyImportsOnce(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<IShell>();
        }

        private static void AddCustomConventions()
        {
            ConventionManager.AddElementConvention<PropertyTools.Wpf.DirectoryPicker>
                (PropertyTools.Wpf.DirectoryPicker.DirectoryProperty, "Directory", "DataContextChanged")
                .ApplyBinding = (viewModelType, path, property, element, convention) =>
                {
                    var bindableProperty = convention.GetBindableProperty(element);
                    if (!ConventionManager.SetBindingWithoutBindingOrValueOverwrite(viewModelType, path, property, element, convention, bindableProperty))
                        return false;

                    ConventionManager.ConfigureSelectedItem(element, PropertyTools.Wpf.DirectoryPicker.DirectoryProperty, viewModelType, path);

                    return true;
            };
        }
    }
}
