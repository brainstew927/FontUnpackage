using Kanvas;
using Kanvas.Encoding;
using Kontract.Interfaces.FileSystem;
using Kontract.Interfaces.Plugins.State;
using Kontract.Kanvas;
using Kontract.Models.Archive;
using Kontract.Models.Context;
using Kontract.Models.Image;
using Kontract.Models.IO;
using plugin_fontUnpackage.Archives;
using plugin_fontUnpackage.Images;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kompression.Implementations.Decoders.Headerless;
using Kompression.Configuration;
using Kompression.Implementations;


/*
 * Author: brainstew927 (https://github.com/brainstew927)
 * Use: Kuriimu2 2 blind plugin used to decompress and view the rune from a Inazuma
 *      Eleven Go Strikers 2013 font archive.
 * Date: 31/12/2021
 */
namespace plugin_fontUnpackage.Misc
{
    class FontUnpackAndViewState : IImageState, ILoadFiles
    {
        private FontUnpackage _Fu;


        public IList<IKanvasImage> Images { get; private set; }
        private FontRune img;

        public EncodingDefinition EncodingDefinition { get; }

        public FontUnpackAndViewState()
        {
            Images = new List<IKanvasImage> { };
            img = new FontRune();
            EncodingDefinition = new EncodingDefinition();
            EncodingDefinition.AddColorEncoding(0, new Rgba(8, 8, 8, 8));

            _Fu = new FontUnpackage();
        }


        public async Task Load(IFileSystem fileSystem, UPath filePath, LoadContext context)
        {
            context.ProgressContext.IsRunning();

            var input = await fileSystem.OpenFileAsync(filePath);
            context.ProgressContext.ReportProgress("current: loading files... archive", 0, 1000);

            // ora ho in input lo stream, cerco di leggerlo
            var files = _Fu.Load(input);

            context.ProgressContext.SetMaxValue(files.Count);
            context.ProgressContext.ReportProgress("current: Files Loaded", 0, files.Count);


            
            // carico le immagini (lo faccio in un altro thread almeno l'interfaccia non si blocca TODO)
            int blocchi = 2;
            // numero da sommare al files.Count per permettergli di essere multiplo di blocchi
            int c = 0;
            // faccio in modo che la file count sia un multiplo di blocchi
            // QUESTO FUNZIONA SOLO SE è PARI IL NUM DI BLOCCHI
            if (files.Count %2 != 0){
                c = 1;
            }
            for (int i = 0; i < files.Count; i++)
            {
                    var file_data = await files[i].GetFileData();
                    var img_info = img.Load(file_data);
                    // aggiunge l'immagine alla lista delle immagini
                    Images.Add(new KanvasImage(EncodingDefinition, img_info));
                    if(i > 3000)
                    {
                        break;
                    }
                context.ProgressContext.ReportProgress($"caricamento: {i + 1}/{blocchi}", i, blocchi);
            }

            context.ProgressContext.ReportProgress("finished loading", 100, 100);
        }

        //// load the images from / to 
        //private async void LoadArray(List<IArchiveFileInfo> files, ref KanvasImage[] array_to, int from, int to)
        //{
            
        //    for (int i = from; i < to; i++)
        //    {
        //        var file_data = await files[i].GetFileData();
        //        var img_info = img.Load(file_data);
        //        // aggiunge l'immagine alla lista delle immagini
        //        array_to[i] =  new KanvasImage(EncodingDefinition, img_info));
        //    }
        //}
     
    }
}
