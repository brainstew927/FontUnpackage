using Kanvas;
using Kanvas.Encoding;
using Komponent.IO;
using Kontract.Interfaces.FileSystem;
using Kontract.Interfaces.Plugins.State;
using Kontract.Kanvas;
using Kontract.Models.Context;
using Kontract.Models.Image;
using Kontract.Models.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace plugin_fontUnpackage.Images
{
    class FontViewerState :  IImageState, ILoadFiles
    {
        public IList<IKanvasImage> Images { get; private set; }
        private FontRune img;

        public EncodingDefinition EncodingDefinition { get; }

        public FontViewerState()
        {
            img = new FontRune();
            EncodingDefinition = new EncodingDefinition();
            EncodingDefinition.AddColorEncoding(0, new Rgba(8, 8, 8, 8));
        }

        public async Task Load(IFileSystem fileSystem, UPath filePath, LoadContext loadContext)
        {
            var fileStream = await fileSystem.OpenFileAsync(filePath);
            var img_info = img.Load(fileStream);
            
            Images = img_info.Select(info => new KanvasImage(EncodingDefinition, info)).ToArray(); ;
        }
    }
}
