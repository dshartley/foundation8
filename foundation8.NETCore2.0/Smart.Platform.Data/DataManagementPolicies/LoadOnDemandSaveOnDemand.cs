using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smart.Platform.Data.DataManagementPolicies
{
    /// <summary>
    /// A data management policy where data is loaded manually on demand and saved
    /// manually on demand, therefore remaining fairly passive
    /// </summary>
    public class LoadOnDemandSaveOnDemand : IDataManagementPolicy
    {
        #region Constructors

        public LoadOnDemandSaveOnDemand() { }

        #endregion

        #region IDataManagementPolicy Members

        public void Initialise(IDataAdministrator dataAdministrator)
        {
            // TODO:
        }

        protected IDataAdministrator _dataAdministrator;

        public IDataAdministrator DataAdministrator
        {
            get { return _dataAdministrator; }
        }

        #endregion
    }
}
