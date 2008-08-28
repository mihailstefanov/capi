namespace Mommosoft.Capi {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.ComponentModel;

    public abstract class MessageReceiver : Component {
        /// <summary>
        /// Local confirmation of the connect request.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="confirmation"></param>
        internal abstract void ConnectConfirmation(ConnectConfirmation confirmation, MessageAsyncResult result);

        /// <summary>
        /// Local confirmation of the request.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="confirmation"></param>
        internal abstract void ListenConfirmation(ListenConfirmation confirmation, MessageAsyncResult result);

        internal abstract void FacilityConfirmation(FacilityConfirmation confirmation, MessageAsyncResult result);
        internal abstract void DisconnectB3Confirmation(DisconnectB3Confirmation confirmation, MessageAsyncResult result);
        internal abstract void DataB3Confirmation(DataB3Confirmation confirmation, MessageAsyncResult result);

        /// <summary>
        /// Indicates an incoming physical connection.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="indication"></param>
        internal abstract void ConnectIndication(ConnectIndication indication);

        /// <summary>
        /// Indicates the activation of a physical connection.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="indication"></param>
        internal abstract void ConnectActiveIndication(ConnectActiveIndication indication);

        /// <summary>
        /// Indicates an incoming logical connection.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="indication"></param>
        internal abstract void ConnectB3Indication(ConnectB3Indication indication);

        /// <summary>
        /// Indicates the activation of a logical connection
        /// </summary>
        /// <param name="header"></param>
        /// <param name="indication"></param>
        internal abstract void ConnectB3ActiveIndication(ConnectB3ActiveIndication indication);

        /// <summary>
        /// Indicates the clearing down of a logical connection.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="indication"></param>
        internal abstract void DisconnectB3Indication(DisconnectB3Indication indication);

        /// <summary>
        /// Indicates the clearing of a physical connection
        /// </summary>
        /// <param name="header"></param>
        /// <param name="indication"></param>
        internal abstract void DisconnectIndication(DisconnectIndication indication);

        /// <summary>
        /// Indicates additional facilities (e.g. ext. equipment).
        /// </summary>
        /// <param name="header"></param>
        /// <param name="indication"></param>
        internal abstract void FacilityIndication(FacilityIndication indication);


    }
}
