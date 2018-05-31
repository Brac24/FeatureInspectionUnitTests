using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using FeatureInspection;
using System.Data;


namespace Feature_Inspection.UnitTests
{
	[TestFixture]
	class FinalInspectionPresenterTests
	{
		private Mock<IFinalInspection> mockView;
		private Mock<IFinalInspectionModel> mockModel;

		private FinalInspectionPresenter sut;
		private FinalInspectionModel _modelImpl;

		string jobNumber;
		private DataTable jobInfo;

		private List<Operation> expectedOperationList = new List<Operation>();

		[SetUp]
		public void SetUp()
		{

			mockView = new Mock<IFinalInspection>(MockBehavior.Strict); 
			mockModel = new Mock<IFinalInspectionModel>(MockBehavior.Strict);

			sut = new FinalInspectionPresenter(mockView.Object, mockModel.Object);

			jobNumber = "2123FLKDL";

			jobInfo = new DataTable();

		}

		[Test]
		public void jobTextBoxKeyDown_ValidJobNumber_SetJobProperty()
		{
			//Arrange
			mockModel.Setup(f => f.GetMarShaftBluePrintInspectedData(jobNumber)).Returns(new DataTable());
			mockModel.Setup(f => f.JobNumberExists(jobNumber)).Returns(1);
			mockModel.Setup(f => f.GetJobInfo(jobNumber)).Returns(jobInfo); //Returns empty table be do not care for this test
			mockModel.Setup(f => f.GetAllOperationsOnJob(jobNumber)).Returns(new DataTable());
			mockView.Setup(f => f.SetJobInfo(jobInfo));
			mockView.Setup(f => f.SetOperationButtons(new List<Operation>()));
			mockView.Setup(f => f.AddNCRs(new List<string>()));
			mockModel.Setup(f => f.GetNCRList(jobNumber)).Returns(new List<string>());
			mockModel.Setup(f => f.GetOperationsNotBeingInspectedThatHaveFeatures(jobNumber)).Returns(new List<string>());
			mockView.Setup(f => f.DisplayTable());
			mockView.Setup(f => f.SetMarShaftData(mockModel.Object.GetMarShaftBluePrintInspectedData(jobNumber)));


			//Act
			sut.jobTextBoxKeyDown(jobNumber);

			//Assert
			Assert.AreEqual(jobNumber, sut.JobNumber);
		}

		[Test]
		public void jobTextBoxKeyDown_InvalidJobNumberReturns0Ops_JobNumberEmpty_ClearInfo_ShowInvalidDialogBox()
		{
			//Arrange
			mockModel.Setup(f => f.JobNumberExists(jobNumber)).Returns(0);
			mockView.Setup(f => f.ClearPage_JobNumberDoesNotExist()).Verifiable();

			//Act
			sut.jobTextBoxKeyDown(jobNumber);

			//Assert
			Assert.IsEmpty(sut.JobNumber);

		}

		[Test]
		public void IntegrationTest_jobTextBoxKeyDown_ValidJobNumber_GetAndSetJobInfoView()
		{
			//Arrange
			jobNumber = "30715";
			_modelImpl = new FinalInspectionModel(); //This essentially makes it an integration test because we call the server 
			mockModel.Setup(f => f.JobNumberExists(jobNumber)).Returns(1);
			mockModel.Setup(f => f.GetJobInfo(jobNumber)).Returns(_modelImpl.GetJobInfo(jobNumber)).Verifiable(); //Accessing DB
			mockModel.Setup(f => f.GetAllOperationsOnJob(jobNumber)).Returns(new DataTable());
		    mockModel.Setup(f => f.GetMarShaftBluePrintInspectedData(jobNumber)).Returns(new DataTable());
			mockView.Setup(f => f.SetJobInfo(mockModel.Object.GetJobInfo(jobNumber))).Verifiable();
			mockView.Setup(f => f.SetOperationButtons(new List<Operation>()));
			mockView.Setup(f => f.AddNCRs(new List<string>()));
			mockView.Setup(f => f.SetMarShaftData(mockModel.Object.GetMarShaftBluePrintInspectedData(jobNumber)));
			mockModel.Setup(f => f.GetNCRList(jobNumber)).Returns(new List<string>());
			mockModel.Setup(f => f.GetOperationsNotBeingInspectedThatHaveFeatures(jobNumber)).Returns(new List<string>());
			mockView.Setup(f => f.DisplayTable());
			



			//Act 
			sut.jobTextBoxKeyDown(jobNumber);
			mockModel.Verify(f => f.GetJobInfo(jobNumber));
			mockView.Verify(f => f.SetJobInfo(mockModel.Object.GetJobInfo(jobNumber)));

			//Assert
			Assert.AreEqual(jobNumber, sut.JobNumber);

		}

		[Test]
		public void SetJobProperty_InvalidJobNumber_JobNumberPropertyNotSet()
		{
			string jobNumber = "2123FLKDL";

			//Arrange
			mockModel.Setup(f => f.JobNumberExists(jobNumber)).Returns(0);

			//Act
			bool response = sut.SetJobProperty(jobNumber);

			//Assert
			Assert.IsEmpty(sut.JobNumber);
			Assert.IsFalse(response);

		}

