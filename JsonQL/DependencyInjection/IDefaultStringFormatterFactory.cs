// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.Compilation.JsonValueTextGenerator.StringFormatters;

namespace JsonQL.DependencyInjection;

/// <summary>
/// A default factory for <see cref="IStringFormatter"/> that uses a pre-selected list of formatters for different types (string, boolean, etc.).
/// Should be used in DI setup to configure DI setup for <see cref="IStringFormatter"/>.
/// Using this class does not allow customization. If customization is needed, do not use this class, and instead
/// use a custom implementation of <see cref="IStringFormatter"/> (such as <see cref="AggregatedStringFormatter"/>).
/// </summary>
public interface IDefaultStringFormatterFactory
{
    /// Creates an instance of IStringFormatter.
    /// <returns>An instance of IStringFormatter.</returns>
    IStringFormatter Create();
}

/// <inheritdoc />
public class DefaultStringFormatterFactory: IDefaultStringFormatterFactory
{
    private readonly IDateTimeOperations _dateTimeOperations;

    public DefaultStringFormatterFactory(IDateTimeOperations dateTimeOperations)
    {
        _dateTimeOperations = dateTimeOperations;
    }

    /// <inheritdoc />
    public IStringFormatter Create()
    {
        return new AggregatedStringFormatter(new List<IStringFormatter>
        {
            new DateTimeToStringFormatter(_dateTimeOperations),
            new BooleanToStringFormatter(),
            new DoubleToStringFormatter(),
            new ObjectToStringFormatter()
        });
    }
}