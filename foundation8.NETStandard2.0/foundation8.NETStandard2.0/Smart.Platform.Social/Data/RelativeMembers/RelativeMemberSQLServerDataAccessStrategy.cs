using System;
using System.Data;
using System.Globalization;
using System.Collections.Generic;
using System.Xml;
using Smart.Platform.Data;
using Smart.Platform.Data.DataAccessStrategies;
using Smart.Platform.Data.DataAccessStrategies.SQLServer;
using Smart.Platform.Diagnostics;

namespace Smart.Platform.Social.Data.RelativeMembers
{
    /// <summary>
    /// Provides SQL Server data access for RelativeMember data.
    /// </summary>
    public class RelativeMemberSQLServerDataAccessStrategy : SQLServerDataAccessStrategyBase, IRelativeMemberDataAccessStrategy
    {
        public enum StoredProcedureParameterKeys
        {
            ID,
            ApplicationID,
            UserProfileID,
            Email,
            ConnectionContractType,
            FindText,
            CurrentRelativeMemberID,
            ConnectionContractTypes,
            PreviousRelativeMemberID,
            NumberOfItemsToLoad,
            SelectItemsAfterPreviousYN,
            ScopeType,
            AspectTypes,
            MaxResults
        }

        #region Constructors

        private RelativeMemberSQLServerDataAccessStrategy() : base() { }

        public RelativeMemberSQLServerDataAccessStrategy(string connectionString,
                                                            CultureInfo cultureInfo)
            : base(connectionString, cultureInfo, "RelativeMembers")
        { }

        #endregion

        #region IRelativeMemberDataAccessStrategy Methods