		[Test]
		public void SetJobProperty_ValidJobNumber_SetJobInfoCalled_JobNumberPropertySet()
		{
			mockModel.Setup(f => f.JobNumberExists(jobNumber)).Returns(1);

			bool response = sut.SetJobProperty(jobNumber);

			Assert.AreEqual(jobNumber, sut.JobNumber);
			Assert.IsTrue(response);
		}

		[TestCase("ASF4515")]
		public void IntegrationTest_SetJobProperty_InvalidJobNumber_JobPropertyEmpty(string jobNumber)
		{
			//Arrange
			_modelImpl = new FinalInspectionModel();
			mockModel.Setup(f => f.JobNumberExists(jobNumber)).Returns(_modelImpl.JobNumberExists(jobNumber));

			//Act
			sut.SetJobProperty(jobNumber);

			//Assert
			Assert.IsEmpty(sut.JobNumber);
		}

		[Test]
		public void SetJobOperationButtons_ValidNumOfOperations_CalledSetOpButton_NumOfOperationsTimes()
		{
			Operation op = new Operation();

			//Arrange
			mockModel.Setup(f => f.GetAllOperationsOnJob(jobNumber)).Returns(jobInfo);
			//mockView.Setup(f => f.SetOperationButton(opNumber, System.Drawing.Color.))
		}

		[Test]
		public void IntegrationTest_GetOperationList_QueryAllOpsAndInspectedOps_Returns2GreenAnd22GrayOperations()
		{
			jobNumber = "T01440";
			sut.JobNumber = "T01440";

			//Arrange
			FinalInspectionModel modelImp = new FinalInspectionModel();

			List<Operation> actualList = new List<Operation>();
			List<Operation> expectedList = new List<Operation>();

			expectedList = ManualListOfOps_Returns2Green_22Gray();

			mockModel.Setup(f => f.GetAllOperationsOnJob(jobNumber)).Returns(modelImp.GetAllOperationsOnJob(jobNumber));
			mockModel.Setup(f => f.GetOperationsNotBeingInspectedThatHaveFeatures(jobNumber)).Returns(modelImp.GetOperationsNotBeingInspectedThatHaveFeatures(jobNumber));
			//Act
			actualList = sut.GetOperationList();

			//Assert
			Assert.That(List.Map(actualList).Property("Color"), Is.EqualTo(List.Map(expectedList).Property("Color")));
			Assert.That(List.Map(actualList).Property("OpNumber"), Is.EqualTo(List.Map(expectedList).Property("OpNumber")));

		}

		private List<Operation> ManualListOfOps_Returns2Green_22Gray()
		{
			List<Operation> list = new List<Operation>();

			string[] ops = new string[] {"10","15", "16", "25", "40", "60", "70", "80", "90", "93", "97", "100",
										 "110", "120", "130", "135", "140", "150", "160", "170", "180", "190", "200", "210"};

			for (int i = 1; i <= ops.Count(); i++)
			{
				if (i == 12 || i == 16)
				{
					list.Add(new Operation(ops[i - 1], System.Drawing.Color.SeaGreen));
				}
				else if(i == 10)// 93
				{
					list.Add(new Operation(ops[i - 1], System.Drawing.Color.FromArgb(151,70,44)));
				}
				else
				{
					list.Add(new Operation(ops[i - 1], System.Drawing.Color.Gray));
				}

			}

			return list;

		}

		[TestCase(1)]
		[TestCase(10)]
		[TestCase(30)]
		public void GetOperationList_DataTableWithMixedInspectionCompleteAndNotComplete_ReturnsCorrectList(int numOfOps)
		{
			//Arrange
			DataTable dt = new DataTable();
			List<Operation> actualList = new List<Operation>();
			List<string> listOfOpsWithFeatures = new List<string>();
			jobNumber = "adfsdf";
			sut.JobNumber = jobNumber;
			dt = HelperMethod_GetAllOperationsOnJob_DataTableMakerWithSomeNullValuesAndInspectionComplete_Random(numOfOps);
			listOfOpsWithFeatures = SetHasFeaturesToEveryOtherGrayOpAndColoredOps();
			mockModel.Setup(f => f.GetAllOperationsOnJob(jobNumber)).Returns(dt);
			mockModel.Setup(f => f.GetOperationsNotBeingInspectedThatHaveFeatures(jobNumber)).Returns(listOfOpsWithFeatures);

			//Act
			actualList = sut.GetOperationList();

			//Assert
			Assert.That(List.Map(actualList).Property("Color"), Is.EqualTo(List.Map(expectedOperationList).Property("Color")));
			Assert.That(List.Map(actualList).Property("OpNumber"), Is.EqualTo(List.Map(expectedOperationList).Property("OpNumber")));
			Assert.That(List.Map(actualList).Property("HasFeatures"), Is.EqualTo(List.Map(expectedOperationList).Property("HasFeatures")));
		}

