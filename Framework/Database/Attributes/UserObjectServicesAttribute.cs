// <copyright filename="UserObjectServicesAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

using B1PP.Enumerations;

namespace B1PP.Database.Attributes
{
    using System;

    using SAPbobsCOM;

    public sealed class UserObjectServicesAttribute : Attribute
    {
        private readonly ObjectServices[] services;

        public UserObjectServicesAttribute(params ObjectServices[] services)
        {
            this.services = services;
        }

        internal void Apply(UserObjectsMD userObject)
        {
            // the properties are unset (null) at this point in the userObject
            // we set them to tNO to avoid the no value found exception
            DisableAllServices(userObject);

            foreach (var service in services)
            {
                ActivateService(service, userObject);
            }
        }

        private void ActivateService(ObjectServices service, UserObjectsMD userObject)
        {
            switch (service)
            {
                case ObjectServices.Find:
                    userObject.CanFind = BoYesNoEnum.tYES;
                    break;
                case ObjectServices.Delete:
                    userObject.CanDelete = BoYesNoEnum.tYES;
                    break;
                case ObjectServices.Cancel:
                    userObject.CanCancel = BoYesNoEnum.tYES;
                    break;
                case ObjectServices.Close:
                    userObject.CanClose = BoYesNoEnum.tYES;
                    break;
                case ObjectServices.Log:
                    userObject.CanLog = BoYesNoEnum.tYES;
                    userObject.LogTableName = $"{userObject.TableName}_LOG";
                    break;
                case ObjectServices.ManageSeries:
                    userObject.ManageSeries = BoYesNoEnum.tYES;
                    break;
                case ObjectServices.YearTransfer:
                    userObject.CanYearTransfer = BoYesNoEnum.tYES;
                    break;
                case ObjectServices.Default:
                    userObject.CanFind = BoYesNoEnum.tYES;
                    userObject.CanDelete = BoYesNoEnum.tYES;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(service), service, "Unknown service");
            }
        }

        private void DisableAllServices(UserObjectsMD userObject)
        {
            userObject.CanFind = BoYesNoEnum.tNO;
            userObject.CanDelete = BoYesNoEnum.tNO;
            userObject.CanCancel = BoYesNoEnum.tNO;
            userObject.CanClose = BoYesNoEnum.tNO;
            userObject.CanLog = BoYesNoEnum.tNO;
            userObject.ManageSeries = BoYesNoEnum.tNO;
            userObject.CanYearTransfer = BoYesNoEnum.tNO;
        }
    }
}