namespace DotMSN
{
    using System;

    internal interface IInvitationHandler
    {
        // Methods
        void HandleMessage(Conversation conversation, Contact sender, string applicationName, int cookie, string header, string body);

    }}

