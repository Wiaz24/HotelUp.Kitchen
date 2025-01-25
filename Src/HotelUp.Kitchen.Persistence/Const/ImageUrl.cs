using FluentValidation;
using HotelUp.Kitchen.Persistence.Const.Exceptions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HotelUp.Kitchen.Persistence.Const;

public record ImageUrl
{
    public string Value { get; private init; } = null!;
    private ImageUrl(){}
    public ImageUrl(string value)
    {
        var url = new ImageUrl { Value = value };
        var validator = new ImageUrlValidator();
        var result = validator.Validate(url);
        if (!result.IsValid)
        {
            var failure = result.Errors.FirstOrDefault();
            throw new InvalidImageUrlException(failure!.ErrorMessage);
        }
        Value = value;
    }
    
    private class ImageUrlValidator : AbstractValidator<ImageUrl>
    {
        public ImageUrlValidator()
        {
            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("URL can't be empty.")
                .Matches(@"^https?://([\w-]+\.)+[\w-]+(/[\w\- ./?%&=]*)?$")
                .WithMessage("URL must be valid http or https address.")
                .Matches(@"\.(png|jpg|jpeg)$")
                .WithMessage("URL must be a valid image format (.png, .jpg, .jpeg).");
        }
    }
    public static implicit operator string(ImageUrl imageUrl) => imageUrl.Value;
    public static implicit operator ImageUrl(string value) => new(value);
    public static ValueConverter GetValueConverter()
    {
        return new ValueConverter<ImageUrl, string>(
            vo => vo.Value,
            value => new ImageUrl(value));
    }
}