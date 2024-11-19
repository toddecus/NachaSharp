# NachaSharp

This is a C# library for writing out Nacha files. Maybe someday I will include reading in... doubt it. In reality, I am learning C# AND testing out GitHub + GitHub Copilot. Buyer Beware!

The detailed documentation file for the format I found: [ChoETL.NACHA](https://github.com/Cinchoo/ChoETL.NACHA). I don't know anything about the ETL tool and my needs were far less than a whole ETL thing....

## Installation

To install NachaSharp, clone the repository and build the project:

```sh
git clone https://github.com/toddecus/NachaSharp.git
cd NachaSharp
dotnet build


Usage
Here is an example of how to use NachaSharp to generate a Nacha file:
dotnet test
dotnet run

Usage in Code:
var nachaFile = new NachaFile();
nachaFileGenerator

```
Features
Generate Nacha files
Supports file headers, batch headers, and entry details and Addendums, along with BatchControl and File Control and finally padding for 10 block
Batch has a collection of EntryDetailRecords that may or may not have Addendums to them (2 lines)
A file can contain multiple batches 

Requirements
.NET 8.0 or later
Contributing
Contributions are welcome! Please fork the repository and submit a pull request.

License
This project is licensed under the MIT License. See the LICENSE file for details.

Contact
For questions or feedback, please contact Todd Ray Ellermann at toddecus+nachasharp@gmail.com.