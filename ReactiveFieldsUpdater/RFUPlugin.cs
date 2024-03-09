using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace ReactiveFieldsUpdater
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Reactive Fields Updater"),
        ExportMetadata("Description", "Create and update Power Apps fields in a smart and reactive way"),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "/9j/4AAQSkZJRgABAQEASABIAAD/2wBDAAoHBwgHBgoICAgLCgoLDhgQDg0NDh0VFhEYIx8lJCIfIiEmKzcvJik0KSEiMEExNDk7Pj4+JS5ESUM8SDc9Pjv/2wBDAQoLCw4NDhwQEBw7KCIoOzs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozv/wAARCAAgACADASIAAhEBAxEB/8QAGQAAAwEBAQAAAAAAAAAAAAAAAAUGBAED/8QAJxAAAgEEAQIGAwEAAAAAAAAAAQIDAAQFESEGEhMiMUFhghUjMnL/xAAYAQEAAwEAAAAAAAAAAAAAAAABAAIFBv/EABsRAAICAwEAAAAAAAAAAAAAAAECAAMRITFB/9oADAMBAAIRAxEAPwCPY6Y0wxuBy2YVnsLJ5UThpOFQfY6FYApaUKFLEnWh6mtXVmfupbsY8wNYwWn647MHYh1wfblid7NdLY5UamwxI5PTJ4LK4dUa/s3hR/5fhlP2GxS8HmnnQmSR8smKyFx24++BjmjcbRyR5f8AJ3rRFKr2K3gvporWc3EKOVSUr294HvrZorsLHBkUnODC0vJsffxXduVEsLh0LKGAI+DVB1BiH64vPzGIkimuJQpltSypLCwAB4Ou9eOCOamWG2Nc0aXrDb9gyZ37KC06ag6fVrrqPwx2qfDsVlBknbXG+0+RR6k/FT/q29a+KNGgDmlFK9MsAR2f/9k="),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "/9j/4AAQSkZJRgABAQEASABIAAD/2wBDAAoHBwgHBgoICAgLCgoLDhgQDg0NDh0VFhEYIx8lJCIfIiEmKzcvJik0KSEiMEExNDk7Pj4+JS5ESUM8SDc9Pjv/2wBDAQoLCw4NDhwQEBw7KCIoOzs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozv/wAARCABQAFADASIAAhEBAxEB/8QAHAAAAQUBAQEAAAAAAAAAAAAAAgABBQYHAwQI/8QAMxAAAQIEAwUGBQUBAAAAAAAAAQIDAAQFEQYSITFTcZHRIjJBUXLBBxNhgbEUFUJSoWP/xAAaAQEAAwEBAQAAAAAAAAAAAAAAAQMEAgUG/8QAJREAAgICAQIGAwAAAAAAAAAAAAECAxESMRMhBDJBUWGxUoHw/9oADAMBAAIRAxEAPwCoqypSnsA3Tckk9YHMndp5nrDubEemAj6lHtBZk7tPM9YWZO7TzPWBhRICzJ3aeZ6wsyd2nmesd5enT04LyslMPjzaaUv8CGmKfOyYvNScwwP+rSkfkRztHOMkZRxzJ3aeZ6wsyd2nmesDCjokLMndp5nrD9kpV2ACBcanzEBBJ7q+HuIAdzYj0wEG5sR6YCCCO0rKvzs03KyzSnXnVBKEJGpMaVJ4aw7gmQbqGIlompxXdatnF/JKfHif8iCwBU6BQjNVKpTNpwAoYbDalEJtckEC1zs5+cVuuVqardSeqE652l7B4IT4AfQRjsU7ZuHEVz8lElKcteEXWc+LUz8wpp1LYbaGifnkk24CwEKS+LT6l5KnS2XGVaK+QSDbgq4POMpXMKmH7oUUoQdLeMd2XyhVlG6TFap8O+2v7Cprfoa1PYWw/jKnLqWGlol5sd5kDKknyUn+J+o0jNZiXelJhyXmG1NutKKVoUNQRExg5+qsYhZXR0hx+xKmisJDqPEakX01+1/CLz8QcGzVYeYqdMlwqYUnLMIK0p0A0Vcm1xs5RMbOhZ05Synx8HKl05aN9jKYJPdXw9xDEWJGmnkbw6e6vh7iN5pHc2I9MBBubEemGSAVgKOVJOptewiFwFwTtLwlPVXDs7WGR2ZbuItq7bVVuAin1J5ecNWsm1+MfQuD6tR52hfIozboZkgGy24kBZ0vfb46/e8YtiVmnTVVmf25DrcupZUhDqcqmz4pt5A3jBC2y1zg1j+4M0Zym5JoqoUUquNse5GoBIsfKPMhv5T5Q6LKGyPS2lTi8qRrHMEyyskKVPTEjOtTMuspcZUFoV5Ee0aji7HbE1hGWRT3MsxUUWdSDq0kaKH3OnC8ZewwoqSy0kqWsgADUqJiexDhGew1Ly7s9MSylPmyW21EqFhqTccOcXzqrco7vuJxg5LPJAwSe6vh7iBgk91fD3Eay0dzYj0wEG5sR6YCCCJrC2InsNVhM4gFbKxkeb/sn6fUbRF1xDg+Txax+/YbfaU68LuN3slw+PpV5gxmEe+lVupUSY+fTptbCj3gNUq4g6GM1tMnLet4l9lU623tF4ZwqWH56UfCahTn2ltnapBsfvsIg6dQKjPOBqQp77ylHalBt9ydB94usp8W6i2kJm6bLPkfybUUX/MNOfFupOJKZSnSzBOxS1FZH4iva/PkWffJzm38SSw/hORwbLGvYifbEw0Lttg5ggny/sr8f7FFxNiB/ElYXOujI2BkZb/ogbPv4mPNVKzUa1M/qKjNLfX4X0CR5ADQR4YsqpalvN5l9HUK2ntJ5YoJPdXw9xAwSe6vh7iNJaO5sR6YCOisqko7YFk2IIMDlTvE8j0iEAYUFlTvE8j0hZU7xPI9InIBhQWVO8TyPSFlTvE8j0hkAwoLKneJ5HpCyp3ieR6QyAYJPdXw9xCyp3ieR6Q/ZCVdsEkbAD5iAP/Z"),
        ExportMetadata("BackgroundColor", "Lavender"),
        ExportMetadata("PrimaryFontColor", "Black"),
        ExportMetadata("SecondaryFontColor", "Gray")]
    public class RFUPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new RFUPluginControl();
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        public RFUPlugin()
        {
            // If you have external assemblies that you need to load, uncomment the following to 
            // hook into the event that will fire when an Assembly fails to resolve
            // AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
        }

        /// <summary>
        /// Event fired by CLR when an assembly reference fails to load
        /// Assumes that related assemblies will be loaded from a subfolder named the same as the Plugin
        /// For example, a folder named Sample.XrmToolBox.MyPlugin 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly loadAssembly = null;
            Assembly currAssembly = Assembly.GetExecutingAssembly();

            // base name of the assembly that failed to resolve
            var argName = args.Name.Substring(0, args.Name.IndexOf(","));

            // check to see if the failing assembly is one that we reference.
            List<AssemblyName> refAssemblies = currAssembly.GetReferencedAssemblies().ToList();
            var refAssembly = refAssemblies.Where(a => a.Name == argName).FirstOrDefault();

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (refAssembly != null)
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToLower();
                string folder = Path.GetFileNameWithoutExtension(currAssembly.Location);
                dir = Path.Combine(dir, folder);

                var assmbPath = Path.Combine(dir, $"{argName}.dll");

                if (File.Exists(assmbPath))
                {
                    loadAssembly = Assembly.LoadFrom(assmbPath);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to locate dependency: {assmbPath}");
                }
            }

            return loadAssembly;
        }
    }
}