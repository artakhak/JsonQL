===================
Mutating JSON Files 
===================

.. contents::
   :local:
   :depth: 2

.. note::
   All JSON files used in examples in this section can be found here :doc:`./SampleFiles/index`  
   
- JsonQL expressions are used in one or many JSON files. JsonQL loads the JSON files into an instance of `JsonQL.Compilation.ICompilationResult <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/Compilation/ICompilationResult.cs#L8>`_.
- The property **CompiledJsonFiles** contains collection of `JsonQL.Compilation.ICompiledJsonData <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/Compilation/ICompiledJsonData.cs#L11>`_: one per loaded file. 
- `JsonQL.Compilation.ICompiledJsonData <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/Compilation/ICompiledJsonData.cs#L11>`_ represents mutated JSON files (i.e., mutated by using JsonQL expressions).  
- The property **CompilationErrors** contains collection of `JsonQL.Compilation.ICompilationErrorItem <https://github.com/artakhak/JsonQL/blob/f4341606f1a14f355c13eb35c717bba55e8c76e3/JsonQL/Compilation/ICompilationErrorItem.cs#L13>`_ with error details if any. 
- If many JSON files are specified the following rules and techniques are used:
  - Parent/child relationships between JSON files are maintained, and parent JSON files are evaluated before child JSON files are evaluated.
  - Lookup of JSON values specified in JsonQL expressions starts in JSON containing the expression first, and then in parent JSON files.


  
As an example lets consider the following JSON files.

- :doc:`./SampleFiles/companies` - a JSON file for data on number of companies
- :doc:`./SampleFiles/countries` - a JSON file for data on number of countries
- :doc:`./SampleFiles/Example1/parameters` - a JSON file with lists "FilteredCountryNames" and "FilteredCompanyNames" referenced in other JSON files below in JsonQL expressions.
- :doc:`./SampleFiles/Example1/filtered-companies` - a JSON file with JsonQL expressions that filter companies in :doc:`./SampleFiles/companies` to include only some companies using data in :doc:`./SampleFiles//Example1/parameters`.

    .. sourcecode:: json

         {
           "AdditionalCompanyNames": [ "Atlantic Transfers, Inc" ],

           "comments1": "'FilteredCompanyNames' is in parent JSON 'Parameters.json'.",
           "comments2": "We filter companies that are either in FilteredCompanyNames or in AdditionalCompanyNames in this file.",
           "FilteredCompanies": "$value(Companies.Where(c => Any(FilteredCompanyNames, x => x == c.CompanyData.Name) || Any(AdditionalCompanyNames, x => x == c.CompanyData.Name)))"
         }



  
.. toctree::

   SampleFiles/index.rst
   ErrorDetails/index.rst
   ReusingCompiledJsonFiles/index.rst