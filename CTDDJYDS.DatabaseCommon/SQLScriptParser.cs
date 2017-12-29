using System;
using System.IO;
using System.Reflection;

namespace CTDDJYDS.Database.Common
{
    class SQLScriptParser : IDisposable
    {
        private string path;
        private StreamReader rStream;

        public SQLScriptParser()
        {
        }

        public SQLScriptParser ( string _path )
            : this ()
        {
            LoadScript ( _path );
        }

        public void LoadScript ( string _path )
        {
            path = _path;
            FileStream fs = new FileStream ( path, FileMode.Open, FileAccess.Read, FileShare.Read );
            rStream = new StreamReader(fs );
        }

        public string LoadScriptContent(string _path)
        {
            Assembly assembly = this.GetType().Assembly;

            Stream stream = assembly.GetManifestResourceStream(_path);
            if (stream != null)
            {
                using(rStream = new StreamReader(stream))
                {
                    string str = rStream.ReadToEnd();
               
                    return str;
                }
            }
            else
                return string.Empty;
        }

        public string NextCommand()
        {
            long lastPos = rStream.BaseStream.Position;
            string cmdText = string.Empty;
            string ln = string.Empty;
            int posSemicolon = -1;
            int len = 0;
            do
            {
                ln = rStream.ReadLine();
                if (ln != null && !ln.Trim().StartsWith("--") )
                {
                    ln = ln.Trim();
                    len = ln.Length;
                    posSemicolon = ln.LastIndexOf( ';' );
                    if ( posSemicolon == -1 || posSemicolon == len-1 )
                    {
                        int idxInnerDash = ln.IndexOf("--");
                        if ( idxInnerDash != -1 )
                        {
                            ln = ln.Substring(0, idxInnerDash).Trim();
                        }
                        cmdText += ln;
                    }
                    else
                        break;
                }
            }
            while(ln != null && !ln.EndsWith(";"));
            if ( posSemicolon != -1 && posSemicolon != len-1 )
            {
                rStream.BaseStream.Seek ( lastPos, SeekOrigin.Begin );
                cmdText = string.Empty;
            }
            if ( ln == null )
            {
                rStream.Close();
            }
            return cmdText;
        }
        
        public void LoadScriptEx(string path)
        {
            if (rStream != null)
            {
                rStream.Close();
            }
            Assembly assembly = this.GetType().Assembly;

            Stream stream = assembly.GetManifestResourceStream(path);
            if (stream != null)
                rStream = new StreamReader(stream);
            else
                rStream = null;
        }

        public int CurProgress
        {
            get
            {
                if (rStream != null)
                {
                    return (int)(rStream.BaseStream.Position * 100 / rStream.BaseStream.Length);
                }
                return 0;
            }
        }

        public void Dispose()
        {
            if (rStream != null)
            {
                rStream.Dispose();
            }
        }

    }
}
