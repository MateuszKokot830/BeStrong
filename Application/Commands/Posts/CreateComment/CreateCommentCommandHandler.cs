using Application.Dto.Comment;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappings;
using Domain.Factories;
using ErrorOr;
using MediatR;

namespace Application.Commands.Posts.CreateComment
{
    public class CreateCommentCommandHandler(
        ICommentRepository commentRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateCommentCommand, ErrorOr<CommentDto>>
    {
        private readonly ICommentRepository _commentRepository = commentRepository;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<CommentDto>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var dto = request.CommentCreateDto;
            var comment = CommentFactory.Create(_currentUserService.UserId, dto.Description, dto.PostId);
            await _commentRepository.AddAsync(comment, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return comment.ToDto();
        }
    }
}
