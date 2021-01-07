using Smart.Platform.Data;
using Smart.Platform.Diagnostics;
using Smart.Platform.Social.Data.RelativeConnectionRequests;
using Smart.Platform.Social.Data.RelativeConnections;
using Smart.Platform.Social.Data.RelativeInteractions;
using Smart.Platform.Social.Data.RelativeMembers;
using Smart.Platform.Social.Data.RelativeTimelineEvents;
using System;
using System.Collections.Generic;

namespace Smart.Platform.Social.Data
{
    /// <summary>
    /// Manages the data layer and provides access to the data administrators.
    /// </summary>
    public class SocialDataManager : IDataAdministratorProvider
    {
        private string                                      _defaultCultureInfoName;
        private Dictionary<string, IDataAdministrator>      _dataAdministrators;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialDataManager"/> class.
        /// </summary>
        private SocialDataManager() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialDataManager"/> class.
        /// </summary>
        /// <param name="defaultCultureInfoName">Default name of the culture info.</param>
        public SocialDataManager(string defaultCultureInfoName)
        {
            #region Check Parameters

            if (defaultCultureInfoName == String.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "defaultCultureInfoName is nothing"));

            #endregion

            _defaultCultureInfoName     = defaultCultureInfoName;
            _dataAdministrators         = new Dictionary<string, IDataAdministrator>();
        }

        #endregion

        #region Public Methods

        #region RelativeConnectionRequests

        private RelativeConnectionRequestDataAdministrator  _relativeConnectionRequestsDA;
        private IDataManagementPolicy                       _relativeConnectionRequestsDMP;
        private IDataAccessStrategy                         _relativeConnectionRequestsDAS;

