using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EliteWealth.Framework.CSVTool;

namespace CsvTransform
{
	public partial class frmCsvTransform : Form
	{
		public frmCsvTransform()
		{
			InitializeComponent();
		}

		private void btnTransform_Click(object sender, EventArgs e)
		{
			OpenFileDialog fdlg = new OpenFileDialog();
			fdlg.Title = "Select file";
			fdlg.InitialDirectory = @"C:\JobInterview\CodeTest\CsvTransform\Resources";
			fdlg.Filter = "CSV files (*.csv)|*.csv";
			fdlg.FilterIndex = 1;
			fdlg.RestoreDirectory = true;
			if (fdlg.ShowDialog() == DialogResult.OK)
			{
				try
				{
					TransformCsvFile(fdlg.FileName);
				}
				catch (IOException ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}
		}

		private void TransformCsvFile(string filePathAndName)
		{
			byte[] sourceFileContent = File.ReadAllBytes(filePathAndName);
			CSVFileClass csvFile = CSVEngine.CSVFileLoad(sourceFileContent);

			if (!CSVEngine.CSVFileValidate2<CsvInputClass>(csvFile, out Dictionary<string, int> mapCSVColumnNameToColumnIndex, out string validationMessage))
				return;

			int lineNumber = 2;
			List<CsvInputClass> csvInputLines = new List<CsvInputClass>(csvFile.Content.Count);

			foreach (List<string> csvContentLine in csvFile.Content)
			{
				CsvInputClass csvInputLine = Activator.CreateInstance(typeof(CsvInputClass), lineNumber, csvFile.HeaderLine, csvContentLine, mapCSVColumnNameToColumnIndex) as CsvInputClass;
				csvInputLines.Add(csvInputLine);
			}

			//Method to return transformed list csvOutputLines
			//**
			List<CsvOutputClass> csvOutputLines = TransformCsvToOutputFile(csvInputLines);

			CSVFileClass outCsvFile = CSVEngine.CreateCsvFile(csvOutputLines);
			File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(filePathAndName), "OutputFile.csv"), CSVEngine.ToByteArray(outCsvFile));
		}

		private static List<CsvOutputClass> TransformCsvToOutputFile(List<CsvInputClass> csvInputLines)
		{
			/*
			* New Tool Request:

			Hi,
			We need a tool to convert data from the AA Corporation standard format to our own internal format,
			Please find the spec for the AA Corp format below


			* AA Corporation Transaction File Specification:

			Transactions are grouped by the TxNumber field, rows with the same TxNumber are in the same transaction

			In a transaction group(rows with the same TxNumber), if there's a transaction type (in the TxType field) of "Investment", use that row for the output file and ignore all of the other rows in that transaction group

			In a transaction group, if there's a transaction type of "Net Investment" and no "Investment" transaction type in the group,
			take the "Net Investment" amount, add(+) any "Initial Fee" transaction amounts in the transaction group, and subtract(-) any "Correction" transaction amounts in the transaction group.
			Since "Correction" and "Initial Fee" transactions are added to "Net investment" ammount, they should not be written by themselves to the output file.
			*/

			List<CsvOutputClass> csvOutputLines = new List<CsvOutputClass>();

			return csvOutputLines;
		}
	}
}
