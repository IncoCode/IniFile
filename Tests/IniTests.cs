#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ini;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace Tests
{
    [TestClass]
    public class IniTests
    {
        private readonly IniFile _ini;
        private readonly Random _random;
        private readonly Dictionary<string, Dictionary<string, string>> _data;

        public IniTests()
        {
            this._ini = new IniFile( "TestIni.ini" );
            this._random = new Random();
            this._data = this.GetData();
            if ( File.Exists( this._ini.Path ) )
            {
                File.WriteAllText( this._ini.Path, String.Empty );
            }
        }

        private string RandomString( int size )
        {
            var builder = new StringBuilder();
            for ( int i = 0; i < size; i++ )
            {
                char ch = Convert.ToChar( Convert.ToInt32( Math.Floor( 26 * this._random.NextDouble() + 65 ) ) );
                builder.Append( ch );
            }

            return builder.ToString();
        }

        private Dictionary<string, Dictionary<string, string>> GetData()
        {
            var data = new Dictionary<string, Dictionary<string, string>>();
            const int sections = 3;
            for ( int i = 0; i < sections; i++ )
            {
                int values = this._random.Next( 3, 20 );
                string section = this.RandomString( this._random.Next( 3, 20 ) );
                data.Add( section, new Dictionary<string, string>() );
                for ( int j = 0; j < values; j++ )
                {
                    string key = this.RandomString( this._random.Next( 3, 20 ) );
                    string value = this.RandomString( this._random.Next( 3, 20 ) );
                    data[ section ][ key ] = value;
                }
            }
            return data;
        }

        [TestMethod]
        public void WriteReadTest()
        {
            foreach ( KeyValuePair<string, Dictionary<string, string>> section in this._data )
            {
                foreach ( KeyValuePair<string, string> value in section.Value )
                {
                    this._ini.Write( value.Key, value.Value, section.Key );
                }
            }
            foreach ( KeyValuePair<string, Dictionary<string, string>> section in this._data )
            {
                foreach ( KeyValuePair<string, string> value in section.Value )
                {
                    string readValue = this._ini.Read( value.Key, section.Key, "" );
                    string valueStr = value.Value;
                    if ( readValue != valueStr )
                    {
                        Assert.Fail();
                    }
                }
            }
        }

        [TestMethod]
        public void GetAllSections()
        {
            this.WriteReadTest();
            foreach ( KeyValuePair<string, Dictionary<string, string>> section in this._data )
            {
                string[] allKeys = this._ini.GetAllKeysInSection( section.Key );
                foreach ( KeyValuePair<string, string> value in section.Value )
                {
                    if ( !allKeys.Contains( value.Key ) )
                    {
                        Assert.Fail();
                    }
                }
            }
        }
    }
}