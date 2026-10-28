using MediatR;

namespace ProjectManager.Application.Contracts.Commands.Ledgers;

public class VoidWorkPackageLedgerCommand : IRequest<bool>
{
    public long Id { get; set; }
}