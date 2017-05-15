#TextualDB


##Components.TextualDatabase
            
Represents a TextualDB Database, containing subordinate tables
        
###Properties

####FilePath
The path on disc of the database
####Tables
The tables contained within the database
###Methods


####FromFile(System.String)
Deserializes a TextualDatabase from the given path
> #####Parameters
> **filePath:** The path on disc of the database

> #####Return value
> The resulting TextualDatabase

####Constructor
Constructs a new TextualDatabase from the given path
> #####Parameters
> **filePath:** The path on disc of the database


####Constructor
Constructs a new TextualDatabase from the given path with the given collection of tables
> #####Parameters
> **filePath:** The path on disc of the database

> **tables:** The collection of tables for the database to contain


####AddTable(TextualDB.Components.TextualTable)
Adds an existing table to the database
> #####Parameters
> **table:** The table to be added

> #####Return value
> The current instance of TextualDatabase

####AddTable(System.String)
Adds a new table to the database with the given name
> #####Parameters
> **name:** The name of the new table

> #####Return value
> The current instance of TextualDatabase

####AddTable(System.String,System.Collections.Generic.IEnumerable{System.String})
Adds a new table to the database with the given name and enumerable collection of column strings
> #####Parameters
> **name:** The name of the new table

> **columns:** The string collection of column names for the new table

> #####Return value
> The current instance of TextualDatabase

####AddTable(System.String,System.Collections.Generic.IEnumerable{System.String},System.Collections.Generic.List{TextualDB.Components.TextualRow})
Adds a new table to the database with the given name, enumerable collection of column string, and list of rows
> #####Parameters
> **name:** The name of the new table

> **columns:** The string collection of column names for the new table

> **rows:** The list of rows for the new table

> #####Return value
> The current instance of TextualDatabase

####ContainsTable(System.String)
Returns true if the database contains a table with the given name
> #####Parameters
> **name:** The table name to check

> #####Return value
> True if the database contains the given table, otherwise; false

####GetTable(System.String)
Returns the TextualTable with the given name from the database if said table exists, otherwise throws TableNotFoundException
> #####Parameters
> **name:** The name of the table to return

> #####Return value
> The table of the given name

####RemoveTable(System.String)
Removes the table with the given name from the database if said table exists, otherwise throws TableNotFoundException
> #####Parameters
> **name:** The name of the table to remove


##Components.TextualTable
            
Represents a TextualDB Table, defined by columns and containing subordinate rows
        
###Properties

####Name
The name of the table
####Columns
The columns of the table
####ColumnLength
The length of the list of columns expressed visually
####Rows
The rows of the table
###Methods


####Constructor
Constructs a new TextualDatabase with the given name and enumerable collection of string column names
> #####Parameters
> **name:** The name of the new table

> **columns:** The enumerable collection of string column names for the new table


####Constructor
Constructs a new TextualDatabase with the given name, enumerable collection of string column names, and list of rows
> #####Parameters
> **name:** The name of the new table

> **columns:** The enumerable collection of string column names for the new table

> **rows:** The list of rows for the new table


####AddColumn(System.String,System.Int32)
Adds a new column to the table with the given name at the optional index, default to the end
> #####Parameters
> **name:** The name of the new column

> **pos:** The optional index parameter for the position of the new column

> #####Return value
> The current instance of TextualTable

####AddRow(TextualDB.Components.TextualRow,System.Int32)
Adds an existing row to the table at the optional index, default to the end
> #####Parameters
> **row:** The row to be added

> **pos:** The optional index parameter for the position of the new row


####AddRow(System.Int32,System.String[])
Adds a new row to the table at the optional index, with a params array of string values
> #####Parameters
> **pos:** The optional index parameter for the position of the new row

> **values:** The params array of string values for the row


####ChangeColumnName(System.String,System.String)
Changes the name of a column within the table
> #####Parameters
> **oldName:** The name of the column to be changed

> **newName:** The new name for the column to be changed to

> #####Return value
> The current instance of TextualTable

####ChangeTableName(System.String)
Changes the name of the table
> #####Parameters
> **newName:** The new name of the table

> #####Return value
> The current instance of TextualTable

####ContainsColumn(System.String)
Returns true if the table contains a column with the given name
> #####Parameters
> **name:** The column name to check

> #####Return value
> True if the table contains the given column, otherwise; false

####GetRow(System.Int32)
Returns the row in the table at the given index if index exists, otherwise throws RowNotFoundException
> #####Parameters
> **pos:** The position in the table of the desired row

> #####Return value
> The row at the given position

####RemoveColumn(System.String)
Removes the column with the given name from the table if the column exists, otherwise throws ColumnNotFoundException
> #####Parameters
> **name:** The name of the table to remove


####RemoveRow(TextualDB.Components.TextualRow)
Removes the given row from the table if the row belongs to this table, otherwise throws RowNotFoundException
> #####Parameters
> **row:** The row to remove from the table


####Select(System.String[])
Returns a new table based on the current table that only contains columns from the given params string array of column names. If none given, returns all columns
> #####Parameters
> **columns:** The params string array of columns for the new table, none given returns all

> #####Return value
> The resulting table

