using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Telerik.Reporting.Schema;

bool validArguments = args.Length == 1 && args[0] == "types"
	|| args.Length == 2 && args[0] == "schema"
	|| args.Length == 3 && args[0] == "validate";

if (!validArguments)
{
	Console.Error.WriteLine("Usage: types | schema <type> | validate <type> <json-file>");
	return 2;
}

try
{
	var knownTypes = ReportSchemaService.GetKnownTypeNames().ToArray();

	if (args[0] == "types")
	{
		Console.WriteLine(JsonSerializer.Serialize(knownTypes));
		return 0;
	}

	var matchingTypes = knownTypes.Where(name =>
		string.Equals(name, args[1], StringComparison.OrdinalIgnoreCase)
		|| string.Equals(name.Split('.').Last(), args[1],
			StringComparison.OrdinalIgnoreCase)).ToArray();

	if (matchingTypes.Length != 1)
	{
		Console.Error.WriteLine("Unknown or ambiguous type. Use types for supported names.");
		return 2;
	}

	string typeName = matchingTypes[0];

	if (args[0] == "schema")
	{
		Console.WriteLine(ReportSchemaService.GetSchema(typeName));
		return 0;
	}

	string definitionJson = File.ReadAllText(args[2]);
	string resultJson = ReportSchemaService.ValidateDeep(typeName, definitionJson);
	using var result = JsonDocument.Parse(resultJson);
	var root = result.RootElement;

	if (root.ValueKind != JsonValueKind.Object
		|| !root.TryGetProperty("isValid", out var isValid)
		|| (isValid.ValueKind != JsonValueKind.True
			&& isValid.ValueKind != JsonValueKind.False)
		|| !root.TryGetProperty("errors", out var errors)
		|| errors.ValueKind != JsonValueKind.Array)
	{
		Console.Error.WriteLine("The SDK returned an unexpected validation result.");
		return 2;
	}

	Console.WriteLine(resultJson);
	return isValid.GetBoolean() ? 0 : 1;
}
catch (Exception ex)
{
	// Do not expose report content or configuration in error messages.
	Console.Error.WriteLine($"Helper failed ({ex.GetType().Name}). Check the local "
		+ "license, package version, and file access.");
	return 2;
}