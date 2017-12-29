using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace CTDDJYDS.CommonModule
{
    public class SharpZipHelper
    {
        private static readonly string m_zipPassword = "201712";
        public static bool UnzipFileToFolder(string inputFile, string dirName, string pwd, bool useZipName = false)
        {
            try
            {
                using (ZipInputStream inputStream = new ZipInputStream(File.OpenRead(inputFile)))
                {
                    inputStream.Password = pwd;
                    ZipEntry theEntry;
                    while ((theEntry = inputStream.GetNextEntry()) != null)
                    {                      
                        // create directory
                        if (dirName.Length > 0 && !Directory.Exists(dirName))
                        {
                            Directory.CreateDirectory(dirName);
                        }
                        var fileName = theEntry.Name;
                        if (useZipName)
                        {
                            var ext = Path.GetExtension(fileName);
                            var name = Path.GetFileNameWithoutExtension(inputFile);
                            fileName = name + ext;
                        }
                        string outputFile = Path.Combine(dirName, fileName);

                        if (!string.IsNullOrEmpty(outputFile))
                        {
                            using (FileStream streamWriter = File.Create(outputFile))
                            {
                                int size = 4096;
                                byte[] data = new byte[4096];
                                while (true)
                                {
                                    size = inputStream.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                return false;
            }
        }

        public static bool UnzipFile(string inputFile, string outputFile, string pwd)
        {
            try
            {
                using (ZipInputStream inputStream = new ZipInputStream(File.OpenRead(inputFile)))
                {
                    inputStream.Password = pwd;
                    ZipEntry theEntry;
                    while ((theEntry = inputStream.GetNextEntry()) != null)
                    {                       
                        string directoryName = Path.GetDirectoryName(outputFile);
                        string fileName = Path.GetFileName(outputFile);

                        // create directory
                        if (directoryName.Length > 0 && !Directory.Exists(directoryName))
                        {
                            Directory.CreateDirectory(directoryName);
                        }

                        if (fileName != String.Empty)
                        {
                            using (FileStream streamWriter = File.Create(outputFile))
                            {
                                int size = 4096;
                                byte[] data = new byte[4096];
                                while (true)
                                {
                                    size = inputStream.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                return false;
            }
        }

        /// <param name="inputFile">需要压缩的文件</param>
        /// <param name="outputFile">压缩后的文件路径（绝对路径）+压缩后的文件名称（文件名，默认 同源文件同名）</param>
        /// <param name="level">压缩等级（0 无 - 9 最高，默认 5）</param>
        /// <param name="BlockSize">缓存大小（每次写入文件大小，默认 2048）</param>
        /// <param name="IsEncrypt">是否加密（默认 加密）</param>
        /// <param name="isAbsolutePath">是否是绝对路径</param>
        public static bool ZipFile(string inputFile, string outputFile, string pwd, int level,bool isEncrypt=false, bool isAbsolutePath = true)
        {
            try
            {
                using (ZipOutputStream outputStream = new ZipOutputStream(File.Create(outputFile)))
                {
                    outputStream.SetLevel(level);
                    if (isEncrypt)
                        outputStream.Password = pwd;
                    string newInputFilePath = inputFile;
                    if (!isAbsolutePath)
                    {
                        newInputFilePath = Path.GetFileName(inputFile);
                    }                  
                    ZipEntry entry = new ZipEntry(newInputFilePath);
                    entry.DateTime = DateTime.Now;
                    outputStream.PutNextEntry(entry);
                    using (FileStream fs = File.OpenRead(inputFile))
                    {
                        int sourceBytes;
                        byte[] buffer = new byte[4096];
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            outputStream.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                    outputStream.Finish();
                    System.Diagnostics.Debug.Assert(outputStream.IsFinished);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                return false;
            }
        }

        public static bool ZipMultiFiles(string[] inputFiles, string outputFile, string pwd, int level,
            BackgroundWorker worker = null, bool isAbsolutePath = true,string zipComment="")
        {
            ZipOutputStream zipStream = null;
            FileStream streamReader = null;
            try
            {
                //Create Zip File
                using (zipStream = new ZipOutputStream(File.Create(outputFile)))
                {
                    //Specify Level
                    zipStream.SetLevel(Convert.ToInt32(level));
                    //Specify Password
                    if (pwd != null && pwd.Trim().Length > 0)
                    {
                        zipStream.Password = pwd;
                    }
                    //Save the current application version in the comment
                    if (!string.IsNullOrEmpty(zipComment))
                    {
                        zipStream.SetComment(zipComment);
                    }

                    byte[] buffer = new byte[4096 * 1024];
                    int sourceCount = 0;
                    //for each File
                    foreach (string file in inputFiles)
                    {
                        //Check Whether the file exists
                        if (!File.Exists(file))
                        {
                            throw new FileNotFoundException(file);
                        }

                        //Read the file to stream
                        using (streamReader = File.OpenRead(file))
                        {
                            //Specify ZipEntry
                            string newInputFilePath = file;
                            if (!isAbsolutePath)
                            {
                                newInputFilePath = Path.GetFileName(file);
                            }
                            ZipEntry zipEntry = new ZipEntry(newInputFilePath);
                            zipEntry.DateTime = DateTime.Now;
                            zipEntry.Size = streamReader.Length;
                            // Using the Unicode Text to make sure the name of the entry will be
                            // recognized. e.g., if the file path contains Chinese characters and
                            // it's packed in a pure English system, this will make sure the file
                            // path will be saved correctly.
                            zipEntry.IsUnicodeText = true;

                            //Put file info into zip stream
                            zipStream.PutNextEntry(zipEntry);

                            while ((sourceCount = streamReader.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                //Put file data into zip stream
                                zipStream.Write(buffer, 0, sourceCount);
                                if (worker != null && worker.IsBusy)
                                {
                                    if (worker.WorkerReportsProgress)
                                    {
                                        worker.ReportProgress((int)(streamReader.Position * 100 / streamReader.Length));
                                    }
                                    if (worker.CancellationPending)
                                    {
                                        // update the entry size to avoid the ZipStream exception
                                        zipEntry.Size = streamReader.Position;
                                        break;
                                    }
                                }
                            }
                        }

                        if (worker != null && worker.CancellationPending)
                        {
                            break;
                        }
                    }
                    zipStream.Finish();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }
                if (zipStream != null)
                {
                    zipStream.Finish();
                    zipStream.Close();
                }
            }
            return true;
        }

        public static bool UnzipMultiFiles(string inputFile, string outputFolder, string pwd, BackgroundWorker worker = null)
        {
            FileStream streamWriter = null;
            ZipInputStream zipStream = null;

            try
            {
                if (inputFile == string.Empty || outputFolder == string.Empty || !File.Exists(inputFile))
                    return false;

                if (!Directory.Exists(outputFolder))
                    Directory.CreateDirectory(outputFolder);

                ZipEntry entry = null;
                using (zipStream = new ZipInputStream(File.OpenRead(inputFile)))
                {
                    zipStream.Password = pwd;
                    byte[] buffer = new byte[4096 * 1024];
                    int sourceCount = 0;

                    while ((entry = zipStream.GetNextEntry()) != null)
                    {
                        string filename = Path.Combine(outputFolder, Path.GetFileName(entry.Name));
                        if (File.Exists(filename))
                            File.Delete(filename);

                        using (streamWriter = File.Create(filename))
                        {
                            while ((sourceCount = zipStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                streamWriter.Write(buffer, 0, sourceCount);
                                if (worker != null && worker.WorkerReportsProgress)
                                {
                                    worker.ReportProgress((int)(streamWriter.Position * 100 / entry.Size));
                                }
                            }
                        }
                        File.SetLastWriteTime(filename, entry.DateTime);
                    }
                }
                return true;
            }
            catch (System.Exception e)
            {
                LogHelper.Log(e);
            }
            finally
            {
                if (streamWriter != null)
                    streamWriter.Close();
                if (zipStream != null)
                    zipStream.Close();
            }
            return false;
        }

        public static bool CompressBuffer(byte[] inbuffer, out byte[] outBuffer)
        {
            if (inbuffer == null || inbuffer.Length == 0)
            {
                outBuffer = null;
                return true;
            }

            try
            {
                using (MemoryStream outMs = new MemoryStream())
                using (GZipStream gs = new GZipStream(outMs, CompressionMode.Compress))
                {
                    gs.Write(inbuffer, 0, inbuffer.Length);
                    //Make sure close GZip stream before get buffer
                    gs.Close();
                    outBuffer = outMs.GetBuffer();
                }
                return true;
            }
            catch (System.Exception e)
            {
                LogHelper.Log(e);
                outBuffer = null;
                return false;
            }
        }

        public static bool DecompressBuffer(byte[] inbuffer, out byte[] outBuffer)
        {
            if (inbuffer == null || inbuffer.Length == 0)
            {
                outBuffer = null;
                return false;
            }

            try
            {
                using (MemoryStream inMs = new MemoryStream(inbuffer))
                using (GZipStream gs = new GZipStream(inMs, CompressionMode.Decompress, true))
                using (MemoryStream outMs = new MemoryStream())
                {
                    const int bufferLength = 4096;
                    byte[] buffer = new byte[bufferLength];
                    int read = gs.Read(buffer, 0, bufferLength);
                    while (read > 0)
                    {
                        outMs.Write(buffer, 0, read);
                        read = gs.Read(buffer, 0, bufferLength);
                    }
                    outBuffer = outMs.GetBuffer();
                }
                return true;
            }
            catch (System.Exception e)
            {
                LogHelper.Log(e);
                outBuffer = null;
                return false;
            }
        }
    }
}