        /// <summary>
        /// Selects the by ID.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="id">The ID.</param>
        /// <param name="connectionContractType">The connectionContractType.</param>
        /// <param name="currentRelativeMemberID">The currentRelativeMemberID.</param>
        /// <returns></returns>
        IDataItemCollection IRelativeMemberDataAccessStrategy.SelectByID(   IDataItemCollection collection,
                                                                            int id, 
                                                                            RelativeConnectionContractTypes connectionContractType,
                                                                            string currentRelativeMemberID)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));

            #endregion

            // Get the parameters collection
            SQLServerParametersCollection parameters = new SQLServerParametersCollection();
            parameters.Add(StoredProcedureParameterKeys.ID.ToString(), id);
            parameters.Add(StoredProcedureParameterKeys.ConnectionContractType.ToString(), (int)connectionContractType);
            parameters.Add(StoredProcedureParameterKeys.CurrentRelativeMemberID.ToString(), currentRelativeMemberID);

            // Call the stored procedure
            DataSet data = this.RunProcedureLoad("sp_RelativeMembersSelectByIDConnectionContractType", parameters);

            // Fill the collection with the loaded data
            this.FillCollection(collection, data.Tables[0]);

            // Process the additional dataset tables
            this.DoLoadDataSetTables(collection, data);

            return collection;
        }

        /// <summary>
        /// Selects the by UserProfileID.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="userProfileID">The userProfileID.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        /// <exception cref="System.ApplicationException"></exception>
        IDataItemCollection IRelativeMemberDataAccessStrategy.SelectByUserProfileID(IDataItemCollection collection,
                                                                                    string applicationID,
                                                                                    string userProfileID)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));
            if (String.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));

            #endregion

            // Get the parameters collection
            SQLServerParametersCollection parameters = new SQLServerParametersCollection();
            parameters.Add(StoredProcedureParameterKeys.ApplicationID.ToString(), applicationID);
            parameters.Add(StoredProcedureParameterKeys.UserProfileID.ToString(), userProfileID);

            // Call the stored procedure
            DataSet data = this.RunProcedureLoad("sp_RelativeMembersSelectByUserProfileID", parameters);

            // Fill the collection with the loaded data
            this.FillCollection(collection, data.Tables[0]);

            // Process the additional dataset tables
            this.DoLoadDataSetTables(collection, data);

            return collection;
        }

        /// <summary>
        /// Selects the by email.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        /// <exception cref="System.ApplicationException"></exception>
        IDataItemCollection IRelativeMemberDataAccessStrategy.SelectByEmail(IDataItemCollection collection,
                                                                            string applicationID, 
                                                                            string email)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));
            if (String.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));

            #endregion

            // Get the parameters collection
            SQLServerParametersCollection parameters = new SQLServerParametersCollection();
            parameters.Add(StoredProcedureParameterKeys.ApplicationID.ToString(), applicationID);
            parameters.Add(StoredProcedureParameterKeys.Email.ToString(), email);

            // Call the stored procedure
            DataSet data = this.RunProcedureLoad("sp_RelativeMembersSelectByEmail", parameters);

            // Fill the collection with the loaded data
            this.FillCollection(collection, data.Tables[0]);

            // Process the additional dataset tables
            this.DoLoadDataSetTables(collection, data);

            return collection;
        }

        /// <summary>
        /// Selects the by email.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="email">The email.</param>
        /// <param name="connectionContractType">The connectionContractType.</param>
        /// <param name="currentRelativeMemberID">The currentRelativeMemberID.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">
        /// </exception>
        IDataItemCollection IRelativeMemberDataAccessStrategy.SelectByEmail(IDataItemCollection collection,
                                                                            string applicationID,
                                                                            string email, 
                                                                            RelativeConnectionContractTypes connectionContractType,
                                                                            string currentRelativeMemberID)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));
            if (String.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));
            if (string.IsNullOrEmpty(email)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "email is nothing"));

            #endregion

            // Get the parameters collection
            SQLServerParametersCollection parameters = new SQLServerParametersCollection();
            parameters.Add(StoredProcedureParameterKeys.ApplicationID.ToString(), applicationID);
            parameters.Add(StoredProcedureParameterKeys.Email.ToString(), email);
            parameters.Add(StoredProcedureParameterKeys.ConnectionContractType.ToString(), (int)connectionContractType);
            parameters.Add(StoredProcedureParameterKeys.CurrentRelativeMemberID.ToString(), currentRelativeMemberID);

            // Call the stored procedure
            DataSet data = this.RunProcedureLoad("sp_RelativeMembersSelectByEmailConnectionContractType", parameters);

            // Fill the collection with the loaded data
            this.FillCollection(collection, data.Tables[0]);

            // Process the additional dataset tables
            this.DoLoadDataSetTables(collection, data);

            return collection;
        }

        /// <summary>
        /// Selects the by findtext.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="findText">The findtext.</param>
        /// <param name="currentRelativeMemberID">The currentRelativeMemberID.</param>
        /// <param name="scopeType">The scopeType.</param>
        /// <param name="previousRelativeMemberID">The previousRelativeMemberID.</param>
        /// <param name="numberOfItemsToLoad">The numberOfItemsToLoad.</param>
        /// <param name="selectItemsAfterPreviousYN">The selectItemsAfterPreviousYN.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">
        /// </exception>
        IDataItemCollection IRelativeMemberDataAccessStrategy.SelectByFindText( IDataItemCollection collection,
                                                                                string applicationID,
                                                                                string findText,
                                                                                string currentRelativeMemberID,
                                                                                RelativeMemberScopeTypes scopeType,
                                                                                string previousRelativeMemberID,
                                                                                int numberOfItemsToLoad, 
                                                                                bool selectItemsAfterPreviousYN)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));
            if (String.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));
            if (string.IsNullOrEmpty(findText)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "findText is nothing"));
            if (string.IsNullOrEmpty(currentRelativeMemberID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "currentRelativeMemberID is nothing"));
            if (string.IsNullOrEmpty(previousRelativeMemberID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "previousRelativeMemberID is nothing"));

            #endregion

            // Get the parameters collection
            SQLServerParametersCollection parameters = new SQLServerParametersCollection();
            parameters.Add(StoredProcedureParameterKeys.ApplicationID.ToString(), applicationID);
            parameters.Add(StoredProcedureParameterKeys.FindText.ToString(), findText);
            parameters.Add(StoredProcedureParameterKeys.CurrentRelativeMemberID.ToString(), currentRelativeMemberID);
            parameters.Add(StoredProcedureParameterKeys.ScopeType.ToString(), (int)scopeType);
            parameters.Add(StoredProcedureParameterKeys.PreviousRelativeMemberID.ToString(), previousRelativeMemberID);
            parameters.Add(StoredProcedureParameterKeys.NumberOfItemsToLoad.ToString(), numberOfItemsToLoad);
            parameters.Add(StoredProcedureParameterKeys.SelectItemsAfterPreviousYN.ToString(), selectItemsAfterPreviousYN);

            // Call the stored procedure
            DataSet data = this.RunProcedureLoad("sp_RelativeMembersSelectByFindTextPreviousRelativeMemberID", parameters);

            // Fill the collection with the loaded data
            this.FillCollection(collection, data.Tables[0]);

            // Process the additional dataset tables
            this.DoLoadDataSetTables(collection, data);

            return collection;
        }

        /// <summary>
        /// Selects the by aspecttypes.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="aspectTypes">The aspectTypes.</param>
        /// <param name="maxResults">The maxResults.</param>
        /// <param name="currentRelativeMemberID">The currentRelativeMemberID.</param>
        /// <param name="scopeType">The scopeType.</param>
        /// <param name="previousRelativeMemberID">The previousRelativeMemberID.</param>
        /// <param name="numberOfItemsToLoad">The numberOfItemsToLoad.</param>
        /// <param name="selectItemsAfterPreviousYN">The selectItemsAfterPreviousYN.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">
        /// </exception>
        IDataItemCollection IRelativeMemberDataAccessStrategy.SelectByAspects(  IDataItemCollection collection,
                                                                                string applicationID,
                                                                                List<RelativeMemberQueryAspectTypes> aspectTypes,
                                                                                int maxResults,
                                                                                string currentRelativeMemberID,
                                                                                RelativeMemberScopeTypes scopeType,
                                                                                string previousRelativeMemberID,
                                                                                int numberOfItemsToLoad,
                                                                                bool selectItemsAfterPreviousYN)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));
            if (String.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));
            if (aspectTypes == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "aspectTypes is nothing"));
            if (string.IsNullOrEmpty(currentRelativeMemberID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "currentRelativeMemberID is nothing"));
            if (string.IsNullOrEmpty(previousRelativeMemberID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "previousRelativeMemberID is nothing"));

            #endregion

            // Get the parameters collection
            SQLServerParametersCollection parameters = new SQLServerParametersCollection();
            parameters.Add(StoredProcedureParameterKeys.ApplicationID.ToString(), applicationID);
            parameters.Add(StoredProcedureParameterKeys.AspectTypes.ToString(), this.relativeMemberQueryAspectTypesFromList(aspectTypes));
            parameters.Add(StoredProcedureParameterKeys.MaxResults.ToString(), maxResults);
            parameters.Add(StoredProcedureParameterKeys.CurrentRelativeMemberID.ToString(), currentRelativeMemberID);
            parameters.Add(StoredProcedureParameterKeys.ScopeType.ToString(), (int)scopeType);
            parameters.Add(StoredProcedureParameterKeys.PreviousRelativeMemberID.ToString(), previousRelativeMemberID);
            parameters.Add(StoredProcedureParameterKeys.NumberOfItemsToLoad.ToString(), numberOfItemsToLoad);
            parameters.Add(StoredProcedureParameterKeys.SelectItemsAfterPreviousYN.ToString(), selectItemsAfterPreviousYN);

            // Call the stored procedure
            DataSet data = this.RunProcedureLoad("sp_RelativeMembersSelectByAspectsPreviousRelativeMemberID", parameters);

            // Fill the collection with the loaded data
            this.FillCollection(collection, data.Tables[0]);

            // Process the additional dataset tables
            this.DoLoadDataSetTables(collection, data);

            return collection;
        }

        #endregion

        #region Protected Override Methods

        /// <summary>
        /// Builds and returns the collection of parameters. Remove the omitted parameters which are only returned by select 
        /// stored procedures and are not columns in the table.
        /// </summary>
        /// <param name="item">The data item.</param>
        /// <param name="outputParameters"></param>
        /// <returns>
        /// A <see cref="ParametersCollection"/> instance.
        /// </returns>
        protected override IParametersCollection BuildParameters(IDataItem item, bool outputParameters)
        {
            // Build the parameter collection in the base class
            IParametersCollection p = base.BuildParameters(item, outputParameters);

            // Remove the ConnectionContractTypes parameter, because this data column is only returned by select stored procedures
            // and is not a column in the table
            //p.Remove(StoredProcedureParameterKeys.ConnectionContractTypes.ToString());

            return p;
        }

        #endregion

        #region Private Methods

        private string relativeMemberQueryAspectTypesFromList(List<RelativeMemberQueryAspectTypes> relativeMemberQueryAspectTypes)
        {
            string result = "";

            // Go through each item
            relativeMemberQueryAspectTypes.ForEach(item =>
            {
                if (result.Length > 0)
                {
                    result += ",";
                }

                result += ((int)item).ToString();

            });

            return result;
        }

        #endregion
    }
}
