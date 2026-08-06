using Application.Commands.Posts.CreateComment;
using Application.Dto.Comment;
using Application.Validators.Commands;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Commands
{
    public class CreateCommentCommandValidatorTests
    {
        private readonly CreateCommentCommandValidator _validator = new();

        [Fact]
        public void Validate_WithValidComment_HasNoErrors()
        {
            var result = _validator.TestValidate(new CreateCommentCommand(new CommentCreateDto("nice!", PostId: 1)));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenPostIdIsNotPositive_HasError()
        {
            var result = _validator.TestValidate(new CreateCommentCommand(new CommentCreateDto("nice!", PostId: 0)));

            result.ShouldHaveValidationErrorFor(x => x.CommentCreateDto.PostId);
        }

        [Fact]
        public void Validate_WhenDescriptionIsEmpty_HasError()
        {
            var result = _validator.TestValidate(new CreateCommentCommand(new CommentCreateDto("", PostId: 1)));

            result.ShouldHaveValidationErrorFor(x => x.CommentCreateDto.Description);
        }

        [Fact]
        public void Validate_WhenDescriptionExceedsMaxLength_HasError()
        {
            var dto = new CommentCreateDto(new string('a', 501), PostId: 1);

            var result = _validator.TestValidate(new CreateCommentCommand(dto));

            result.ShouldHaveValidationErrorFor(x => x.CommentCreateDto.Description);
        }
    }
}
