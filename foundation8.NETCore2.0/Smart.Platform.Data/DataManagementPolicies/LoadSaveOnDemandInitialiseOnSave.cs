using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smart.Platform.Diagnostics;

namespace Smart.Platform.Data.DataManagementPolicies
{
    /// <summary>
    /// A data management policy where data is loaded manually on demand and saved
    /// manually on demand, and then when saved the data administrator is reinitialised
    /// </summary>
    public class LoadSaveOnDemandInitialiseOnSave : IDataManagementPolicy
    {
        #region Constructors

        public LoadSaveOnDemandInitialiseOnSave() { }

        #endregion

        #region IDataManagementPolicy Members

        public void Initialise(IDataAdministrator dataAdministrator)
        {
            #region Check Parameters

            if (dataAdministrator == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataAdministrator is nothing"));

            #endregion

            _dataAdministrator = dataAdministrator;

            // Set up the event handlers for the data administrator
            _dataAdministrator.DataSaved += new DataAdministratorEventHandler(OnDataSaved);
        }

        protected IDataAdministrator _dataAdministrator;

        public IDataAdministrator DataAdministrator
        {
            get { return _dataAdministrator; }
        }

        #endregion

        #region Private Methods

        private void OnDataSaved()
        {
            // Initialise the data administrator when the data is saved
            _dataAdministrator.Initialise();
        }

        #endregion
    }
}
