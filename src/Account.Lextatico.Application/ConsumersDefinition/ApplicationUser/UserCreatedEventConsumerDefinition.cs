using Account.Lextatico.Application.Consumers.ApplicationUser;

namespace Account.Lextatico.Application.ConsumersDefinition.ApplicationUser
{
    public class UserCreatedEventConsumerDefinition : BaseConsumerDefinition<UserCreatedEventConsumer>
    {
        public UserCreatedEventConsumerDefinition()
            : base("lextatico.exchange:UserCreatedEvent", "lextatico.UserCreated", "auth.lextatico.queue.UserCreatedEvent")
        {
        }
    }
}