####ToString
Serializes the table and returns the string value
> #####Return value
> The string representation of the table

##Exceptions.ColumnExistsException
            
Exception to be thrown if an attempt is made to create a column that already exists in a given table
        
###Properties

####Message
The string message showing the column name and table name
####Column
The already existing column
####Table
The table where the column already exists
###Methods


####Constructor
Constructs a new ColumnExistsException with the given column and table
> #####Parameters
> **column:** The already existing column

> **table:** The table where the column already exists


##Exceptions.ColumnNotFoundException
            
Exception to be thrown if an attempt is made to access, remove, or rename a non-existent column in a given table
        
###Properties

####Message
The string message showing the column and table name
####Column
The non-existent column
####Table
The table the column does not exist in
###Methods


####Constructor
Constructs a new ColumnNotFoundException with the given column and table
> #####Parameters
> **column:** The non-existent column

> **table:** The table where the column does not exist


##Exceptions.CommandLineParseException
            
Exception to be thrown if there is a generic error with parsing a command
        
###Properties

####Message
The string error message
####SourceLocation
The location in the command string this error originates
###Methods


####Constructor
Constructs a new CommandLineParseException with the given source location, message format string, and format arguments
> #####Parameters
> **location:** The source location of the error

> **msgf:** The C# format message string to be passed into string.Format

> **args:** The format arguments to be passed into string.Format


##Exceptions.CommandLineInterpreterException
            
Exception to be thrown if there is a generic error with interpreting a command
        
###Properties

####Message
The string error message
####SourceLocation
The location in the command string this error originates
###Methods


####Constructor
Constructs a new CommandLineInterpreterException with the given source location, message format string, and format arguments
> #####Parameters
> **location:** The source location of the error

> **msgf:** The C# format message string to be passed into string.Format

> **args:** The format arguments to be passed into string.Format


##Exceptions.DeserializerException
            
Exception to be thrown if there is a generic error with deserializing a TextualDB database
        
###Properties

####Message
The string error message
####SourceLocation
The location in the database file this error originates
###Methods


####Constructor
Constructs a new DeserializerException with the given source location, message format string, and format arguments
> #####Parameters
> **location:** The source location of the error

> **msgf:** The C# format message string to be passed into string.Format

> **args:** The format arguments to be passed into string.Format


##Exceptions.RowNotFoundException
            
Exception to be thrown if an attempt is made to access, modify, or remove a non-existent row in a given table
        
###Properties

####Message
The string message showing the table name and optionally the row index
####Row
The non-existent row
####Position
The non-existent row index
####Table
The table the row does not exist in
###Methods


####Constructor
Constructs a new RowNotFoundException with the given table and row
> #####Parameters
> **table:** The table where the row does not exist

> **row:** The non-existent row


####Constructor
Constructs a new RowNotFoundException with the given table and row index
> #####Parameters
> **table:** The table where the position does not exist

> **position:** The non-existent position


##Exceptions.TableExistsException
            
Exception to be thrown if an attempt is made to create a table that already exists in a given database
        
###Properties

####Message
The string message showing the table name and database file path
####Table
The already existing table
####Database
The database where the table already exists
###Methods


####Constructor
Constructs a new TableExistsException with the given table and database
> #####Parameters
> **table:** The already existing table

> **database:** The database where the table already exists


##Exceptions.TableNotFoundException
            
Exception thrown if an attempt is made to access, remove, or rename a non-existent table in a given database
        
###Properties

####Message
The string message showing the table name and database file path
####TableName
The non-existent table
####Database
The database where the table does not exist
###Methods


####Constructor
Constructs a new TableNotFoundException with the given database and table
> #####Parameters
> **database:** The database where the table does not exist

> **table:** The non-existent table


##TextualCommand
            
Represents a query or operation to be executed within a TextualDB Database
        
###Properties

####CommandString
The string value of the query or operation
####Database
The database for the command to be executed inside
####Placeholders
A collection of prepared statements, where keys are {0}, {1}, {2} and so forth
###Methods


####Constructor
Constructs a new TextualCommand with the given database and command string
> #####Parameters
> **database:** The database for the command to be executed under

> **commandString:** The command string


####Constructor
Constructs a new TextualCommand with the given database path and command string
> #####Parameters
> **databasePath:** The path to the database for the command to be executed under

> **commandString:** The command string


####Constructor
Constructs a new TextualCommand with the given database, command string, and collection of placeholders
> #####Parameters
> **database:** The database for the command to be executed under

> **commandString:** The command string

> **placeholders:** The collection of placeholders, where keys are {0}, {1}, {2} and so forth


####Constructor
Constructs a new TextualCommand with the given database path and command string
> #####Parameters
> **databasePath:** The path to the database for the command to be executed under

> **commandString:** The command string

> **placeholders:** The collection of placeholders, where keys are {0}, {1}, {2} and so forth


####AddPlaceholder(System.Int32,System.String)
Adds a new placeholder with the given key and value
> #####Parameters
> **place:** The index for the placeholder

> **value:** The value of the placeholder

> #####Return value
> The current instance of TextualCommand

####Execute
Executes the query or operation within the database and returns a TextualDB table as a result
> #####Return value
> The resulting TextualDB table