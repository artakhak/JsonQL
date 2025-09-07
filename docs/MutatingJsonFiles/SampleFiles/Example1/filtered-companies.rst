===============================
Example1/FilteredCompanies.json
===============================

.. contents::
   :local:
   :depth: 2

.. sourcecode:: json

     {
       "AdditionalCompanyNames": [ "Atlantic Transfers, Inc" ],

       "comments1": "'FilteredCompanyNames' is in parent JSON 'Parameters.json'.",
       "comments2": "We filter companies that are either in FilteredCompanyNames or in AdditionalCompanyNames in this file.",
       "FilteredCompanies": "$value(Companies.Where(c => Any(FilteredCompanyNames, x => x == c.CompanyData.Name) || Any(AdditionalCompanyNames, x => x == c.CompanyData.Name)))"
     }

