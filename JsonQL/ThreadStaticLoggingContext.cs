// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using OROptimizer;
using OROptimizer.Diagnostics.Log;

namespace JsonQL;

internal class ThreadStaticLoggingContext : ThreadStaticAmbientContext<ILog, LogToConsole>
{

}