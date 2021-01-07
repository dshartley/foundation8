using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smart.Platform.Data.DataManagementPolicies
{
    public class DefaultDataManagementPolicy : IDataManagementPolicy
    {
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
