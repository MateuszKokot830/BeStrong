using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Domain.Entities;

namespace Application.Commands.Posts.CreateComment
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public CreateCommentCommandHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = _mapper.Map<Comment>(request.CommentCreateDto);
            await _postRepository.CreateCommentAsync(comment);

            return Unit.Value;
        }
    }
}