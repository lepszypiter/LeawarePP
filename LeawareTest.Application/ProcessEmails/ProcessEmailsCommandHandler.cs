using LeawareTest.Application.Interfaces;
using LeawareTest.BuildingBlocks;
using LeawareTest.BuildingBlocks.Messaging;
using LeawareTest.Domain;
using Microsoft.Extensions.Logging;

namespace LeawareTest.Application.ProcessEmails;

public record ProcessEmailsCommand : ICommand;

public class ProcessEmailsCommandHandler : ICommandHandler<ProcessEmailsCommand>
{
    private readonly IOrderProcessor _orderProcessor;
    private readonly IEmailRepostiory _emailRepostiory;
    private readonly IOrderRepository _orderRepostiory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProcessEmailsCommandHandler> _logger;

    public ProcessEmailsCommandHandler(IOrderProcessor orderProcessor, ILogger<ProcessEmailsCommandHandler> logger, IEmailRepostiory emailRepostiory, IOrderRepository orderRepostiory, IUnitOfWork unitOfWork)
    {
        _orderProcessor = orderProcessor;
        _logger = logger;
        _emailRepostiory = emailRepostiory;
        _orderRepostiory = orderRepostiory;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ProcessEmailsCommand command, CancellationToken cancellationToken)
    {
        var emails = await _emailRepostiory.GetUnprocessedEmails(cancellationToken);
        foreach (var email in emails)
        {
            // Process the order using the order processor
            var orderItems = await _orderProcessor.ExtractOrder(email.Body, cancellationToken);

            foreach (var orderItem in orderItems)
            {
                var newOrderItem = OrderItem.CreateNewOrderItem(new OrderDetails(orderItem.Product, orderItem.Count, orderItem.Price), email);
                await _orderRepostiory.AddNewOrderitem(newOrderItem, cancellationToken);
            }

            email.MarkAsProcessed();

            // Save changes to the unit of work
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}