		private List<string> SetHasFeaturesToEveryOtherGrayOpAndColoredOps()
		{
			bool hasFeatures = true;
			var listOfOps = new List<string>();
			foreach (var op in expectedOperationList)
			{
				if (op.Color == System.Drawing.Color.FromArgb(151,70,44) || op.Color == System.Drawing.Color.SeaGreen)
				{
					op.HasFeatures = true;
					//listOfOps.Add(op.OpNumber);
				}
				else if (hasFeatures)
				{
					listOfOps.Add(op.OpNumber);
					op.HasFeatures = true;
					op.Color = System.Drawing.Color.Red;
				}
				else
				{

					op.HasFeatures = false;
					op.Color = System.Drawing.Color.Gray;

				}
				hasFeatures = !hasFeatures;
			}

			return listOfOps;
		}

		/// <summary>
		/// Creates a random DataTable as if it was coming from the GetAllOperationsOnJob method from FinalInspectionModel
		/// Only sets Op_Key1 value or InspectionComplete column because that is all we are going to check in the code
		/// This also sets a global variable list that is used for the expected list
		/// </summary>
		/// <param name="numberOfOperations"></param>
		private DataTable HelperMethod_GetAllOperationsOnJob_DataTableMakerWithSomeNullValuesAndInspectionComplete_Random(int numberOfOperations)
		{
			Random r = new Random();
			DataTable allOps = new DataTable();
			int offset = 5;
			int startRange = 0;
			int maxValue = startRange + offset;
			expectedOperationList.Clear();

			allOps.Columns.Add("Op_Key", typeof(int));
			allOps.Columns.Add("Job_Number", typeof(string));
			allOps.Columns.Add("Part_Number", typeof(string));
			allOps.Columns.Add("Operation_Number", typeof(string));
			allOps.Columns.Add("Op_Key1", typeof(int));
			allOps.Columns.Add("Inspection_Key", typeof(int));
			allOps.Columns.Add("status", typeof(string));
			allOps.Columns.Add("Lot_Size", typeof(string));
			allOps.Columns.Add("InspectionCreatedDate", typeof(string));
			allOps.Columns.Add("InspectionComplete", typeof(bool));
			allOps.Columns.Add("CompleteDate", typeof(string));
			allOps.Columns.Add("LotSerialized", typeof(string));

			//Create rows for DataTable.
			for (int i = 0; i < numberOfOperations; i++)
			{
				bool nullOpKey = (r.Next(100) <= 60) ? true : false; //null Op_Key1(Inspection.Op_Key) approx. 60% of the time
				bool inspecComplete = (r.Next(100) <= 30) ? true : false;
				int op = r.Next(startRange, maxValue); //Generate random operation number
				DataRow newRow = allOps.NewRow();

				newRow["Op_Key"] = r.Next(); //OpKey for every operation. This one pertains to Column from Operation table

				if (nullOpKey)
				{
					newRow["Op_Key1"] = DBNull.Value;
					newRow["Operation_Number"] = op.ToString();
					allOps.Rows.Add(newRow);
					expectedOperationList.Add(new Operation(op.ToString(), System.Drawing.Color.Gray));
				}
				else
				{
					newRow["Op_Key1"] = r.Next();
					newRow["InspectionComplete"] = inspecComplete;
					newRow["Operation_Number"] = op.ToString();
					allOps.Rows.Add(newRow);
					//Set the expected list global variable
					expectedOperationList.Add(new Operation(op.ToString(), (inspecComplete) ? System.Drawing.Color.SeaGreen : System.Drawing.Color.FromArgb(151,70,44)));
				}



				startRange += offset;
				maxValue += offset;

			}

			return allOps;
		}

		[TestCase(60)]
		public void GetOperationList_ListOfOpsWithFeaturesNotInOrder_SetHasFeaturesCorrectly(int numOfOps)
		{
			//Arrange
			DataTable dt = new DataTable();
			List<Operation> actualList = new List<Operation>();
			List<string> listOfOpsWithFeatures = new List<string>();
			jobNumber = "adfsdf";
			sut.JobNumber = jobNumber;
			dt = HelperMethod_GetAllOperationsOnJob_DataTableMakerWithSomeNullValuesAndInspectionComplete_Random(numOfOps);
			listOfOpsWithFeatures = SetHasFeaturesToEveryOtherGrayOpAndColoredOps();
			listOfOpsWithFeatures.Reverse(); //Reverse the order of ops
			mockModel.Setup(f => f.GetAllOperationsOnJob(jobNumber)).Returns(dt);
			mockModel.Setup(f => f.GetOperationsNotBeingInspectedThatHaveFeatures(jobNumber)).Returns(listOfOpsWithFeatures);

			//Act
			actualList = sut.GetOperationList();

			//Assert
			Assert.That(List.Map(actualList).Property("HasFeatures"), Is.EqualTo(List.Map(expectedOperationList).Property("HasFeatures")));
		}
	}
}
