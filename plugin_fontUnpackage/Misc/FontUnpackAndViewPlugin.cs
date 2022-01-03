using Kontract.Interfaces.Managers;
using Kontract.Interfaces.Plugins.Identifier;
using Kontract.Interfaces.Plugins.State;
using Kontract.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace plugin_fontUnpackage.Misc
{
    public class FontUnpackAndViewPlugin : IFilePlugin
    { 
        public Guid PluginId => Guid.Parse("0FE4961F-E839-437C-B761-19A563B2CA8E");
        public PluginType PluginType => PluginType.Image;
        public string[] FileExtensions => new[] { "*.bin" };
        public PluginMetadata Metadata { get; }

        public FontUnpackAndViewPlugin()
        {
            Metadata = new PluginMetadata("bin", "brainstew927", "Unpackage and view the fonts archive in Inazuma Eleven Go Strikers 2013.");
        }

        public IPluginState CreatePluginState(IBaseFileManager fileManager)
        {
            return new FontUnpackAndViewState();
        }
    }
}
