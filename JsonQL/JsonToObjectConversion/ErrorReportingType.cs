// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Indicates that errors encountered during JSON to object conversion should be reported as warnings.
/// </summary>
public enum ErrorReportingType
{
    /// <summary>
    /// Represents an option in the ErrorReportingType enumeration where errors encountered during JSON to object
    /// conversion are ignored. Selecting this option will suppress error notifications, and the conversion process
    /// will attempt to proceed despite any issues encountered.
    /// </summary>
    Ignore,

    /// <summary>
    /// Represents an option in the ErrorReportingType enumeration where errors encountered during JSON to object
    /// conversion are reported as warnings. Selecting this option will log errors as warnings, allowing the
    /// conversion process to continue while notifying the user of potential issues.
    /// </summary>
    ReportAsWarning,

    /// <summary>
    /// Represents an option in the ErrorReportingType enumeration where errors encountered
    /// during JSON to object conversion are reported as errors. This ensures that all errors
    /// are explicitly surfaced and can halt the conversion process unless handled appropriately.
    /// </summary>
    ReportAsError
}