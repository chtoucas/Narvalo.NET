using LumenWorks.Framework.IO.Csv;

namespace Narvalo.LumenWorks
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Classe permettant de lire d'un coup toutes les lignes d'un fichier CSV.
    /// </summary>
    public class CvsSlurp
    {
        #region Fields

        private char _delimiter = ';';
        private Encoding _encoding = Encoding.GetEncoding(1252);
        private bool _hasHeaders = true;

        #endregion

        #region Properties

        /// <summary>
        /// Le séparateur utilisé pour distinguer des champs dans une même ligne.
        /// </summary>
        public char Delimiter
        {
            get { return _delimiter; }
            set { _delimiter = value; }
        }

        /// <summary>
        /// Type d'encodage texte utilisé.
        /// </summary>
        public Encoding Encoding
        {
            get { return _encoding; }
            set { _encoding = value; }
        }

        /// <summary>
        /// Vrai si la première ligne du CSV sert pour décrire les champs.
        /// </summary>
        public bool HasHeaders
        {
            get { return _hasHeaders; }
            set { _hasHeaders = value; }
        }

        /// <summary>
        /// Liste des en-têtes si il y en a.
        /// </summary>
        public IList<string> Headers
        {
            get;
            private set;
        }

        /// <summary>
        /// Liste des éléments trouvés.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "FIXME")]
        public IList<Dictionary<string, string>> Records
        {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Analyse un fichier CSV.
        /// </summary>
        /// <param name="fileName">Nom du fichier CSV</param>
        public void Parse(string fileName)
        {
            EnsureInitialized();

            using (StreamReader streamReader = new StreamReader(fileName, Encoding)) {
                CsvReader reader = new CsvReader(streamReader, HasHeaders, Delimiter);

                // Configure the CSV parser to throw an exception on error
                reader.DefaultParseErrorAction = ParseErrorAction.ThrowException;

                int fieldCount = reader.FieldCount;
                Headers = reader.GetFieldHeaders();

                while (reader.ReadNextRecord()) {
                    Dictionary<string, string> record = new Dictionary<string, string>();

                    for (int i = 0; i < fieldCount; i++) {
                        record.Add(Headers[i], reader[i]);
                    }

                    Records.Add(record);
                }
            }
        }

        /// <summary>
        /// Analyse un fichier CSV et retourne la liste des éléments trouvés.
        /// </summary>
        /// <param name="fileName">Nom du fichier CSV</param>
        /// <returns>Liste des éléments trouvés</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "FIXME")]
        public IList<Dictionary<string, string>> Read(string fileName)
        {
            Parse(fileName);

            return Records;
        }

        #region Protected methods

        protected void EnsureInitialized()
        {
            Headers = new string[] { };
            Records = new List<Dictionary<string, string>>();
        }

        #endregion
    }
}
