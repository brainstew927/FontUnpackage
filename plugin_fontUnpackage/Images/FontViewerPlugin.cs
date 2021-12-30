using Komponent.IO;
using Kontract.Interfaces.FileSystem;
using Kontract.Interfaces.Managers;
using Kontract.Interfaces.Plugins.Identifier;
using Kontract.Interfaces.Plugins.State;
using Kontract.Models;
using Kontract.Models.Context;
using Kontract.Models.IO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace plugin_fontUnpackage.Images
{
    public class FontViewerPlugin : 
        IFilePlugin, IIdentifyFiles
    {
        public Guid PluginId => Guid.Parse("F4CCAB55-1502-4E53-86ED-8A667B7E0A8B");
        public PluginType PluginType => PluginType.Image;
        public string[] FileExtensions => new[] { "*.unbin" };
        public PluginMetadata Metadata { get; }

        public FontViewerPlugin()
        {
            Metadata = new PluginMetadata("UNBIn", "brainstew927", "View Inazuma Eleven Go Strikers 2013 font unpackaged with the FontUnpackage plugin.");
        }

        public async Task<bool> IdentifyAsync(IFileSystem fileSystem, UPath filePath, IdentifyContext identifyContext)
        {
            var fileStream = await fileSystem.OpenFileAsync(filePath);

            using var br = new BinaryReaderX(fileStream);
            return br.ReadString(3) == "unbin";
        }

        public IPluginState CreatePluginState(IBaseFileManager fileManager)
        {
            return new FontViewerState();
        }
    }
}
