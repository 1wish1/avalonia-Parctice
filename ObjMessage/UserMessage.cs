using AppoinmentScheduler.Services;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Models;

namespace AppoinmentScheduler.ObjMessages;

public class UserMessage(User result) : ValueChangedMessage<User>(result);
