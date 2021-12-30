using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;
using System.Text;
using Kontract.Interfaces.Managers;
using Kontract.Interfaces.Plugins.Identifier;
using Kontract.Interfaces.Plugins.State;
using Kontract.Models;

namespace plugin_fontUnpackage.Archives
{
    public class FontUnpackagePlugin : IFilePlugin
    { 

        public Guid PluginId => Guid.Parse("5580AE6F-1FC7-474D-ACAF-EA3A76ED7050");
        public PluginType PluginType => PluginType.Archive;
        public string[] FileExtensions => new[] { "*.bin" };
        public PluginMetadata Metadata { get; }

        public FontUnpackagePlugin()
        {
            Metadata = new PluginMetadata("fontUnpackage", "brainstew927", "Unpackage the fonts archive in Inazuma Eleven go striker 2013.");
        }

        public IPluginState CreatePluginState(IBaseFileManager fileManager)
        {
            return new FontUnpackageState();
        }
    }
}
