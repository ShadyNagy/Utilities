using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;

namespace ShadyNagy.Utilities.Compress.SharpZip
{
    public class SharpZip
    {
        private List<MemoryStream> _listMemStreamIn;
        private List<string> _listZipEntryName;
        private readonly int _level;
        private string _password;
        private readonly int? _aesKeySize;

        public SharpZip(string password, int level = 3, int aesSize=256)
        {
            _password = password;
            _level = level;

            _listMemStreamIn = new List<MemoryStream>();
            _listZipEntryName = new List<string>();

            if (!string.IsNullOrEmpty(_password))
            {
                _aesKeySize = aesSize;
            }
        }

        public SharpZip AddFile(string zipEntryName, MemoryStream memStreamIn)
        {
            _listMemStreamIn.Add(memStreamIn);
            _listZipEntryName.Add(zipEntryName);

            return this;
        }

        public SharpZip AddFile(string zipEntryName, string stringData)
        {

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(stringData);
            writer.Flush();
            stream.Position = 0;
            _listMemStreamIn.Add(stream);
            _listZipEntryName.Add(zipEntryName);

            return this;
        }

        public SharpZip ChangePassword(string password)
        {
            _password = password;
            return this;
        }

        public string GetPassword()
        {
            return _password;
        }

        public SharpZip ClearFiles()
        {
            _listMemStreamIn = new List<MemoryStream>();
            _listZipEntryName = new List<string>();

            return this;
        }

        public byte[] BuildBytesFromFiles()
        {
            if(_listZipEntryName == null ||
                _listZipEntryName.Count <= 0 ||
                _listMemStreamIn == null || 
                _listMemStreamIn.Count <= 0)
            {
                return null;
            }

            using (var outputMemStream = new MemoryStream())
            {
                var zipStream = new ZipOutputStream(outputMemStream);

                zipStream.SetLevel(_level); //0-9, 9 being the highest level of compression

                if(!string.IsNullOrEmpty(_password))
                {
                    zipStream.Password = _password;
                }                

                var index = 0;
                foreach(var zipEntryName in _listZipEntryName)
                {
                    var newEntry = new ZipEntry(zipEntryName) {DateTime = DateTime.Now};
                    if (_aesKeySize != null)
                    {
                        newEntry.AESKeySize = (int)_aesKeySize;
                    }
                    
                    zipStream.PutNextEntry(newEntry);

                    StreamUtils.Copy(_listMemStreamIn[index++], zipStream, new byte[4096]);
                }                

                zipStream.CloseEntry();

                zipStream.IsStreamOwner = false;
                zipStream.Close(); 

                outputMemStream.Position = 0;

                return outputMemStream.ToArray();
            }
                
        }
    }
}
