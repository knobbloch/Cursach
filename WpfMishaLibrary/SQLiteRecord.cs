using System;

namespace WpfMishaLibrary
{
    ///<summary>
    /// Gets actual consumption values and forms the query
    /// to the local sqlite database
    ///</summary>
    abstract class SQLiteRecord
    {
        public SQLiteRecord()
        {

        }
        abstract public int AddRecord();
    }
}
