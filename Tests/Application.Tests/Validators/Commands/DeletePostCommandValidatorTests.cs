using Application.Commands.Posts.DeletePost;
using Application.Validators.Commands;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Commands
{
    public class DeletePostCommandValidatorTests
    {
        private readonly DeletePostCommandValidator _validator = new();

        [Fact]
        public void Validate_WithPositivePostId_HasNoErrors()
        {
            var result = _validator.TestValidate(new DeletePostCommand(1));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Validate_WithNonPositivePostId_HasError(int postId)
        {
            var result = _validator.TestValidate(new DeletePostCommand(postId));

            result.ShouldHaveValidationErrorFor(x => x.PostId);
        }
    }
}
