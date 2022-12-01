using MediatR;
using Microsoft.EntityFrameworkCore;
using GuessingGame.Infrastructure.Data;

namespace GuessingGame.Core.Domain.Oracle.Pipelines;

public class VerifyGuess
{
    public record Request(Guid UserGuid, string Guess) : IRequest<Boolean>;

    public class Handler : IRequestHandler<Request, Boolean>
    {
        private readonly GuessingGameDbContext _db;

        public Handler(GuessingGameDbContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

        //Compares the submitted guess to the correct one
        public async Task<Boolean> Handle(Request request, CancellationToken cancellationToken)
        {
            var state = await _db.GameStates.FirstOrDefaultAsync(gamestate => gamestate.UserGuid == request.UserGuid);
            if (state is null)
            {
                throw new Exception("GameState was null when VerifyGuess!");
            }
            return request.Guess.ToLower().Trim() == state.CorrectGuess.ToLower().Trim();
        }
    }
}
