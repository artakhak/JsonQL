using JsonQL.JsonObjects.JsonPath;

namespace JsonQL.JsonToObjectConversion;

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