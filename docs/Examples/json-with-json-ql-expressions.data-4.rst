:orphan:

=============
Overview.json
=============

.. sourcecode:: json
  
  {
    "CountryNamesWithPopulationOf80MlnOrMore": "$value(Countries.Where(x => x.Population >= 80000000).Select(x => x.Name))",
    "CompanyDetailsWithAnyEmployeeWithNullAddress": "$value(Companies.Where(x => Any(x.Employees, x => x.Address is null)).Select(x => x.CompanyData))",
    "EmployeesWithNonEmptyLoginsList": "$value(Companies.Select(x => x.Employees.Where(x => Count(x.Logins) > 0)))",
    "FirstEmployeeWithLoginsField": "$value(Companies.Select(x => x.Employees.Where(x => HasField(x, 'Logins'))).First(x => x.Id != 100000005))",
    "LastEmployeeWithLoginsField": "$value(Companies.Select(x => x.Employees.Where(x => HasField(x, 'Logins'))).Last())",
    "FlattenExamples": {
      // Flattening arrays can be applied multiple times to execute sophisticated queries
      "FlattenArraysAndFilterObjects": "$value(MultiDimensionalMatrix.Flatten().Where(x => x > 3 || x.Name == 'John' || x.CompanyName starts with 'Sherwood'))",
      "GetAllNumericValuesInArraysFiltered": "$value(MultiDimensionalMatrix.Flatten().Flatten(x => typeof x != 'JsonArray' || All(x, y => y != 12)).Where(x => typeof x == 'Number'))"
    },
    // If CompanyData.CEO field is missing or value is null for second company, compilation will fail.
    "SelectSecondCompanyCeoNameWithAssertionThatCeoNameFieldIsPresetAndNotNull": "$(Companies[1].CompanyData.CEO assert)",

    // Example of merging fields into a JSON object. EnhancedCompanyData has fields in CompanyData for company with Name that starts with text 'Sherwood'
    // enhanced with additonal fields
    "EnhancedCompanyData": {
      "Capitalization": 1000000,
      "CompanyDataCopied": "$copyFields(Companies.First(x => x.CompanyData.Name starts with 'Sherwood').CompanyData)",
      "NumberOfEmployees": "$(Count(Companies.First(x => x.CompanyData.Name starts with 'Sherwood').Employees))"
    },

    // Example of merging array items into an array
    // List of company data for companies with  max salary of employees at least 110000 will be merged into array ListOfCompaniesEnhancedWithNewCompanies
    "ListOfCompaniesEnhancedWithNewCompanies": [
      {
        "Name": "Some company 1",
        "CEO": "Napoleon Bonaparte"
      },
      // Merge data of companies with max salary of employees at least 110000
      "$merge(Companies.Where(x => Max(x.Employees, y => true, y => y.Salary) > 110000).Select(x => x.CompanyData))",
      {
        "Name": "Some company 2",
        "CEO": "Georges Danton"
      }
    ],
    // Example of simple value using mutator function "$". In contrast to "$value" it generates a simple value (string, numeroc, etc) and not an object.
    "AverageSalaryOfAllEmployeesInFilteredCompanies": "$(Average(Companies.Where(x => !(x.CompanyData.Name starts with 'Sherwood')).Select(x => x.Employees.Select(x => x.Salary))))",
    "AddressingObjectsInMultiDimensionalMatrix": "$value(MultiDimensionalMatrix[1, 2].Address)",

    // Examples of using contextual data such a 'index' as well as example of referencing parent lamdba function parameter
    "EmployeeNamesInSecondCompanyExcludingFirstAndLastEmployee": "$value(Companies[1].Select(x => x.Employees.Where(y => index != 0 && index != Count(x.Employees) - 1).Select(x => x.Name)))",
    // We can use named parameters to change the order of parameters, as well as not specified optional parameter that are before parameters we pass
    // In this example both criteria and value are optional, and criteria is before value parameter. By using named parameter value, we can ommit criteria parameter
    "MaxRaisedSalaryAccrossAllCompaniesUsingNamedParameter": "$value(Max(Companies.Select(x => x.Employees).Flatten(), value->x=>x.Salary*1.05))",

    // isReverseSearch named parameter does not have to be used here, and we can use true here, since this parameter comes after the predicate. 
    // However, it makes the meaning clear.
    "ThirdEmployeeFromEndWithSalaryGreaterThan_100000_AcrossAllCompanies": "$value(Companies.Select(x => x.Employees).Flatten().At(2, x => x.Salary >= 100000 && x.Id != 100000008, isReverseSearch->true))",

    "UseOf_this_ToEnsureObjectIsLookedUpInCurrentFileAndNotInParentFiles": "$(this.CountryNamesWithPopulationOf80MlnOrMore[1])",
    "UseOf_parent_ToEnsureObjectIsLookedUpInParentFilesAndSearchInCurrentFilesIsSkipped": "$value(Count(parent.Companies, x => x.CompanyData.CEO != 'John Malkowich'))",
    "EmployeeWithNonNullIdAndNullAddress": "$value(Companies.Select(x => x.Employees).Flatten().Where(x => x.Address is null && x.Id is not null))",
    "NumberOfEmployeesWithout_Logins_Field": "$(Count(Companies.Select(x => x.Employees).Flatten().Where(x => x.Logins is undefined)))",
    "NumberOfEmployeesWith_Logins_Field": "$(Count(Companies.Select(x => x.Employees).Flatten().Where(x => x.Logins is not undefined)))",
    "InvalidValuesEvaluateToUndefined": "$(Companies[0].Employees[1000].Salary is undefined) = true",
    "ConversionExample_Datetime": "'2022-05-22T18:25:43.511Z' < '2022-05-23T18:25:43.511Z'=$(ToDateTime('2022-05-22T18:25:43.511Z') < ToDateTime('2022-05-23T18:25:43.511Z'))",
    "DefaultValue": "Defaulted salary to $(Companies[0].Employees[1000].Salary:100000)=100000"
  }

