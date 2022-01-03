using Komponent.IO;
using Kontract.Models.Image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace plugin_fontUnpackage.Images
{
    class FontRune
    {
        public FontRune() {
        
        }
        public ImageInfo Load(Stream input)
        {
            using var br = new BinaryReaderX(input);

            // prendo i byte dall'immagine come facevo in python

            // come fare: 
            // so le dimensioni totali dell'immagine
            // prima di tutto leggo tutti i byte dello stream in input
            var data = br.ReadAllBytes();

            // devo prendere questi dati e implementare questo algoritmo:
            
             int pos = 0;
             int to_read = 0;
             byte[] img_data = new byte[25 * 24 * 4];
             for(int y = 0; y < 25; y++)
             {
                 byte current_byte = 0; 
                 for(int x = 0; x < 24; x++)
                 {
                     byte alpha = 0;
                     if ((pos/4) % 2 == 0)
                     {
                         // leggo un nuvo byte
                         current_byte = data[to_read];
                         to_read++;
                     }



                     if(pos % 2 == 0)
                     {
                         alpha = (byte)(((current_byte & 0xF) << 4) | (current_byte & 0xF));
                     }
                     else
                     {
                         alpha = (byte)((current_byte & 0xF0) | (byte)((current_byte & 0xF0) >> 4));
                     }
                   
                    
                        img_data[pos + 0] = alpha; // R
                        img_data[pos + 1] = 255; // G? (sfondo verde)
                        img_data[pos + 2] = 255;
                        img_data[pos + 3] = 255;
                    
                    pos+=4;  
                 }
             }

            return new ImageInfo(img_data, 0, new Size(24, 25));
        }
    }
}
