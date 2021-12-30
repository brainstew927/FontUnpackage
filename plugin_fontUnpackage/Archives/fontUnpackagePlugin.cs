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
using System.Threading.Tasks;
using Kontract.Interfaces.FileSystem;
using Kontract.Models.IO;
using Kontract.Models.Context;
using Komponent.IO;

namespace plugin_fontUnpackage.Archives
{
    public class FontUnpackagePlugin : IFilePlugin, IIdentifyFiles
    { 
        public Guid PluginId => Guid.Parse("5580AE6F-1FC7-474D-ACAF-EA3A76ED7050");
        public PluginType PluginType => PluginType.Archive;
        public string[] FileExtensions => new[] { "*.bin" };
        public PluginMetadata Metadata { get; }

        public async Task<bool> IdentifyAsync(IFileSystem fileSystem, UPath filePath, IdentifyContext identifyContext)
        {
            var fileStream = await fileSystem.OpenFileAsync(filePath);

            using var br = new BinaryReaderX(fileStream);
            return br.ReadString(3) == "BIN";
        }

        public FontUnpackagePlugin()
        {
            Metadata = new PluginMetadata("bin", "brainstew927", "Unpackage the fonts archive in Inazuma Eleven Go Strikers 2013.");
        }

        public IPluginState CreatePluginState(IBaseFileManager fileManager)
        {
            return new FontUnpackageState();
        }
    }
}
