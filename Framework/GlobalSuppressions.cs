// <copyright filename="GlobalSuppressions.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

using System.Diagnostics.CodeAnalysis;

[assembly:
    SuppressMessage("Sonar Code Smell", "S107:Methods should not have too many parameters",
        Justification = "Necessary for compliance with B1 SDK.", Scope = "member",
        Target =
            "~M:B1PP.Connections.DiApiConnectionSettings.CreateStandardSettings(System.String,System.String,System.String,System.String,System.String,SAPbobsCOM.BoSuppLangs,System.String,SAPbobsCOM.BoDataServerTypes,System.String)~B1PP.Connections.DiApiConnectionSettings")]