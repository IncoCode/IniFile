#region Using

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

#endregion

namespace Ini
{
    public class IniFile
    {
        public readonly string Path;

        private static readonly string Exe = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport( "kernel32" )]
        private static extern long WritePrivateProfileString( string section, string key, string value, string filePath );

        [DllImport( "kernel32" )]
        private static extern int GetPrivateProfileString( string section, string key, string Default, StringBuilder retVal, int size, string filePath );

        [DllImport( "kernel32.dll" )]
        private static extern int GetPrivateProfileSection( string lpAppName, byte[] lpszReturnBuffer, int nSize, string lpFileName );

        public IniFile( string iniPath = null )
        {
            this.Path = new FileInfo( iniPath ?? Exe + ".ini" ).FullName;
        }

        public T Read<T>( string key, string section, T defaultValue )
        {
            try
            {
                var retVal = new StringBuilder( 255 );
                GetPrivateProfileString( section ?? Exe, key, "", retVal, 255, this.Path );
                if ( string.IsNullOrEmpty( retVal.ToString() ) )
                {
                    return defaultValue;
                }
                string val = retVal.ToString();
                if ( typeof( T ) == typeof( string ) )
                {
                    return (T)Convert.ChangeType( val, TypeCode.String );
                }
                if ( typeof( T ) == typeof( bool ) )
                {
                    return (T)Convert.ChangeType( val, TypeCode.Boolean );
                }
                if ( typeof( T ) == typeof( int ) )
                {
                    return (T)Convert.ChangeType( val, TypeCode.Int32 );
                }
                if ( typeof( T ) == typeof( double ) )
                {
                    return (T)Convert.ChangeType( val, TypeCode.Double );
                }
            }
            catch
            {
                return defaultValue;
            }
            return defaultValue;
        }

        public string[] GetAllKeysInSection( string section )
        {
            byte[] buffer = new byte[ 2048 ];
            GetPrivateProfileSection( section, buffer, 2048, this.Path );
            String[] tmp = Encoding.ASCII.GetString( buffer ).Trim( '\0' ).Split( '\0' );
            return tmp.Select( entry => entry.Substring( 0, entry.IndexOf( "=", StringComparison.Ordinal ) ) ).ToArray();
        }

        public void Write<T>( string key, T value, string section = null )
        {
            WritePrivateProfileString( section ?? Exe, key, value.ToString(), this.Path );
        }

        public bool KeyExists( string key, string section = null )
        {
            return this.Read( key, section, "" ).Length > 0;
        }
    }
}