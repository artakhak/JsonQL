=====================
Example1/Example.json
=====================

.. contents::
   :local:
   :depth: 2

.. sourcecode:: json

     {
       "FilteredCountryNames": [ "United States", "Canada", "Australia" ],

       "FilteredCountryData": "$value(Countries.Where(c => Any(FilteredCountryNames.Where(x => x == c.Name))).Select(c => Concatenate('Name:', c.Name, ', Population:', c.Population)))",
         
       "comments": "'FilteredCompanies' array used in JsonQL expressions below is in parent JSON FilteredCompanies.json",
       "FilteredCompanyAddresses": {
         "comments": "'FilteredCompanies' array is in parent JSON FilteredCompanies.json",
         "addresses": "$value(FilteredCompanies.Where(c => c.CompanyData.CEO != 'John Malkowich').Select(x => x.Address))"
       },

       "FilteredCompanyEmployees": "$value(FilteredCompanies.Select(c => c.Employees.Where(e => e.Name !=  'John Smith')))",
       "FilteredCompanyEmployeeAddresses": "$value(FilteredCompanies.Select(c => c.Employees.Select(x => x.Address)))"
     }

