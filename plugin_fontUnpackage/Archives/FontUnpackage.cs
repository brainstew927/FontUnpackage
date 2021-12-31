using Komponent.IO;
using Kontract.Models.Archive;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Komponent.IO.Streams;
using Kompression.Implementations.Decoders.Headerless;


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
        public int getNextOffset(UInt32[] array, int pos)
        {
            for (int i = pos+1; i < array.Length; i++)
            {
                if (array[i] != array[pos])
                {
                    // l'elemento è diverso, restituisci questo el
                    return i;
                }
            }
            // in caso non si sia trovato alcun elemento valido restituisci -1
            return -1;
        }

        public List<IArchiveFileInfo> Load(Stream input)
        {

            var files = new List<IArchiveFileInfo>();
            // leggo per prima cosa il file count (sono i primi 32 bit)
            // oggetto usato per leggere una certa qta di bit per 
            var br = new BinaryReaderX(input, true);
            byte[] data = br.ReadAllBytes();
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
            UInt32 file_count = FromByteArrayToInt(data[0..4]);
            //files.Add(new ArchiveFileInfo(new MemoryStream(file_count_BYTES), $"file_{file_count}"));
            //files.Add(new ArchiveFileInfo(new MemoryStream(data[0..4]), $"filecount_{file_count}"));

            // abbiamo quanti file sono:
            // ciclo for per avere tutti gli "indirizzi" (in realtà sono offset)
            // creo un nuovo array di uint32 dove andrò a memorizzare tutti gli offset dei file.
            // devo poi ordinare questo array.
            UInt32[] file_table = new UInt32[(int)file_count];
            // carico i dati nella filetable
            for(var i = 1; i < file_count; i++)
            {
                // prendo i dati da data
                // carico in file_table effetuando la conversione con la mia funzione
                file_table[i] = FromByteArrayToInt(data[(i * 4)..((i + 1) * 4)]);
            }
            // ordino la file table
            Array.Sort(file_table);
            // ora ho la file table sortata
            // leggo un elemento per volta dalla file table e carico i dati dei vari file nei file
            for (var i = 0; i < file_count - 1; i++)
            {
                UInt32 file_start = file_table[i];

                int file_end_index = getNextOffset(file_table, i);

                int length = 0;

                if (file_end_index != -1)
                {
                    // leggi file_end - file_start byte.
                    length = (int)(file_table[file_end_index] - file_start);
                }
                else
                {
                    // leggi tutto il file
                    length = data.Length;
                }
                var data_stream = new MemoryStream(data[((int)file_start)..((int)file_start + length)]);
                // aggiungo il file all'elenco
                files.Add(new ArchiveFileInfo(data_stream, i.ToString("D8")+".unbin", Kompression.Implementations.Compressions.ShadeLzHeaderless, ShadeLzHeaderlessDecoder.CalculateDecompressedSize(data_stream)));
            }

            return files;
        }

    }
}
