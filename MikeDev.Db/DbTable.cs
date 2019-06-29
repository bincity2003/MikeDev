using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MikeDev.DB
{
    /// <summary>
    /// DbKeeper class is used for database management.
    /// </summary>
    public class DbTable : ICloneable, IEnumerable<string[]>, IDisposable, ICollection
    {
        #region Internal holder

        /// <summary>
        /// Store entries' name and corresponding index.
        /// </summary>
        private Dictionary<string, string> _EntryNames;

        /// <summary>
        /// Store entries' data. Access by using corresponding index.
        /// </summary>
        private Dictionary<string, string[]> _DataTable;

        #endregion

        #region Read-only exposure

        /// <summary>
        /// Get an array containing entries' name.
        /// </summary>
        public string[] GetEntriesNames => _EntryNames.Keys.ToArray();

        /// <summary>
        /// Get an array containing fields.
        /// </summary>
        public string[] FieldNames { get; private set; }

        /// <summary>
        /// Get number of fields.
        /// </summary>
        public int Count => FieldNames.Length;

        /// <summary>
        /// Retrieve specified entry.
        /// </summary>
        /// <param name="name">The name of the entry.</param>
        /// <returns>A String array containing values, first element is the name.</returns>
        public string[] this[string name]
        {
            get
            {
                try
                {
                    string Index = _EntryNames[name];
                    string[] Result = new string[Count + 1];
                    Result[0] = name;
                    Array.Copy(_DataTable[Index], 0, Result, 1, Count);
                    return Result;
                }
                catch (KeyNotFoundException)
                {
                    throw new DbTableException("Entry not found!");
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize a new instance of DbKeeper with specified fields.
        /// </summary>
        /// <param name="fieldNames">A string array containing fields' name</param>
        public DbTable(string[] fieldNames)
        {
            if (fieldNames.Length == 0) // Base assertion
            {
                throw new ArgumentException("Rows or columns' name must not be empty!");
            }
            if (fieldNames[0] != "Name")
            {
                throw new DbTableException("First element must be 'Name'!");
            }

            _InternalFieldNameValidator(fieldNames);
            FieldNames = fieldNames;

            _DataTable = new Dictionary<string, string[]>();
            _EntryNames = new Dictionary<string, string>();
        }

        /// <summary>
        /// Read existing DbKeeper instance from file.
        /// </summary>
        /// <param name="fileName">Path to DbKeeper file</param>
        public DbTable(string fileName)
        {
            // Retrieve data
            string Data = File.ReadAllText(fileName);

            // Load instance from data
            try
            {
                _InternalLoadInstance(Data);
            }
            catch // If data is faulty, then throw
            {
                throw new Exception("Unable to deserialize JSON data! Data is broken");
            }
        }

        /// <summary>
        /// Private constructor for internal uses. Not recommended.
        /// </summary>
        private DbTable() { }

        #endregion

        #region Public APIs

        /// <summary>
        /// Add a new entry.
        /// </summary>
        /// <param name="name">The name of the entry</param>
        /// <param name="values">Other fields that match the fields requirements.</param>
        public void AddEntry(string name, params string[] values)
        {
            if (values.Length != Count) // Argument assertion
            {
                throw new DbTableException("Number of values provided doesn't match number of field");
            }
            foreach (var index in _EntryNames.Keys)
            {
                if (name == index) // Scan for name availability
                {
                    throw new DbTableException("Name is already taken!");
                }
            }

            _InternalAddEntry(name, values);
        }

        /// <summary>
        /// Add multiple new entries.
        /// </summary>
        /// <param name="name">An array containing name for these entries.</param>
        /// <param name="values">Other fields that match the fields requirements.</param>
        public void AddEntry(string[] name, params string[][] values)
        {
            if (name.Length != values.Length)
            {
                throw new DbTableException($"Length not match: {name.Length} versus {values.Length}");
            }
            for (int i = 0; i < name.Length; i++)
            {
                AddEntry(name[i], values[i]);
            }
        }

        /// <summary>
        /// Remove an existing entry.
        /// </summary>
        /// <param name="name">The name of the entry to be removed.</param>
        public void RemoveEntry(string name)
        {
            _InternalRemoveEntry(name);
        }

        /// <summary>
        /// Remove multiple existing entries.
        /// </summary>
        /// <param name="name">The name of the entries to be removed.</param>
        public void RemoveEntry(string[] name)
        {
            foreach (var item in name)
            {
                RemoveEntry(item);
            }
        }

        /// <summary>
        /// Replace an existing entry.
        /// </summary>
        /// <param name="name">The name of the entry to be replaced.</param>
        /// <param name="values">The new set of fields' value.</param>
        public void ReplaceEntry(string name, params string[] values)
        {
            _InternalReplaceEntry(name, values, out string Index);
            _DataTable[Index] = values;
        }

        /// <summary>
        /// Replace multiple existing entry.
        /// </summary>
        /// <param name="name">The name of the entries to be replaced.</param>
        /// <param name="values">The new set of fields' value.</param>
        public void ReplaceEntry(string[] name, params string[][] values)
        {
            if (name.Length != values.Length)
            {
                throw new DbTableException($"Length not match: {name.Length} versus {values.Length}");
            }
            for (int i = 0; i < name.Length; i++)
            {
                ReplaceEntry(name[i], values[i]);
            }
        }

        /// <summary>
        /// Execute advanced command, including filter.
        /// </summary>
        /// <param name="command">Command to be executed.</param>
        /// <returns>An array of String array (fields).</returns>
        public static string[][] Execute(string command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            return _InternalExecutionEngine(command);
        }

        /// <summary>
        /// API helper to calculate index.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetIndex(string s)
        {
            if (s is null)
            {
                throw new ArgumentNullException(nameof(s));
            }
            return _InternalGetIndex(s);
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Internal method for contructor's LoadInstance.
        /// </summary>
        private void _InternalLoadInstance(string jsonData)
        {
            // Deserialize object
            DbTable holder = JsonConvert.DeserializeObject<DbTable>(jsonData);

            // Copying data (need to be added more in the future)
            _EntryNames = holder._EntryNames;
            _InternalFieldNameValidator(holder.FieldNames);
            FieldNames = holder.FieldNames;
            _DataTable = holder._DataTable;
        }

        /// <summary>
        /// Internal method for AddEntry.
        /// </summary>
        private void _InternalAddEntry(string name, string[] values)
        {
            // Calculate unique index
            string Index = _InternalGetIndex(name);
            // Add to index
            _EntryNames.Add(name, Index);

            // Add entry to table
            _DataTable.Add(Index, values);
        }

        /// <summary>
        /// Internal method for RemoveEntry.
        /// </summary>
        private void _InternalRemoveEntry(string name)
        {
            // Self-explanatory
            try
            {
                string Index = _EntryNames[name];
                _EntryNames.Remove(name);
                _DataTable.Remove(Index);
            }
            catch (KeyNotFoundException)
            {
                throw new DbTableException("Entry not found!");
            }
        }

        /// <summary>
        /// Internal method for ReplaceEntry.
        /// </summary>
        private void _InternalReplaceEntry(string name, string[] values, out string Index)
        {
            // Self-explanatory
            try
            {
                Index = _EntryNames[name];
            }
            catch (KeyNotFoundException)
            {
                throw new DbTableException("Entry not found!");
            }
            if (values.Length != Count)
            {
                throw new DbTableException("Number of values provided doesn't match number of field");
            }
        }

        /// <summary>
        /// Internal method for Execute.
        /// </summary>
        private static string[][] _InternalExecutionEngine(string command)
        {
            // Prepare
            string[][] result;
            string[] Partition = command.Split(' ');

            // Determine command
            switch (Partition[0])                                       // Command identifier
            {
                case var p when new Regex(@"[Ss][Ee][Ll][Ee][Cc][Tt]").IsMatch(p):
                    result = _InternalSelectCommand(Partition);
                    break;
                case var p when new Regex(@"[Dd][Ee][Ll][Ee][Tt][Ee]").IsMatch(p):
                    result = _InternalDeleteCommand(Partition);
                    break;
                default:                                                // Default (error)
                    throw new DbTableException("Unknown command!");
            }

            return result;
        }

        /// <summary>
        /// Internal method for _InternalExecutionEngine (DELETE branch).
        /// </summary>
        private static string[][] _InternalDeleteCommand(string[] parts)
        {
            // Token list
            List<string> Tokens = new List<string>();

            // Scan for tokens in partition
            foreach (var part in parts)
            {
                switch (part)
                {
                    case var p when new Regex(@"[Dd][Ee][Ll][Ee][Tt][Ee]").IsMatch(p):
                        Tokens.Add("command_delete");
                        break;
                    case "*":
                        Tokens.Add("wildcard");
                        break;
                    case var p when new Regex(@"[Ww][Hh][Ee][Rr][Ee]").IsMatch(p):
                        Tokens.Add("constraints");
                        break;
                    case var p when new Regex(@"(<[=>]?|==|>=?|\&\&|\|\|)").IsMatch(p):
                        Tokens.Add($"comparison_{part}");
                        break;
                    case "!=":
                        Tokens.Add($"comparison_{part}");
                        break;
                    case "(":
                        Tokens.Add("character_leftparen");
                        break;
                    case ")":
                        Tokens.Add("character_rightparen");
                        break;
                    case var p when new Regex(@"\$[a-zA-Z][0-9a-zA-Z]{0,30}").IsMatch(p):
                        Tokens.Add($"fieldname_{part}");
                        break;
                    default:
                        Tokens.Add($"word_{part}");
                        break;
                }
            }

            // Parse tokens (to be implemented)
            foreach (var token in Tokens)
            {

            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Internal method for _InternalExecutionEngine (SELECT branch).
        /// </summary>
        private static string[][] _InternalSelectCommand(string[] parts)
        {
            // Token list
            List<string> Tokens = new List<string>();

            // Scan for token in partition
            foreach (var part in parts)
            {
                switch (part)
                {
                    case var p when new Regex(@"[Ss][Ee][Ll][Ee][Cc][Tt]").IsMatch(p):
                        Tokens.Add("commmand_select");
                        break;
                    case "*":
                        Tokens.Add("character_wildcard");
                        break;
                    case var p when new Regex(@"[Ww][Hh][Ee][Rr][Ee]").IsMatch(p):
                        Tokens.Add("constraints");
                        break;
                    case var p when new Regex(@"(<[=>]?|==|>=?|\&\&|\|\|)").IsMatch(p):
                        Tokens.Add($"comparison_{p}");
                        break;
                    case "!=":
                        Tokens.Add($"comparison_{part}");
                        break;
                    case "(":
                        Tokens.Add("character_leftparen");
                        break;
                    case ")":
                        Tokens.Add("character_rightparen");
                        break;
                    case var p when new Regex(@"\$[a-zA-Z][0-9a-zA-Z]{0,30}").IsMatch(p):
                        Tokens.Add($"fieldname_{part}");
                        break;
                    default:
                        Tokens.Add($"word_{part}");
                        break;
                }
            }

            // Parse token (to be implemented)
            throw new NotImplementedException();
        }

        /// <summary>
        /// Internal method for _FieldName validator.
        /// </summary>
        private static void _InternalFieldNameValidator(string[] fieldNames)
        {
            foreach (var item in fieldNames)
            {
                if (!new Regex("[a-zA-Z][0-9a-zA-Z]{0,30}").IsMatch(item))
                {
                    throw new DbTableException($"Invalid field name: {item}");
                }
            }
        }

        /// <summary>
        /// Internal method for Index Generator.
        /// </summary>
        private static string _InternalGetIndex(string name)
        {
            // Initialize hash algorithm and get bytes
            var md5 = System.Security.Cryptography.MD5.Create();
            byte[] Result = Encoding.ASCII.GetBytes(name);

            // Compute hash
            for (int i = 0; i < 3; i++)
            {
                Result = md5.ComputeHash(Result);
            }

            // Release resources
            md5.Dispose();

            StringBuilder s = new StringBuilder();
            for (int i = 0; i < Result.Length; i++)
            {
                s.Append(Result[i].ToString("x2"));
            }
            return s.ToString();
        }

        #endregion

        #region Interface Implementation
        public object Clone()
        {
            return new DbTable
            {
                _EntryNames = this._EntryNames,
                _DataTable = this._DataTable,
                FieldNames = this.FieldNames,
            };
        }

        public IEnumerator<string[]> GetEnumerator()
        {
            return _DataTable.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _DataTable.Values.GetEnumerator();
        }

        public void Dispose()
        {
            FieldNames = null;
            _DataTable = null;
            _EntryNames = null;
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronized => false;

        public object SyncRoot => this;

        #endregion
    }
}
