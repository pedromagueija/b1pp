// <copyright filename="DisposableRecordset.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Data
{
    using System;

    using Common;

    using Extensions.SDK.DI;

    using SAPbobsCOM;

    /// <summary>
    /// Wraps an SAP Business One Recordset and adds disposing capabilities.
    /// </summary>
    /// <remarks>
    /// Calling any method on an instance of this class after disposing may result in an exception.
    /// </remarks>
    /// <seealso cref="SAPbobsCOM.IRecordset" />
    /// <seealso cref="System.IDisposable" />
    internal class DisposableRecordset : IRecordset, IDisposable
    {
        private readonly Recordset recordset;
        public bool IsDisposed { get; private set; }

        public Fields Fields
        {
            get
            {
                return recordset.Fields;
            }
        }

        public bool BoF
        {
            get
            {
                return recordset.BoF;
            }
        }

        public bool EoF
        {
            get
            {
                return recordset.EoF;
            }
        }

        public int RecordCount
        {
            get
            {
                return recordset.RecordCount;
            }
        }

        public Command Command
        {
            get
            {
                return recordset.Command;
            }
        }

        public DisposableRecordset(Company company)
        {
            recordset = company.Get<Recordset>(BoObjectTypes.BoRecordset);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableRecordset" /> class.
        /// </summary>
        /// <param name="recordset">The recordset.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Throw when the provided recordset is null.
        /// </exception>
        public DisposableRecordset(Recordset recordset)
        {
            if (recordset == null)
            {
                throw new ArgumentNullException(nameof(recordset));
            }

            this.recordset = recordset;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void MoveNext()
        {
            recordset.MoveNext();
        }

        public void MovePrevious()
        {
            recordset.MovePrevious();
        }

        public void DoQuery(string queryStr)
        {
            recordset.DoQuery(queryStr);
        }

        public void SaveXML(ref string fileName)
        {
            recordset.SaveXML(fileName);
        }

        public void MoveFirst()
        {
            recordset.MoveFirst();
        }

        public void MoveLast()
        {
            recordset.MoveLast();
        }

        public string GetAsXML()
        {
            return recordset.GetAsXML();
        }

        public void SaveToFile(string fileName)
        {
            recordset.SaveToFile(fileName);
        }

        public string GetFixedXML(RecordsetXMLModeEnum xmlMode)
        {
            return recordset.GetFixedXML(xmlMode);
        }

        public string GetFixedSchema()
        {
            return recordset.GetFixedSchema();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ReleaseUnmanagedResources();
            }

            IsDisposed = true;
        }

        private void ReleaseUnmanagedResources()
        {
            Utilities.Release(recordset);
        }

        ~DisposableRecordset()
        {
            Dispose(false);
        }
    }
}