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

        public int NextOffset(UInt32[] filetable,int pos)
        {
            for (var i = pos + 1; i < filetable.Length; i++)
            {
                if (filetable[i] != filetable[pos])
                    return i;
            }
            return -1; //il tuo DEFAULT_VALUE
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
            //filetable con tutti gli indirizzi, 4 byte per indirizzo - 4 byte di file_count
            UInt32[] file_table = new UInt32[file_count]; //array di array di 4 byte
            //files.Add(new ArchiveFileInfo(new MemoryStream(file_count_BYTES), $"file_{file_count}"));
            //files.Add(new ArchiveFileInfo(new MemoryStream(file_count_BYTES), $"filecount_{file_count}"));
            
            // abbiamo quanti file sono:
            // ciclo for per avere tutti gli "indirizzi" (in realtà sono offset)
            //NUOVO CICLO FOR
            for (var i = 0; i < file_count; i++)
            {
                //creo substream per leggere gli indirizzi
                var sStream = new SubStream(input, 4 * (i+1), 4);
                BinaryReaderX reader_ = new BinaryReaderX(sStream, false);
                file_table[i] = reader_.ReadType<UInt32>();
                //file_table[i] = BitConverter.GetBytes(off_partenza);
                //files.Add(new ArchiveFileInfo(new MemoryStream(BitConverter.GetBytes(file_table[i])), file_table[i].ToString("D8") + " " + i.ToString("D8")));
            }

            //ordino array
            Array.Sort(file_table);

            //creo e aggiungo i file
            for (var i = 0; i < file_count; i++)
            {
                //leggo offset di partenza
                int file_start = Convert.ToInt32(file_table[i]);
                //leggo offset di arrivo
                int file_end = NextOffset(file_table, i);

                if (file_end == -1)
                {
                    file_end = Convert.ToInt32(input.Length);
                } else
                {
                    file_end = Convert.ToInt32(file_table[file_end]);
                }

                //quanti byte devo leggere?
                int lenght = file_end - file_start;
                var stream = new SubStream(input, file_start, lenght);
                BinaryReaderX reader_ = new BinaryReaderX(stream, false);
                byte[] data = reader_.ReadAllBytes();
                files.Add(new ArchiveFileInfo(new MemoryStream(data), i.ToString("D8") + ".bin"));

            }
            
            
            //VECCHIO CICLO FOR
            /*for (var i = 0; i < file_count - 1; i++)
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
            }*/


            return files;
        }

    }
}
