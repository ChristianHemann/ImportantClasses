using System.Diagnostics;
using ImportantClasses.Enums;

namespace ImportantClasses
{
    /// <summary>
    /// provides a messaging system dedicated to send messages to the user interface without knowing it
    /// </summary>
    public class Message
    {
        /// <summary>
        /// The text of the message
        /// </summary>
        public string MessageText { get; set; }
        /// <summary>
        /// how the message is classificated
        /// </summary>
        public MessageCode MessageCode { get; }

        /// <summary>
        /// provides a messaging system dedicated to send messages to the user interface without knowing it
        /// </summary>
        /// <param name="text">the text of the message</param>
        /// <param name="code">how the message is classificated</param>
        public Message(string text, MessageCode code = MessageCode.Notification)
        {
            MessageText = text;
            MessageCode = code;
        }

        /// <summary>
        /// invokes the NewMessage event
        /// </summary>
        /// <param name="sender">the sender of the message. In most cases its 'this'"</param>
        /// <param name="message">the message to send</param>
        public static void SendMessage(object sender, Message message)
        {
            NewMessage?.Invoke(sender, message);
#if DEBUG
            Debug.Print(message.MessageCode.ToString() + ": "+message.MessageText);
#endif
        }

        /// <summary>
        /// invokes the NewMessage event
        /// </summary>
        /// <param name="sender">the sender of the message. In most cases its 'this'"</param>
        /// <param name="messageText">the text of the message</param>
        /// <param name="code">how the message if classified</param>
        public static void SendMessage(object sender, string messageText, MessageCode code = MessageCode.Notification)
        {
            SendMessage(sender, new Message(messageText, code));
        }

        public delegate void NewMessageDelegate(object sender, Message message);
        /// <summary>
        /// invoked when the SendMessage function is called
        /// </summary>
        public static event NewMessageDelegate NewMessage;
    }
}
