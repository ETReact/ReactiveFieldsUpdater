﻿using System;
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
        ExportMetadata("SmallImageBase64", "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"),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "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"),
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