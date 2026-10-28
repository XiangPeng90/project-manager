using MediatR;

namespace ProjectManager.Application.Contracts.Commands.Ledgers;

public class VoidProjectLedgerCommand : IRequest<bool>
{
    public long Id { get; set; }
}