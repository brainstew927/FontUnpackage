using Komponent.IO;
using Kontract.Models.Archive;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Komponent.IO.Streams;

namespace plugin_fontUnpackage.Archives
{
    class FontUnpackage
    {

        public UInt32 FromByteArrayToInt(byte []array)
        {
            return BitConverter.ToUInt32(array);
        }        
        public byte[] Get4Byte(byte [] array, int start)
        {
            byte[] new_array = new byte[4];
            for (var i = start; i < start+4; i++)
            {
                new_array[i] = array[i];
            }
            return new_array;
        }

        public List<IArchiveFileInfo> Load(Stream input)
        {

            var files = new List<IArchiveFileInfo>();
            // leggo per prima cosa il file count (sono i primi 32 bit)
            // oggetto usato per leggere una certa qta di bit per 
            var br = new BinaryReaderX(input, true);
            byte[] file_count_BYTES = new byte[4];
            br.Read(file_count_BYTES, 0, 4);
            // ottengo il valore contenuto in file_count
            /*
             * 0: 0XFF -> 1111 1111 
             * 1: 0xA3 
             * 
             * ff a3 ...
             * 
             * prima converto ff in intero e lo moltiplico per 10 alla qta di byte totali (4) - 1 - la posizione attuale (0) (10^3)
             *
             */
            UInt32 file_count = FromByteArrayToInt(file_count_BYTES);
            //files.Add(new ArchiveFileInfo(new MemoryStream(file_count_BYTES), $"file_{file_count}"));
            files.Add(new ArchiveFileInfo(new MemoryStream(file_count_BYTES), $"filecount_{file_count}"));
            
            // abbiamo quanti file sono:
            // ciclo for per avere tutti gli "indirizzi" (in realtà sono offset)
            for (var i = 0; i < file_count - 1; i++)
            {
                byte[] tmp = new byte[10];
                // leggo i byte dell'offset attuale
                // creo un nuovo substream 
                var stream = new SubStream(input, 4*(i+1), 4);
                BinaryReaderX reader_ = new BinaryReaderX(stream, false);
                UInt32 off_partenza = reader_.ReadType<UInt32>();

                //files.Add(new ArchiveFileInfo(new MemoryStream(BitConverter.GetBytes(off_partenza)), $"{i}_partenza"));
                // leggo i byte dell'offset "prossimo"
                stream = new SubStream(input, 4 * (i + 2), 4);
                reader_ = new BinaryReaderX(stream, false);
                UInt32 off_fine = reader_.ReadType<UInt32>();
                //files.Add(new ArchiveFileInfo(new MemoryStream(BitConverter.GetBytes(off_fine)), i.ToString("D8")));


                off_fine -= 1;
                var size = off_fine - off_partenza;

                if(off_fine < off_partenza || off_fine <= 1 || size <= 0)
                {
                    files.Add(new ArchiveFileInfo(new MemoryStream(BitConverter.GetBytes(off_fine)), i.ToString("D8") + "_PROBLEMA"));

                    continue;
                }

                // creo il nuovo stream (che contiene SOLO i dati del file)
                stream = new SubStream(input, off_partenza, size);
                // creo un nuovo reader che legge da strewam
                reader_ = new BinaryReaderX(stream, false);
                // leggo i dati del file e li metto in un array di Byte
                byte[] data = new byte[size];
                for (var j = 0; j < off_fine - off_partenza; j++)
                {
                    data[j] = reader_.ReadByte();
                }
                files.Add(new ArchiveFileInfo(new MemoryStream(data), i.ToString("D8")));
            }


            return files;
        }

    }
}
