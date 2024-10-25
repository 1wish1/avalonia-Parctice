using AppoinmentScheduler.Services;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AppoinmentScheduler.ObjMessages;

public class AuthenticationMessage(OAuthToken result) : ValueChangedMessage<OAuthToken>(result);
