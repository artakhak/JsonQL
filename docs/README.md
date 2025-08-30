# JsonQL Documentation

This is a Sphinx project for JsonQL documentation.
* The NuGet package JsonQL can be downloaded at [https://www.nuget.org/packages/JsonQL/](https://www.nuget.org/packages/JsonQL/).
* The source code is at [https://github.com/artakhak/JsonQL](https://github.com/artakhak/JsonQL)

## Sphinx/ReadTheDocs documentation setup instructions

### Install python from https://www.python.org/download/releases/2.5/msi/.
    -After the installation completes, click on "Disable path length limit" button on last page.
	-Add the python installation folder (the one that contains python.exe), and the Scripts folder under installation folder to Path environment variable (if it is not added by installer)
    
    Execute the following commands in windows command
    python -m pip install -U pip #this step most probably will not be needed since pip should be in python installation)
    python -m pip install --upgrade pip
  
### Install Sphinx  (see http://www.sphinx-doc.org/en/master/).
    -Open regular command window and run the following commands
     import pip #this might not be required)
     python -m pip install -U sphinx
     python -m pip install sphinx sphinx-autobuild

### Install sphinx-rtd-theme Theme
    cd "k:\..\JsonQL\docs" 
    pip install sphinx-rtd-theme

### Create the documentation project with sphinx
    -One time only: in command window run the following commands:
    
    cd "k:\..\JsonQL\docs" 
    sphinx-quickstart #this will build the Sphinx doc project. 
        Here are non-default responses:
            Separate source and build directories: Y.
            Project name: JsonQL
            Do you want to use the epub builder: N
### Building the docs

      - Run the application JsonQL.DocumentationGenerator to re-generate md and rst files from templates (e.g., the main README.rd, docs/index.rst files)
      - In k:\..\JsonQL\docs\conf.py set the value of html_theme to 'sphinx_rtd_theme'
      -In command window change to "k:\..\JsonQL\docs" directory (see the section above), and
        run the following command:
        make html
       
       -To reload the docs, run 
        sphinx-autobuild . _build/html

### Running local python server to view/test rst files
      - Run the following commands
        cd _build/html
        python -m http.server

      - Open this page: http://localhost:8000/
        
### Publishing to Git
      -Before committing to git run "make clean" to clean the generated files. ReadTheDcos will build the docs.
