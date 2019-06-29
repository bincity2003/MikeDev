using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MikeDev.DB
{
    /// <summary>
    /// DbKeeper class is used for database management.
    /// </summary>
    public class DbTable : ICloneable, IEnumerable<string[]>, IDisposable, ICollection
    {
        #region Internal holder

        private Dictionary<string, int> _EntryNames;                                        // Internal storage for entries' name
        private Dictionary<int, string[]> _DataTable;     // Primary storage for entries

        #endregion

        #region Read-only exposure

        /// <summary>
        /// Get an array containing entries' name.
        /// </summary>
        public string[] GetEntriesNames => _EntryNames.Keys.ToArray();

        /// <summary>
        /// Get an array containing columns' name.
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
                    int Index = _EntryNames[name];
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
            if (fieldNames[0] != "Names")
            {
                throw new DbTableException("First element must be 'Names'!");
            }

            _InternalFieldNameValidator(fieldNames);
            FieldNames = fieldNames;

            _DataTable = new Dictionary<int, string[]>();
            _EntryNames = new Dictionary<string, int>();
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

            // Add to index
            _EntryNames.Add(name, _EntryNames.Count);

            // Add entry to table
            _DataTable.Add(_EntryNames.Count - 1, values);
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

            }
        }

        /// <summary>
        /// Remove an existing entry.
        /// </summary>
        /// <param name="name">The name of the entry to be removed.</param>
        public void RemoveEntry(string name)
        {
            try
            {
                int Index = _EntryNames[name];
                _EntryNames.Remove(name);
                _DataTable.Remove(Index);
            }
            catch (KeyNotFoundException)
            {
                throw new DbTableException("Entry not found!");
            }
        }

        /// <summary>
        /// Replace an existing entry.
        /// </summary>
        /// <param name="name">The name of the entry to be replaced.</param>
        /// <param name="values">The new set of fields' value.</param>
        public void ReplaceEntry(string name, params string[] values)
        {
            try
            {
                int Index = _EntryNames[name];
                _DataTable[Index] = values;
            }
            catch (KeyNotFoundException)
            {
                throw new DbTableException("Entry not found!");
            }
        }

        /// <summary>
        /// Filter results from DbTable.
        /// </summary>
        /// <param name="command">Command to be executed.</param>
        /// <returns>An array of String array (fields).</returns>
        public string[][] Filter(string command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            return _InternalExecutionEngine(command);
        }

        #endregion

        #region Internal methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private string[][] _InternalExecutionEngine(string command)
        {
            // Prepare
            string[][] result;
            string[] Partition = command.Split(' ');

            switch (Partition[0])                                       // Command identifier
            {
                case "SELECT":
                    result = _InternalSelectCommand(Partition);
                    break;
                case "DELETE":
                    result = _InternalDeleteCommand(Partition);
                    break;
                default:                                                // Default (error)
                    throw new DbTableException("Unknown command!");
            }

            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private string[][] _InternalDeleteCommand(string[] parts)
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private string[][] _InternalSelectCommand(string[] parts)
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private void _InternalFieldNameValidator(string[] fieldNames)
        {
            foreach (var item in fieldNames)
            {
                if (!new Regex("[a-zA-Z][0-9a-zA-Z]{0,30}").IsMatch(item))
                {
                    throw new DbTableException($"Invalid field name: {item}");
                }
            }
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
