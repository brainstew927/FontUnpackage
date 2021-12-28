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
        public Guid PluginId => Guid.Parse("bab218f4-550f-40ee-9219-d83b11265883");
        public PluginType PluginType => PluginType.Archive;
        public string[] FileExtensions => new[] { "*.bin" };
        public PluginMetadata Metadata { get; }

        public FontUnpackagePlugin()
        {
            Metadata = new PluginMetadata("fontUnpackage", "brainstew927", "The main texture resource archive in Streets Of Rage 4.");
        }

        public IPluginState CreatePluginState(IBaseFileManager fileManager)
        {
            return new FontUnpackageState();
        }
    }
}
