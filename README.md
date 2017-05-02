# TextualDB
Text Based Database in C#

## Premise

The main premise behind TextualDB is to have a database with similar controls and methodology to
a traditional SQL-esque program while maintaining an easily editable ASCII based format that can
be modified with any common text editor.

Each 'database' is a text file that can contain an unlimited amount of tables. A table can have
an unlimited amount of columns and each row will have a string that represents the value for
that column (default is an empty string).

TextualDB.exe provides a command line interface and .NET resource API for creating, modifying, and
data processing with TextualDB tables.

## Sample Table

A basic TextualDB table looks as follows.

```
people:
first | last | age | 
------------------------------
| "Jacob" | "Misirian" | "17" |
------------------------------
| "Sloan" | "Crandell" | "19" |
------------------------------
| "Marco" | "quinten" | "21" | 
-----------------------------
?
```

The table starts with the table name, "people", followed by a colon. The next line contains a pipe (|)
separated list of the column names. After this column list begins the rows. Each row starts and ends with
a pipe (|) and has strings split by pipe (|) characters. Each row has a line above and below of hyphen (-)
characters that extend to the end of the row.
