using System.Collections.Generic;
using System.Data;

namespace Smart.Platform.Social.Data.RelativeInteractions
{
    /// <summary>
    /// Defines a class which provides data access for RelativeInteraction data.
    /// </summary>
    public interface IRelativeInteractionDataAccessStrategy
    {
        /// <summary>
        /// Broadcasts the inserted item with the specified ID in the data source.
        /// </summary>
        /// <param name="ID">The ID.</param>
        /// <param name="recipientTypes">The recipientTypes.</param>
        /// <param name="degreesofSeparation">The degreesofSeparation.</param>
        DataSet BroadcastOnInsert(  int ID, 
                                    List<RelativeInteractionBroadcastRecipientTypes> recipientTypes,
                                    int degreesofSeparation);

    }
}
