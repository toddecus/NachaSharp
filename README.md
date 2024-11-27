# NachaSharp

This is a C# library for writing out Nacha files. Maybe someday I will include reading in... doubt it. In reality, I am learning C# AND testing out GitHub + GitHub Copilot. Buyer Beware!

The detailed documentation file for the format I found: Old National Bank https://www.oldnational.com/ 

## Installation

To install NachaSharp, clone the repository and build the project:

```sh
git clone https://github.com/toddecus/NachaSharp.git
cd NachaSharp
dotnet build
```

Usage
Here is an example of how to use NachaSharp to generate a Nacha file:
```sh
dotnet test
dotnet run 
```
THis will run the corresponding Main Method to call PopulateTestFile and then write out the resulting Nacha test file. This is strictly a test harness.

Usage in Your Code:
```csharp
string YourBankName = "Your Bank Name"; //23 chars max
string YourBankRoutingNumber = "999999992"; // DFI="99999999" CheckDigit="2" 9 digits
string YourCompanyName = "My Company";// 23 chars max
string BankAssignedYourCompanyACHID = "999999999"; //max 10 chars relates to your bank account number
string YourSystemReference= "1234ABCD";// 8 chars for your to reference this file in your system
var nachaFile = new NachaFile(
                new FileHeaderRecord(YourBankName,
                new ACHRoutingNumber(YourBankRoutingNumber),
                YourCompanyName,
                BankAssignedYourCompanyACHID,
                YourSystemReference),
                logger); 
```
Now go see NachaFile.PopulateTestData() << Probably should be looping through batches and adding entryDetail records. Suggest keeping track of your own credit and debit sums to validate before you dump file as a sanity check... that and entry/addendum counts 
```csharp
// at the end of each Batch please call:
batch.CalculateControlRecord();

//When done with all batches, call:
nachaFile.CalculateFileControl()

//Then write out the file to whatever file mechanism you are using. S3? 
System.out.write(nachaFile.ToStringValue());
// or to write out a local file in /outputFile/nacha.txt
nachaFile.generateNachaFile(fullPath)
```

Features
Generate Nacha files
Supports file headers, batch headers, and entry details and Addendums, along with BatchControl and File Control and finally padding for 10 block
Batch has a collection of EntryDetailRecords that may or may not have Addendums to them (2 record lines one entryDetail then one Addendum)
A file can contain multiple batches 

Requirements
.NET 8.0 or later
Contributing
Contributions are welcome! Please fork the repository and submit a pull request.

License
This project is licensed under the MIT License. See the LICENSE file for details.

Contact
For questions or feedback, please contact Todd Ellermann at toddecus+nachasharp@gmail.com.