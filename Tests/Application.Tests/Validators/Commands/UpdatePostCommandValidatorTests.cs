using Application.Commands.Posts.UpdatePost;
using Application.Dto.Post;
using Application.Validators.Commands;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Commands
{
    public class UpdatePostCommandValidatorTests
    {
        private readonly UpdatePostCommandValidator _validator = new();

        [Fact]
        public void Validate_WithValidUpdate_HasNoErrors()
        {
            var result = _validator.TestValidate(new UpdatePostCommand(1, new UpdatePostDto("hi")));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenPostIdIsNotPositive_HasError()
        {
            var result = _validator.TestValidate(new UpdatePostCommand(0, new UpdatePostDto("hi")));

            result.ShouldHaveValidationErrorFor(x => x.PostId);
        }

        [Fact]
        public void Validate_WhenDescriptionIsNull_HasNoError()
        {
            var result = _validator.TestValidate(new UpdatePostCommand(1, new UpdatePostDto(null)));

            result.ShouldNotHaveValidationErrorFor(x => x.UpdatePostDto.Description);
        }

        [Fact]
        public void Validate_WhenDescriptionExceedsMaxLength_HasError()
        {
            var result = _validator.TestValidate(new UpdatePostCommand(1, new UpdatePostDto(new string('a', 2001))));

            result.ShouldHaveValidationErrorFor(x => x.UpdatePostDto.Description);
        }
    }
}
