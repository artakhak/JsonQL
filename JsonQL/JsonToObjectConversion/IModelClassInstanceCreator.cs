using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.JsonToObjectConversion;

public interface IModelClassInstanceCreator
{
    bool TryCreate(Type createdInstanceType, IReadOnlyList<IModelClassCreationPropertyData> propertyValues, [NotNullWhen(true)] out object? createdInstance, 
        [NotNullWhen(false)] out string? errorMessage);
}

/// <inheritdoc />
public class ModelClassInstanceCreator : IModelClassInstanceCreator
{
    /// <inheritdoc />
    public bool TryCreate(Type createdInstanceType, IReadOnlyList<IModelClassCreationPropertyData> propertyValues, 
        [NotNullWhen(true)] out object? createdInstance, [NotNullWhen(false)] out string? errorMessage)
    {
        createdInstance = null;
        errorMessage = null;

        if (!createdInstanceType.IsClass || createdInstanceType.IsAbstract)
        {
            errorMessage = $"Type [{createdInstanceType}] is abstract or is not a reference type.";
            return false;
        }

        var publicConstructors = createdInstanceType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).ToList();
        
        if (publicConstructors.Count == 0)
        {
            errorMessage = $"Type [{createdInstanceType}] has no public constructors.";
            return false;
        }

        if (publicConstructors.Count > 1)
        {
            errorMessage = $"Type [{createdInstanceType}] has more than one public constructors.";
            return false;
        }

        var constructorInfo = publicConstructors[0];
       
        var constructorParametersMetadata = constructorInfo.GetParameters();
        if (constructorParametersMetadata.Length > 0)
        {
            var constructorParametersValues = new object?[constructorParametersMetadata.Length];

            var constructorParameterNameToPropertyData = new Dictionary<string, IModelClassCreationPropertyData>(StringComparer.OrdinalIgnoreCase);
            
            foreach (var propertyValue in propertyValues)
            {
                var constructorParameterName = propertyValue.PropertyInfo.Name.Length == 1 ? propertyValue.PropertyInfo.Name.ToLower() :
                    string.Concat(Char.ToLower(propertyValue.PropertyInfo.Name[0]), propertyValue.PropertyInfo.Name[1..]);
                constructorParameterNameToPropertyData[constructorParameterName] = propertyValue;
            }

            for (var paramIndex = 0; paramIndex < constructorParametersMetadata.Length; ++paramIndex)
            {
                var parameterInfo = constructorParametersMetadata[paramIndex];

                object? parameterValue = null;
                if (parameterInfo.Name != null && constructorParameterNameToPropertyData.TryGetValue(parameterInfo.Name, out var propertyValue))
                {
                    if (!parameterInfo.ParameterType.IsAssignableFrom(propertyValue.PropertyInfo.PropertyType))
                    {
                        errorMessage = $"Constructor parameter [{parameterInfo.Name}] of type [{parameterInfo.ParameterType.FullName}] is not compatible with type [{propertyValue.PropertyInfo.PropertyType.FullName}] of property [{propertyValue.PropertyInfo.Name}].";
                        return false;
                    }

                    parameterValue = propertyValue.PropertyValue;
                }

                constructorParametersValues[paramIndex] = parameterValue;
            }

            try
            {
                var createdInstanceLocal = constructorInfo.Invoke(constructorParametersValues);
                createdInstance = createdInstanceLocal;
            }
            catch (Exception e)
            {
                errorMessage = $"Failed to create an instance of type [{createdInstanceType}] with given parameters. Error message: {e.Message}";
                LogHelper.Context.Log.Error(errorMessage, e);
                return false;
            }
        }
        else
        {
            createdInstance = constructorInfo.Invoke(Array.Empty<object?>());
        }

        foreach (var propertyValue in propertyValues)
        {
            if (propertyValue.PropertyValue == null)
                continue;

            if (!propertyValue.PropertyInfo.CanWrite)
                continue;

            var setter = propertyValue.PropertyInfo.GetSetMethod();

            if (!(setter?.IsPublic??false))
                continue;

            try
            {
                propertyValue.PropertyInfo.SetValue(createdInstance, propertyValue.PropertyValue);
            }
            catch (Exception e)
            {
                LogHelper.Context.Log.Error($"Failed to set the value of property [{propertyValue.PropertyInfo.Name}] in [{createdInstance.GetType()}]", e);
                continue;
            }
        }

        return true;
    }
}