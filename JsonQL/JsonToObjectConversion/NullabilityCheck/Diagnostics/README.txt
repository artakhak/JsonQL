The implementation JsonQL.Extensions.JsonToObjectConversion.NullabilityCheck.NullableTypeHelpers relies on internal Microsoft attributes
that are not documented and can change at any point.
The classes in this library test if the implementation works and if something changes an error will be reported so that the implementation of 
JsonQL.Extensions.JsonToObjectConversion.NullabilityCheck.INullableTypeHelpers is replaced with something else (say to use other custom attributes similar
to the ones in JetBrains.Annotations, or JsonQL.Extensions.JsonToObjectConversion.NullabilityCheck.NullableTypeHelpersi is modified to adjust for the changes done in Microsoft 
libraries.
