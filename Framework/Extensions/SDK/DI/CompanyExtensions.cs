// <copyright filename="CompanyExtensions.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Extensions.SDK.DI
{
    using SAPbobsCOM;

    public static class CompanyExtensions
    {
        public static T Get<T>(this Company company, BoObjectTypes type)
        {
            return (T) company.GetBusinessObject(type);
        }
    }
}