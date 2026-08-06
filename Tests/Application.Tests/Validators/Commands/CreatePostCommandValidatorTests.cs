using Application.Commands.Posts.CreatePost;
using Application.Dto.Post;
using Application.Validators.Commands;
using Domain.Common;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators.Commands
{
    public class CreatePostCommandValidatorTests
    {
        private readonly CreatePostCommandValidator _validator = new();

        [Fact]
        public void Validate_NormalPostWithDescription_HasNoErrors()
        {
            var dto = new PostCreateDto(PostType.Normal, "hello", null, null);

            var result = _validator.TestValidate(new CreatePostCommand(dto));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_NormalPostWithoutDescription_HasError()
        {
            var dto = new PostCreateDto(PostType.Normal, null, null, null);

            var result = _validator.TestValidate(new CreatePostCommand(dto));

            result.ShouldHaveValidationErrorFor(x => x.PostCreateDto.Description)
                .WithErrorMessage("A normal post must have a description.");
        }

        [Fact]
        public void Validate_WorkoutPublicationWithWorkoutId_HasNoErrors()
        {
            var dto = new PostCreateDto(PostType.WorkoutPublication, null, WorkoutId: 5, null);

            var result = _validator.TestValidate(new CreatePostCommand(dto));

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WorkoutPublicationWithoutWorkoutId_HasError()
        {
            var dto = new PostCreateDto(PostType.WorkoutPublication, null, WorkoutId: null, null);

            var result = _validator.TestValidate(new CreatePostCommand(dto));

            result.ShouldHaveValidationErrorFor(x => x.PostCreateDto.WorkoutId)
                .WithErrorMessage("A workout publication must reference a workout.");
        }

        [Fact]
        public void Validate_WorkoutPublicationWithoutDescription_HasNoError()
        {
            // Unlike a Normal post, a WorkoutPublication doesn't require a description.
            var dto = new PostCreateDto(PostType.WorkoutPublication, null, WorkoutId: 5, null);

            var result = _validator.TestValidate(new CreatePostCommand(dto));

            result.ShouldNotHaveValidationErrorFor(x => x.PostCreateDto.Description);
        }

        [Fact]
        public void Validate_DescriptionExceedingMaxLength_HasError()
        {
            var dto = new PostCreateDto(PostType.Normal, new string('a', 2001), null, null);

            var result = _validator.TestValidate(new CreatePostCommand(dto));

            result.ShouldHaveValidationErrorFor(x => x.PostCreateDto.Description);
        }
    }
}
