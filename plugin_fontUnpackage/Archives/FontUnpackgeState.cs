using Kontract.Interfaces.FileSystem;
using Kontract.Interfaces.Plugins.State;
using Kontract.Models.Archive;
using Kontract.Models.Context;
using Kontract.Models.IO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace plugin_fontUnpackage.Archives
{

    class FontUnpackageState : IArchiveState, ILoadFiles
    {
        private FontUnpackage _Fu;
        public FontUnpackageState()
        {
            _Fu = new FontUnpackage();
        }
        // Exposes the loaded files from the archive format to any consuming user interface
        public IList<IArchiveFileInfo> Files { get; private set; }

        public async Task Load(IFileSystem fileSystem, UPath filePath, LoadContext context)
        {
            var input = await fileSystem.OpenFileAsync(filePath);

            // ... Load archive files from input into 'Files' property
            // Example: Files = new List<IArchiveFileInfo>(); /* for an empty list of files
            // ora ho in input lo stream, cerco di leggerlo
            Files = _Fu.Load(input);
        }

    }
}
