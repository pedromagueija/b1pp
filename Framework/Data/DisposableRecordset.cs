// <copyright filename="DisposableRecordset.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Data
{
    using System;
    using System.Runtime.InteropServices;

    using Extensions.SDK.DI;

    using SAPbobsCOM;

    /// <summary>
    /// Disposable recordset decorator.
    /// </summary>
    /// <seealso cref="SAPbobsCOM.IRecordset" />
    /// <seealso cref="System.IDisposable" />
    internal class DisposableRecordset : IRecordset, IDisposable
    {
        private readonly Recordset recordset;

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

        public void Dispose()
        {
            ReleaseUnmanagedResources();
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

        private void ReleaseUnmanagedResources()
        {
            if (recordset != null)
            {
                Marshal.ReleaseComObject(recordset);
            }
        }

        ~DisposableRecordset()
        {
            ReleaseUnmanagedResources();
        }
    }
}