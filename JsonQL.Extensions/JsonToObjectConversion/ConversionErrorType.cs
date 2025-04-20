using JsonQL.JsonObjects.JsonPath;

namespace JsonQL.Extensions.JsonToObjectConversion;

public enum ConversionErrorType
{
    Error,
    PropertyTypeDoesNotExist,
    InterfaceImplementationNotFound,
    CannotCreateInstanceOfClass,
    ValueNotSet,
    NonNullablePropertyNotSet,
    NonNullableCollectionItemValueNotSet,
    FailedToConvertJsonValueToExpectedType
}