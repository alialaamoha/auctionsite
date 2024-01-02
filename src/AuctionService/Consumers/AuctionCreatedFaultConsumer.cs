using Contracts;
using MassTransit;

namespace AuctionService;

public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
{
    public Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
    {
        throw new NotImplementedException();
    }
}
