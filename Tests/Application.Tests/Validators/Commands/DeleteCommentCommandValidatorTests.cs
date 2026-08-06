using Application.Commands.Posts.DeleteComment;
using Application.Validators.Commands;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Commands
{
    public class DeleteCommentCommandValidatorTests
    {
        private readonly DeleteCommentCommandValidator _validator = new();

        [Fact]
        public void Validate_WithPositiveCommentId_HasNoErrors()
        {
            var result = _validator.TestValidate(new DeleteCommentCommand(1));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Validate_WithNonPositiveCommentId_HasError(int commentId)
        {
            var result = _validator.TestValidate(new DeleteCommentCommand(commentId));

            result.ShouldHaveValidationErrorFor(x => x.CommentId);
        }
    }
}
