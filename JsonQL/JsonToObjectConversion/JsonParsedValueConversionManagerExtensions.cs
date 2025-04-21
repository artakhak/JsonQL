using JsonQL.JsonObjects;
using System.Collections.Generic;

namespace JsonQL.JsonToObjectConversion;

public static class JsonParsedValueConversionManagerExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsonParsedValueConversionManager">Extended class instance.</param>
    /// <param name="parsedValue"></param>
    /// <param name="convertedValueNullability">
    /// If the value is not null, specifies the nullability of returned value and might result in errors/warnings<br/>
    /// being reported if the returned value is null, or any item in returned collection items are null.<br/>
    /// 'Non-Nullable value not set' errors will be reported only if <see cref="ConversionErrorType.NonNullablePropertyNotSet"/> or<br/>
    /// <see cref="ConversionErrorType.NonNullableCollectionItemValueNotSet"/> are not configured with <see cref="ErrorReportingType.Ignore"/>.<br/>
    /// Note, this value only affects the returned value. Property values in returned objects (including collection item nullability)  are checked<br/>
    /// by using microsoft's nullability flag '?'.<br/>
    /// The following rules are used.<br/> 
    /// -If the returned value is not a collection, <paramref name="convertedValueNullability"/> should <br/>
    /// have a single value with <b>true</b> for nullable returned value and <b>false</b> otherwise.<br/>
    /// -If the returned type is a collection, such as <see cref="IReadOnlyList{T}"/>, then the first<br/>
    /// item specifies nullability of collection itself (<b>true</b> for nullable, <b>false</b> for non-nullable),<br/>
    /// the second value specifies the nullability of first level items, the third item<br/>
    /// specifies nullability of second level items (such as items in lists of lists), etc.<br/>
    /// For example if returned type is <b>IEnumerable&lt;IReadOnlyList&lt;IEmployee&gt;&gt; Employees {get; set}</b>,<br/>
    /// then [true, false, true] will result in implementation assuming that the returned value can be nullable,<br/>
    /// lists <b>IReadOnlyList&lt;IEmployee&gt;</b> in returned value are not nullable, and <b>IEmployee</b> items in <b>IReadOnlyList&lt;IEmployee&gt;</b> are nullable.
    /// </param> 
    /// <param name="jsonConversionSettingOverrides"></param>
    /// <returns></returns>
    public static IConversionResult<T> Convert<T>(this IJsonParsedValueConversionManager jsonParsedValueConversionManager,
        IParsedValue parsedValue, IReadOnlyList<bool>? convertedValueNullability,
        IJsonConversionSettingsOverrides? jsonConversionSettingOverrides = null)
    {
        var convertedValue = jsonParsedValueConversionManager.Convert(parsedValue, typeof(T), convertedValueNullability, jsonConversionSettingOverrides);
        
        if (convertedValue.Value is not T instanceOfT)
            return new ConversionResult<T>(convertedValue.ConversionErrorsAndWarnings);
       
        return new ConversionResult<T>(instanceOfT, convertedValue.ConversionErrorsAndWarnings);
    }
}