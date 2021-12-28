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
        // TODO: generare la mia GUID
        public Guid PluginId => Guid.Parse("39ef72b0-8a08-4edb-a83e-3543020c86dd");
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