        /// <summary>
        /// Setups the RelativeConnectionRequest data administrator.
        /// </summary>
        /// <param name="relativeConnectionRequestsDMP">The relativeConnectionRequests DMP.</param>
        /// <param name="relativeConnectionRequestsDAS">The relativeConnectionRequests DAS.</param>
        public void SetupRelativeConnectionRequestDataAdministrator(IDataManagementPolicy relativeConnectionRequestsDMP, IDataAccessStrategy relativeConnectionRequestsDAS)
        {
            #region Check Parameters

            if (relativeConnectionRequestsDMP == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "relativeConnectionRequestsDMP is nothing"));
            if (relativeConnectionRequestsDAS == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "relativeConnectionRequestsDAS is nothing"));

            #endregion

            if (_dataAdministrators.ContainsKey("RelativeConnectionRequests")) _dataAdministrators.Remove("RelativeConnectionRequests");

            _relativeConnectionRequestsDMP  = relativeConnectionRequestsDMP;
            _relativeConnectionRequestsDAS  = relativeConnectionRequestsDAS;
            _relativeConnectionRequestsDA   = new RelativeConnectionRequestDataAdministrator(_relativeConnectionRequestsDMP, _relativeConnectionRequestsDAS, _defaultCultureInfoName, this);

            _dataAdministrators["RelativeConnectionRequests"] = _relativeConnectionRequestsDA;
        }

        /// <summary>
        /// Gets the RelativeConnectionRequest data administrator.
        /// </summary>
        /// <value>The RelativeConnectionRequest data administrator.</value>
        public RelativeConnectionRequestDataAdministrator RelativeConnectionRequestDataAdministrator
        {
            get { return _relativeConnectionRequestsDA; }
        }

        #endregion

        #region RelativeConnections

        private RelativeConnectionDataAdministrator _relativeConnectionsDA;
        private IDataManagementPolicy               _relativeConnectionsDMP;
        private IDataAccessStrategy                 _relativeConnectionsDAS;

        /// <summary>
        /// Setups the RelativeConnection data administrator.
        /// </summary>
        /// <param name="relativeConnectionsDMP">The relativeConnections DMP.</param>
        /// <param name="relativeConnectionsDAS">The relativeConnections DAS.</param>
        public void SetupRelativeConnectionDataAdministrator(IDataManagementPolicy relativeConnectionsDMP, IDataAccessStrategy relativeConnectionsDAS)
        {
            #region Check Parameters

            if (relativeConnectionsDMP == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "relativeConnectionsDMP is nothing"));
            if (relativeConnectionsDAS == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "relativeConnectionsDAS is nothing"));

            #endregion

            if (_dataAdministrators.ContainsKey("RelativeConnections")) _dataAdministrators.Remove("RelativeConnections");

            _relativeConnectionsDMP = relativeConnectionsDMP;
            _relativeConnectionsDAS = relativeConnectionsDAS;
            _relativeConnectionsDA  = new RelativeConnectionDataAdministrator(_relativeConnectionsDMP, _relativeConnectionsDAS, _defaultCultureInfoName, this);

            _dataAdministrators["RelativeConnections"] = _relativeConnectionsDA;
        }

        /// <summary>
        /// Gets the RelativeConnection data administrator.
        /// </summary>
        /// <value>The RelativeConnection data administrator.</value>
        public RelativeConnectionDataAdministrator RelativeConnectionDataAdministrator
        {
            get { return _relativeConnectionsDA; }
        }

        #endregion

        #region RelativeInteractions

        private RelativeInteractionDataAdministrator    _relativeInteractionsDA;
        private IDataManagementPolicy                   _relativeInteractionsDMP;
        private IDataAccessStrategy                     _relativeInteractionsDAS;

        /// <summary>
        /// Setups the RelativeInteraction data administrator.
        /// </summary>
        /// <param name="relativeInteractionsDMP">The relativeInteractions DMP.</param>
        /// <param name="relativeInteractionsDAS">The relativeInteractions DAS.</param>
        public void SetupRelativeInteractionDataAdministrator(IDataManagementPolicy relativeInteractionsDMP, IDataAccessStrategy relativeInteractionsDAS)
        {
            #region Check Parameters

            if (relativeInteractionsDMP == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "relativeInteractionsDMP is nothing"));
            if (relativeInteractionsDAS == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "relativeInteractionsDAS is nothing"));

            #endregion

            if (_dataAdministrators.ContainsKey("RelativeInteractions")) _dataAdministrators.Remove("RelativeInteractions");

            _relativeInteractionsDMP    = relativeInteractionsDMP;
            _relativeInteractionsDAS    = relativeInteractionsDAS;
            _relativeInteractionsDA     = new RelativeInteractionDataAdministrator(_relativeInteractionsDMP, _relativeInteractionsDAS, _defaultCultureInfoName, this);

            _dataAdministrators["RelativeInteractions"] = _relativeInteractionsDA;
        }

        /// <summary>
        /// Gets the RelativeInteraction data administrator.
        /// </summary>
        /// <value>The RelativeInteraction data administrator.</value>
        public RelativeInteractionDataAdministrator RelativeInteractionDataAdministrator
        {
            get { return _relativeInteractionsDA; }
        }

        #endregion

        #region RelativeMembers

        private RelativeMemberDataAdministrator _relativeMembersDA;
        private IDataManagementPolicy           _relativeMembersDMP;
        private IDataAccessStrategy             _relativeMembersDAS;

        /// <summary>
        /// Setups the RelativeMember data administrator.
        /// </summary>
        /// <param name="relativeMembersDMP">The relativeMembers DMP.</param>
        /// <param name="relativeMembersDAS">The relativeMembers DAS.</param>
        public void SetupRelativeMemberDataAdministrator(IDataManagementPolicy relativeMembersDMP, IDataAccessStrategy relativeMembersDAS)
        {
            #region Check Parameters

            if (relativeMembersDMP == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "relativeMembersDMP is nothing"));
            if (relativeMembersDAS == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "relativeMembersDAS is nothing"));

            #endregion

            if (_dataAdministrators.ContainsKey("RelativeMembers")) _dataAdministrators.Remove("RelativeMembers");

            _relativeMembersDMP = relativeMembersDMP;
            _relativeMembersDAS = relativeMembersDAS;
            _relativeMembersDA  = new RelativeMemberDataAdministrator(_relativeMembersDMP, _relativeMembersDAS, _defaultCultureInfoName, this);

            _dataAdministrators["RelativeMembers"] = _relativeMembersDA;
        }

        /// <summary>
        /// Gets the RelativeMember data administrator.
        /// </summary>
        /// <value>The RelativeMember data administrator.</value>
        public RelativeMemberDataAdministrator RelativeMemberDataAdministrator
        {
            get { return _relativeMembersDA; }
        }

        #endregion

        #region RelativeTimelineEvents

        private RelativeTimelineEventDataAdministrator  _relativeTimelineEventsDA;
        private IDataManagementPolicy                   _relativeTimelineEventsDMP;
        private IDataAccessStrategy                     _relativeTimelineEventsDAS;

        /// <summary>
        /// Setups the RelativeTimelineEvent data administrator.
        /// </summary>
        /// <param name="relativeTimelineEventsDMP">The relativeTimelineEvents DMP.</param>
        /// <param name="relativeTimelineEventsDAS">The relativeTimelineEvents DAS.</param>
        public void SetupRelativeTimelineEventDataAdministrator(IDataManagementPolicy relativeTimelineEventsDMP, IDataAccessStrategy relativeTimelineEventsDAS)
        {
            #region Check Parameters

            if (relativeTimelineEventsDMP == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "relativeTimelineEventsDMP is nothing"));
            if (relativeTimelineEventsDAS == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "relativeTimelineEventsDAS is nothing"));

            #endregion

            if (_dataAdministrators.ContainsKey("RelativeTimelineEvents")) _dataAdministrators.Remove("RelativeTimelineEvents");

            _relativeTimelineEventsDMP  = relativeTimelineEventsDMP;
            _relativeTimelineEventsDAS  = relativeTimelineEventsDAS;
            _relativeTimelineEventsDA   = new RelativeTimelineEventDataAdministrator(_relativeTimelineEventsDMP, _relativeTimelineEventsDAS, _defaultCultureInfoName, this);

            _dataAdministrators["RelativeTimelineEvents"] = _relativeTimelineEventsDA;
        }

        /// <summary>
        /// Gets the RelativeTimelineEvent data administrator.
        /// </summary>
        /// <value>The RelativeTimelineEvent data administrator.</value>
        public RelativeTimelineEventDataAdministrator RelativeTimelineEventDataAdministrator
        {
            get { return _relativeTimelineEventsDA; }
        }

        #endregion

        #endregion

        #region IDataAdministratorProvider Members

        /// <summary>
        /// Gets the data administrator with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public IDataAdministrator GetDataAdministrator(string key)
        {
            #region Check Parameters

            if (String.IsNullOrEmpty(key)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is nothing"));
            if (!_dataAdministrators.ContainsKey(key)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is invalid"));

            #endregion

            if (_dataAdministrators.ContainsKey(key)) return _dataAdministrators[key];
            return null;
        }

        #endregion
    }
}
