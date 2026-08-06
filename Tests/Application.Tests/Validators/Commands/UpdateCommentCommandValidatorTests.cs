using Application.Commands.Posts.UpdateComment;
using Application.Dto.Comment;
using Application.Validators.Commands;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Commands
{
    public class UpdateCommentCommandValidatorTests
    {
        private readonly UpdateCommentCommandValidator _validator = new();

        [Fact]
        public void Validate_WithValidUpdate_HasNoErrors()
        {
            var result = _validator.TestValidate(new UpdateCommentCommand(1, new UpdateCommentDto("edited")));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenCommentIdIsNotPositive_HasError()
        {
            var result = _validator.TestValidate(new UpdateCommentCommand(0, new UpdateCommentDto("edited")));

            result.ShouldHaveValidationErrorFor(x => x.CommentId);
        }

        [Fact]
        public void Validate_WhenDescriptionIsEmpty_HasError()
        {
            var result = _validator.TestValidate(new UpdateCommentCommand(1, new UpdateCommentDto("")));

            result.ShouldHaveValidationErrorFor(x => x.UpdateCommentDto.Description);
        }

        [Fact]
        public void Validate_WhenDescriptionExceedsMaxLength_HasError()
        {
            var result = _validator.TestValidate(new UpdateCommentCommand(1, new UpdateCommentDto(new string('a', 501))));

            result.ShouldHaveValidationErrorFor(x => x.UpdateCommentDto.Description);
        }
    }
}
